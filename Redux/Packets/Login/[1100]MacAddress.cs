using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Packets.Login
{
    public unsafe struct MacAddressPacket
    {
        public uint AccountId;
        private fixed sbyte _addr[Constants.MACSTR_SIZE];

        public string MacAddress
        {
            get
            {
                fixed (sbyte* ptr = _addr)
                {
                    return new string(ptr, 0, Constants.MACSTR_SIZE, Encoding.Default).Split('\0')[0];
                }
            }
            set
            {
                fixed (sbyte* ptr = _addr)
                {
                    MSVCRT.memset(ptr, 0, Constants.MACSTR_SIZE);
                    value.CopyTo(ptr);
                }
            }
        }

        public static implicit operator MacAddressPacket(byte* ptr)
        {
            var packet = new MacAddressPacket();
            packet.AccountId = *((uint*)(ptr + 4));
            MSVCRT.memcpy(packet._addr, ptr + 8, 1100);
            return packet;
        }
    }
}