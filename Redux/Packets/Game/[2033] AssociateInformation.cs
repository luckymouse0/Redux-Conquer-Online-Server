using Redux.Enum;
using Redux.Game_Server;
using System;

namespace Redux.Packets.Game
{
    public unsafe struct AssociateInformationPacket
    {
         	
        public uint UID;	
        public uint Lookface;	
        public byte Level;
        public byte Profession;
        public short PKP;
        public ushort GuildID;	
        public byte GuildRank;
        public bool IsEnemy; 	
        private fixed sbyte _spousename[Constants.MAX_NAMESIZE];

        public string SpouseName
        {
            get
            {
                fixed (sbyte* ptr = _spousename)
                {
                    return new string(ptr).TrimEnd('\0');
                }
            }
            set
            {
                fixed (sbyte* ptr = _spousename)
                {
                    MSVCRT.memset(ptr, 0, Constants.MAX_NAMESIZE);
                    value.CopyTo(ptr);
                }
            }
        }

        public static AssociateInformationPacket Create(Player player)
        {
            AssociateInformationPacket packet = new AssociateInformationPacket();
            packet.UID = player.UID;
            packet.Lookface = player.Lookface;
            packet.Level = player.Level;
            packet.Profession = (byte)player.Character.Profession;
            packet.PKP = player.PK;
            packet.GuildID = (ushort)player.GuildId;
            packet.GuildRank = (byte)player.GuildRank;
            packet.SpouseName = player.Spouse;
            return packet;
        }





        public static implicit operator AssociateInformationPacket(byte* ptr)
        {
            var packet = new AssociateInformationPacket();
            packet.UID = *(uint*)(ptr + 4);
            packet.Lookface = *(uint*)(ptr + 8);
            packet.Level = *(byte*)(ptr + 12);
            packet.Profession = *(byte*)(ptr + 13);
            packet.PKP = *(short*)(ptr + 14);
            packet.GuildID = *(ushort*)(ptr + 16);
            packet.GuildRank = *(byte*)(ptr + 19);
            return packet;
        }

        public static implicit operator byte[](AssociateInformationPacket packet)
        {
            var buffer = new byte[54];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, Constants.MSG_ASSOCIATE_INFO);
                *((uint*)(ptr + 4)) = packet.UID;
                *((uint*)(ptr + 8)) = packet.Lookface;
                *((byte*)(ptr + 12)) = packet.Level;
                *((byte*)(ptr + 13)) = packet.Profession;
                *((short*)(ptr + 14)) = packet.PKP;
                *((ushort*)(ptr + 16)) = packet.GuildID;
                *((byte*)(ptr + 18)) = 0;
                *((byte*)(ptr + 19)) = packet.GuildRank;
                *((bool*)(ptr + 36)) = packet.IsEnemy;
                MSVCRT.memcpy(ptr + 20, packet._spousename, Constants.MAX_NAMESIZE);
                
            }
            return buffer;
        }
    }
}
