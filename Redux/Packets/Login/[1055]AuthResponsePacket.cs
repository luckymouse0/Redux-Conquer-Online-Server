using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Packets.Login
{    
    public unsafe struct AuthResponsePacket
    {
        public uint AccountId;
        public uint Data1;
        private fixed sbyte _info[Constants.MAX_NAMESIZE];
        public int ServerPort;

        public string Info
        {
            get { fixed (sbyte* ptr = _info) return new string(ptr, 0, Constants.MAX_NAMESIZE, Encoding.Default).TrimEnd('\0'); }
            set
            {
                fixed (sbyte* ptr = _info)
                {
                    MSVCRT.memset(ptr, 0, Constants.MAX_NAMESIZE);
                    value.CopyTo(ptr);
                }
            }
        }
        public static AuthResponsePacket Create()
        {
            var packet = new AuthResponsePacket();
            packet.Data1 = Constants.RESPONSE_INVALID;
            return packet;
        }
        public static implicit operator AuthResponsePacket(byte* ptr)
        {
            var packet = new AuthResponsePacket();
            packet.AccountId = *((uint*)(ptr + 4));
            packet.Data1 = *((uint*)(ptr + 8));
            MSVCRT.memcpy(packet._info, ptr + 12, Constants.MAX_NAMESIZE);
            packet.ServerPort = *((int*)(ptr + 28));
            return packet;
        }

        public static implicit operator byte[](AuthResponsePacket packet)
        {
            var buffer = new byte[32];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length+8, 1055);
                *((uint*)(ptr + 4)) = packet.AccountId;
                *((uint*)(ptr + 8)) = packet.Data1;
                packet.Info.CopyTo(ptr + 12);
                *((int*)(ptr + 28)) = packet.ServerPort;
            }
            return buffer;
        }
    }
}


