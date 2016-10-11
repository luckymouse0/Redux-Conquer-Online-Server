using System.Text;

namespace Redux.Packets.Login
{
    public unsafe struct ConnectAuthPacket
    {
        public uint AccountId;
        public uint Data;
        private fixed sbyte _info[Constants.MAX_NAMESIZE];

        public string Info
        {
            get
            {
                fixed (sbyte* ptr = _info)
                {
                    return new string(ptr).TrimEnd('\0');
                }
            }
            set
            {
                fixed (sbyte* ptr = _info)
                {
                    MSVCRT.memset(ptr, 0, Constants.MAX_NAMESIZE);
                    value.CopyTo(ptr);
                }
            }
        }

        public static bool Create(uint accountId, uint data, string info, out ConnectAuthPacket packet)
        {
            packet = new ConnectAuthPacket();

            packet.AccountId = accountId;
            packet.Data = data;
            packet.Info = info;

            return true;
        }

        public static implicit operator byte[](ConnectAuthPacket packet)
        {
            var buffer = new byte[28];
            fixed (byte* ptr = buffer)
            {
                *((ushort*)(ptr + 0)) = 28;
                *((ushort*)(ptr + 2)) = 1052;
                *((uint*)(ptr + 4)) = packet.AccountId;
                *((uint*)(ptr + 8)) = packet.Data;
                MSVCRT.memcpy(ptr + 12, packet._info, Constants.MAX_NAMESIZE);
            }
            return buffer;
        }
    }

    public unsafe struct ConnectPacket
    {
        public uint AccountId;
        public uint Data;
        public uint MacAddress1;

        public static implicit operator ConnectPacket(byte* ptr)
        {
            var packet = new ConnectPacket();
            packet.AccountId = *((uint*)(ptr + 4));
            packet.Data = *((uint*)(ptr + 8));
            packet.MacAddress1 = *((uint*)(ptr + 12));
            return packet;
        }
    }
}
