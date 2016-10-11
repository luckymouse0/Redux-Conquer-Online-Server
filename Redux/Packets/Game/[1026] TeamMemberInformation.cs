using Redux.Enum;
using Redux.Game_Server;

namespace Redux.Packets.Game
{
    public unsafe struct TeamMemberInformation
    {
        #region Name
        private fixed sbyte _name[Constants.MAX_NAMESIZE];
        public string Name
        {
            get
            {
                fixed (sbyte* ptr = _name)
                {
                    return new string(ptr).TrimEnd('\0');
                }
            }
            set
            {
                fixed (sbyte* ptr = _name)
                {
                    MSVCRT.memset(ptr, 0, Constants.MAX_NAMESIZE);
                    value.CopyTo(ptr);
                }
            }
        }
        #endregion

        public uint MemberID;
        public uint Mesh;
        public ushort Life;
        public ushort MaximumLife;
        public TeamMemberAction Action;

        public static TeamMemberInformation Create(Player member, TeamMemberAction action)
        {
            var packet = new TeamMemberInformation();
            packet.Action = action;
            packet.MemberID = member.UID;
            packet.Mesh = member.Lookface;
            packet.Life = (ushort)member.Life;
            packet.MaximumLife = (ushort)member.MaximumLife;
            packet.Name = member.Name;
            return packet;
        }

        public static implicit operator byte[](TeamMemberInformation packet)
        {
            var buffer = new byte[44];//36
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, 1026);
               // *((ushort*)(ptr + 2)) = 0x402;
                *((byte*)(ptr + 5)) = (byte)1;
                MSVCRT.memcpy(ptr + 8, packet._name, 16);
                *((uint*)(ptr + 24)) = packet.MemberID;
                *((uint*)(ptr + 28)) = packet.Mesh;
                *((ushort*)(ptr + 32)) = packet.MaximumLife;
                *((ushort*)(ptr + 34)) = packet.Life;
                }
            return buffer;
        }
    }
}
