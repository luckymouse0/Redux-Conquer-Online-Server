using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Network;
using Redux.Utility;
using Redux.Packets.Game;
using Redux.Enum;
using Redux.Managers;
namespace Redux.Game_Server
{
    #region Game Server
    public unsafe sealed class GameServer 
    {
        #region Constructor
        public GameServer(string name, int port)            
        {
            //Begin ServerListener
            NetworkServer server = new NetworkServer(name)
            {
                ClientBufferSize = 0x1000,
                OnConnect = OnConnect,
                OnReceive = OnReceive,
                OnDisconnect = OnDisconnect

            };
            server.Prepare(port, 100);
            server.BeginAccept();

            //Begin World Thread
            var t = new Threading.WorldThread();
            t.CreateThread();

            //Start monster thread
        }
        #endregion
        #region Client Connect
        public static void OnConnect(NetworkClient client)
        {
            var player = new Player(client);
            player.StartExchange();
            client.Owner = player;     
        }
        #endregion
        #region Receive Data
        private unsafe static void OnReceive(NetworkClient client, byte[] buffer)
        {
            if (client.Owner == null || !(client.Owner is Player)) return;
            var user = client.Owner as Player;
            user.Crypto.Decrypt(buffer);
            if (!user.UseThreading)
            {
                user.CompleteExchange(buffer);
                return;
            }
            #region Split
            fixed (byte* ptr = buffer)
                {
                    var offset = 0;
                    while (offset < buffer.Length)
                    {
                        var size = *((ushort*)(ptr + offset)) + 8;
                        var type = *((ushort*)(ptr + offset + 2));

                        if (size < buffer.Length)
                        {
                            var sub = new byte[size];
                            fixed (byte* pSub = sub)
                            {
                                MSVCRT.memcpy(pSub, ptr + offset, size);
                                ProcessPacket(user, pSub, sub, type);
                            }
                        }
                        else if (size > buffer.Length)
                        {
                            user.Disconnect();
                            break;
                        }
                        else // size == buffer.Length
                        {
                            ProcessPacket(user, ptr, buffer, type);
                        }
                        offset += size;
                    }
                }
            
            #endregion
        }
        #endregion
        #region Socket Error
        private static void OnDisconnect(NetworkClient client)
        {
            if (client == null || client.Owner == null || !(client.Owner is Player))
                return;
            var user = client.Owner as Player;
            user.Disconnect(true);
        }
        #endregion
        #region Process Packet
        public static void ProcessPacket(Player user, byte* ptr, byte[] safePacket, ushort type)
        {
            switch (type)
            {
                case Constants.MSG_REGISTER: Process_MsgRegisterPacket(user, ptr); break;
                case Constants.MSG_CONNECT: Process_MsgConnectPacket(user, ptr); break;
                case Constants.MSG_ACTION: Process_GeneralActionPacket(user, ptr); break;
                case Constants.MSG_ITEM_ACTION: Process_ItemActionPacket(user, ptr); break;
                case Constants.MSG_TALK: Process_TalkPacket(user, ptr); break;
                case Constants.MSG_WALK: Process_WalkPacket(user, ptr); break;
                default: Console.WriteLine("Unknown packet type: " + type); break;
            }
        }
        #endregion
        #region Handlers
        #region MsgRegisterPacket
        public static void Process_MsgRegisterPacket(Player client, RegisterPacket packet)
        {
            if (!Common.ValidChars.IsMatch(packet.CharacterName) || packet.CharacterName.Length < 3 || packet.CharacterName.Length >= 16 || packet.CharacterName.ToLower().Contains("admin"))
            {
                client.Send(new TalkPacket(ChatType.Register, "Invalid character name"));
                return;
            }           
            if (Database.ServerDatabase.Context.Characters.GetByName(packet.CharacterName) != null)
            {
                client.Send(new TalkPacket(ChatType.Register, "Character name already in use"));
                return;
            }
            if (!Common.ValidCharacterMeshes.Contains(packet.Mesh))
            {
                client.Send(new TalkPacket(ChatType.Register, "Invalid character mesh " + packet.Mesh));
                return;
            }
            if (!Common.ValidBaseProfessions.Contains(packet.Profession))
            {
                client.Send(new TalkPacket(ChatType.Register, "Invalid character profession " + packet.Profession));
                return;
            }        
            client.CreateDbCharacter(packet.CharacterName, packet.Mesh, packet.Profession);
            client.Send(new TalkPacket(ChatType.Register, Constants.REPLY_OK_STR));
            
            
        }
        #endregion
        #region MsgTalkPacket
        public static void Process_TalkPacket(Player client, TalkPacket packet)
        {
            if (packet.Words[0] == Constants.COMMAND_PREFIX)
            {
                Commands.Handle(client, packet.Words.Substring(1).ToLower().Split(' '));
                return;
            }
            switch (packet.Type)
            {
                case ChatType.Talk:
                    client.SendToScreen(packet);
                    break;
                case ChatType.Whisper:
                    var target = Managers.PlayerManager.GetUser(packet.Hearer);
                    if (target != null)                    
                        target.Send(packet);                    
                    else
                    {
                        packet.Words = packet.Hearer + " is not online";
                        client.Send(packet);
                    }
                    break;
            }
        }
        #endregion
        #region MsgConnectPacket
        public static void Process_MsgConnectPacket(Player client, ConnectPacket packet)
        {
            client.Account = Database.ServerDatabase.Context.Accounts.GetByToken(packet.AccountId);
            client.UID = client.Account.UID;
            if (client.Account.Timestamp + 10 < Common.SecondsServerOnline)
            {
                Console.WriteLine("User {0} is no longer valid", client.Account.Username);
                client.Disconnect(false);
                return;
            }
            if (PlayerManager.Players.ContainsKey(client.Account.UID))
            {
                Player removedPlayer;
                if (PlayerManager.Players.TryRemove(client.Account.UID, out removedPlayer))
                {
                    Console.WriteLine("User {0} is already logged in. Disconnecting old copy of user.", client.Account.Username); 
                    removedPlayer.Disconnect();
                }
            }
            var character = Database.ServerDatabase.Context.Characters.GetByUID(client.Account.UID);
            if (character == null)
                client.Send(new TalkPacket(ChatType.Entrance, Constants.NEW_ROLE_STR));
            else
            {
                client.Send(new TalkPacket(ChatType.Entrance, Constants.REPLY_OK_STR));
                client.Populate(character);
            }
        }
        #endregion
        #region GeneralActionPacket
        public static void Process_GeneralActionPacket(Player client, GeneralActionPacket packet)
        {
            switch (packet.Action)
            {
                #region ChangeDirection
                case DataAction.ChangeDirection:
                    client.Direction = (byte)(packet.Data4 % 8);
                    client.SendToScreen(packet);
                    break;
                #endregion
                #region ChangePKMode
                case DataAction.ChangePKMode:
                    client.PKMode = (PKMode)packet.Data1;
                    client.Send(packet);
                    break;
                #endregion
                #region ChangeAction
                case DataAction.ChangeAction:
                    var action = (ActionType)packet.Data1;
                   // if (client.Action != action)
                    {
                        client.Action = action;
                        client.SendToScreen(packet);
                        if (action == ActionType.Sit || action == ActionType.Lie)
                            client.LastSitAt = Common.Clock;
                    }
                    break;
                #endregion
                #region Hotkeys
                case DataAction.Hotkeys:
                    client.Send(packet);
                    break;
                #endregion
                #region Set Location
                case DataAction.SetLocation:
                    {
                        packet.Data1 = (ushort)client.Character.Map;
                        packet.Data3Low = (ushort)client.Character.X;
                        packet.Data3High = (ushort)client.Character.Y;
                        client.Send(packet);
                        break;
                    }
                #endregion
                #region GetSurroundings
                case DataAction.GetSurroundings: MapManager.AddPlayer(client, client.Character.Map); break;
                #endregion
                #region Jump
                case DataAction.Jump: client.HandleJump(packet); break;
                #endregion
                #region Default
                default: Console.WriteLine("Unhandled MsgActionPacket type {0} from player {1}", packet.Action, client.Character.Name); break;
                #endregion
            }
        }
        #endregion
        #region ItemActionPacket
        public static void Process_ItemActionPacket(Player client, ItemActionPacket packet)
        {
            switch (packet.ActionType)
            {
                case ItemAction.Ping: client.LastPingReceived = Common.Clock; client.Send(packet); break;
                default: Console.WriteLine("Unhandled ItemActionPacket type {0} from player {1}", packet.ActionType, client.Character.Name); break;
            }
        }
        #endregion
        #region WalkPacket
        public static void Process_WalkPacket(Player client, WalkPacket packet)
        {
            client.SendToScreen(packet, true);
            packet.Direction %= 8;
            client.X += (ushort)Common.DeltaX[packet.Direction];
            client.Y += (ushort)Common.DeltaY[packet.Direction];
            client.OnMove();
            client.UpdateSurroundings();
        }
        #endregion
        #endregion
    }
    #endregion
}
