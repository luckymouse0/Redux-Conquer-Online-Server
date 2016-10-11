using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Network;
using Redux.Login_Server;
using Redux.Packets.Login;
using Redux.Utility;

namespace Redux.Login_Server
{
    #region Login Server
    public class LoginServer
    {
        public  ThreadSafeCounter LoginCounter = new ThreadSafeCounter(); 
        #region Constructor
        public LoginServer(string name, int port)            
        {
            //Begin ServerListener
            NetworkServer server = new NetworkServer(name)
            {
                ClientBufferSize = 0x400,
                OnConnect = Auth_OnConnect,
                OnReceive = Auth_OnReceive
            };
            server.Prepare(port, 100);
            server.BeginAccept();
        }
        #endregion
        #region Client Connect
        private unsafe void Auth_OnConnect(NetworkClient client)
        {
            var user = new LoginPlayer(client);            
            client.Owner = user;
        }
        #endregion
        #region Receive Data
        private unsafe void Auth_OnReceive(NetworkClient client, byte[] buffer)
        {
            var user = client.Owner as LoginPlayer;
            if (user == null) return;
            user.Cryptographer.Decrypt(buffer, buffer, buffer.Length); 
            fixed (byte* ptr = buffer)
            {
                var type = *((ushort*)(ptr + 2));
                Process(user, ptr, buffer, type);
            }
                        
        }
        #endregion
        #region Process Packet
        public unsafe void Process(LoginPlayer client, byte* packet, byte[] safePacket, ushort type)
        {
            switch (type)
            {
                case 1051: Process_AuthAccountExPacket(client, packet); break;
               // case 1100: Process_AuthMacAddressPacket(client, packet); break;
                default:
                    {
                       // Console.WriteLine("Unknown packet type: " + type);
                        client.Socket.Disconnect();
                        break;
                    }
            }
        }

        private unsafe void Process_AuthAccountExPacket(LoginPlayer client, AccountExPacket packet)
        {
            var account = Database.ServerDatabase.Context.Accounts.GetByName(packet.Account);
           
            AuthResponsePacket reply = AuthResponsePacket.Create();

            if (account == null)
            {
                if (Constants.DEBUG_MODE)
                {

                    account = new Database.Domain.DbAccount();
                    account.Username = packet.Account;
                    account.Password = packet.Password;
                    account.Permission = Enum.PlayerPermission.GM;
                    account.EMail = "";
                    account.Answer = "";
                    account.Question = "";
                    Database.ServerDatabase.Context.Accounts.AddOrUpdate(account);

                    client.Account = new Account(account);
                    client.Account.Token = (uint)LoginCounter.Counter;
                    client.Account.AllowLogin();
                    reply.Data1 = Constants.RESPONSE_VALID;
                    reply.ServerPort = Constants.GAME_PORT;
                    reply.Info = Constants.GAME_IP;
                    reply.AccountId = account.Token;
                    Console.WriteLine("Client {0} Connecting to server {1}", packet.Account, packet.Server);
                }
                else
                    reply.Data1 = Constants.RESPONSE_INVALID;
            }

            else if (account.Permission == Enum.PlayerPermission.Banned)
                reply.Data1 = Constants.RESPONSE_BANNED;
            else if (account.Password.Equals(packet.Password))
            {
                client.Account = new Account(account);
                client.Account.Token = (uint)LoginCounter.Counter;
                client.Account.AllowLogin();
                reply.Data1 = Constants.RESPONSE_VALID;
                reply.ServerPort = Constants.GAME_PORT;
                reply.Info = Constants.GAME_IP;
                reply.AccountId = account.Token;
                Console.WriteLine("Client {0} Connecting to server {1}", packet.Account, packet.Server);
            }           
            client.Send(reply);            
        }
        private void Process_AuthMacAddressPacket(LoginPlayer client, MacAddressPacket packet)
        {
            ConnectAuthPacket msg;
            if (ConnectAuthPacket.Create(packet.AccountId, 20, "version.dat", out msg))
            {
                client.Send(msg);
            }
        }
        #endregion
    }
    #endregion
}
