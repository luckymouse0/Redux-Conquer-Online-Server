using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Redux.Cryptography;
using Redux.Network;
namespace Redux.Login_Server
{
    public class LoginPlayer 
    {
        #region Variables
        public NetworkClient Socket { get; private set; }
        public AuthCryptography Cryptographer { get; private set; }
        public Account Account;
        private byte[] SendBuffer;
        public const int BaseSendBufferSize = 512;
        #endregion
        #region Constructor
        public LoginPlayer(NetworkClient client)
        {
            Socket = client;
            Cryptographer = new AuthCryptography();
            SendBuffer = new byte[BaseSendBufferSize];
        }
        #endregion
        #region Methods
        public void Send(byte[] buffer)
        {
            Cryptographer.Encrypt(buffer, buffer, buffer.Length);
            Socket.Send(buffer);
        }
        public unsafe void Send(void* ptr)
        {
            var size = *((ushort*)ptr);
            var chunk = new byte[size];
            Cryptographer.Encrypt((byte*)ptr, chunk, size);
            Socket.Send(chunk);
        }
        public void Disconnect()
        {
            if(Socket.Alive)
                Socket.Disconnect();
        }
        #endregion
    }
}
