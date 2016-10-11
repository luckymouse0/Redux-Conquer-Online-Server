using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Packets.Login
{
    public unsafe struct AccountExPacket
    {
        private fixed sbyte _account[Constants.MAX_NAMESIZE];
        private fixed sbyte _password[Constants.MAX_NAMESIZE];
        private fixed sbyte _server[16];
        public string DecryptedPassword;
        public string Account
        {
            get { fixed (sbyte* ptr = _account) return new string(ptr, 0, Constants.MAX_NAMESIZE, Encoding.Default).TrimEnd('\0'); }
            set
            {
                fixed (sbyte* ptr = _account)
                {
                    MSVCRT.memset(ptr, 0, Constants.MAX_NAMESIZE);
                    value.CopyTo(ptr);
                }
            }
        }

        public string Password
        {
            get { fixed (sbyte* ptr = _password) return new string(ptr, 0, Constants.MAX_NAMESIZE, Encoding.Default).TrimEnd('\0'); }
            set
            {
                fixed (sbyte* ptr = _password)
                {
                    MSVCRT.memset(ptr, 0, Constants.MAX_NAMESIZE);
                    value.CopyTo(ptr);
                }
            }
        }

        public string Server
        {
            get { fixed (sbyte* ptr = _server) return new string(ptr, 0, 16, Encoding.Default).TrimEnd('\0'); }
            set
            {
                fixed (sbyte* ptr = _server)
                {
                    MSVCRT.memset(ptr, 0, Constants.MAX_NAMESIZE);
                    value.CopyTo(ptr);
                }
            }
        }

        public void Dump()
        {
            //Kernel.WriteLine("AccountPacket.Dump()\r\nAccount: {0}\r\nPassword: {1}\r\nServer: {2}", Account, Password, Server);
        }

        public static implicit operator AccountExPacket(byte* ptr)
        {
            var packet = new AccountExPacket();
            MSVCRT.memcpy(packet._account, ptr + 4, 16);
            new Cryptography.RC5().Decrypt(ptr + 20, 16);
            MSVCRT.memcpy(packet._password, ptr + 20, 16);
            MSVCRT.memcpy(packet._server, ptr + 36, 16);
            return packet;
        }

        public static implicit operator byte[](AccountExPacket packet)
        {
            return null;
        }
    }
}