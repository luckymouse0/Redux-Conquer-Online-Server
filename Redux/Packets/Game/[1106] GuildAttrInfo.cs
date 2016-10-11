using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;
using Redux.Structures;

namespace Redux.Packets.Game
{
    public unsafe struct GuildAttrInfoPacket
    {
        public uint GuildId;
        //public uint Unknown8;
        public long Fund;
        public uint Donation;
        //public int CP;
        public int Amount;
        public GuildRank Rank;
        private fixed sbyte _leader[16];
        /*public int RequiredLevel;
        public int RequiredMetempsychosis;
        public int RequiredProfession;
        public byte Level;
        public ushort Unknown61;
        public uint Unknown63;
        public uint EnrollmentDate;
        public byte Unknown71;
        public uint Unknown72;
        public uint Unknown76;
        public uint Unknown80;
        public uint Unknown84;
        public uint Unknown88;*/
        public uint MemberCount;
        
        public string Leader
        {
            get
            {
                fixed (sbyte* ptr = _leader)
                {
                    return new string(ptr, 0, 16, Encoding.Default).TrimEnd('\0');
                }
            }
            set
            {
                fixed (sbyte* ptr = _leader)
                {
                    
                   Redux.MSVCRT.memset(ptr, 0, 16);
                    value.CopyTo(ptr);
                }
            }
        }

        public static GuildAttrInfoPacket Create(Structures.GuildAttrInfoStruct info, Structures.Guild guild)
        {
            var packet = new GuildAttrInfoPacket();
            
            packet.GuildId = info.GuildId;
            packet.Rank = info.Rank;
            //packet.EnrollmentDate = info.JoinDate;

            if (guild != null)
            {
                //packet.Unknown8 = 123456;
                packet.Fund = guild.Money;
                //packet.CP = guild.EMoney;
                packet.Donation = (uint)info.SilverDonation;
                packet.Amount = guild.Amount; // TODO: .GetSynAmount()
                packet.Leader = guild.LeaderName;
                packet.MemberCount = (uint)guild.Amount;
                //packet.RequiredLevel = guild.RequiredLevel;
                //packet.RequiredMetempsychosis = guild.RequiredMetempsychosis;
                //packet.RequiredProfession = guild.RequiredProfession;
               // packet.Level = guild.Level;
            }

            return packet;
        }

        public static implicit operator GuildAttrInfoPacket(byte* ptr)
        {
            var packet = new GuildAttrInfoPacket();
            packet.GuildId = *((uint*)(ptr + 4));
            packet.Donation = *((uint*)(ptr + 8)); //Donation
            packet.Fund = *((long*)(ptr + 12));
            packet.MemberCount = *((uint*)(ptr + 16));
            packet.Rank = *((GuildRank*)(ptr + 20));
            //packet.Amount = *((int*)(ptr + 24));
            Redux.MSVCRT.memcpy(packet._leader, ptr + 21, 16);

            /*packet.Level = *(ptr + 60);
            packet.Unknown61 = *((ushort*)(ptr + 61));
            packet.Unknown63 = *((uint*)(ptr + 63));
            packet.EnrollmentDate = *((uint*)(ptr + 67));
            packet.Unknown71 = *(ptr + 71);
            packet.Unknown72 = *((uint*)(ptr + 72));
            packet.Unknown76 = *((uint*)(ptr + 76));
            packet.Unknown80 = *((uint*)(ptr + 80));
            packet.Unknown84 = *((uint*)(ptr + 84));
            packet.Unknown88 = *((uint*)(ptr + 88));*/
            return packet;
        }

        public static implicit operator byte[](GuildAttrInfoPacket packet)
        {
            var buffer = new byte[92 + 8];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, 1106);
                *((uint*)(ptr + 4)) = packet.GuildId;
                *((uint*)(ptr + 8)) = packet.Donation;
                *((long*)(ptr + 12)) = packet.Fund;
                *((uint*)(ptr + 16)) = packet.MemberCount;
                *((GuildRank*)(ptr + 20)) = packet.Rank;
                //*((int*)(ptr + 24)) = packet.Amount;
                Redux.MSVCRT.memcpy(ptr + 21, packet._leader, 16);

                /**((GuildRank*)(ptr + 28)) = packet.Rank;
                Redux.MSVCRT.memcpy(ptr + 32, packet._leader, 16);
                *((int*)(ptr + 48)) = packet.RequiredLevel;
                *((int*)(ptr + 52)) = packet.RequiredMetempsychosis;
                *((int*)(ptr + 56)) = packet.RequiredProfession;
                *(ptr + 60) = packet.Level;
                *((ushort*)(ptr + 61)) = packet.Unknown61;
                *((uint*)(ptr + 63)) = packet.Unknown63;
                *((uint*)(ptr + 67)) = packet.EnrollmentDate;
                *(ptr + 71) = packet.Unknown71;
                *((uint*)(ptr + 72)) = packet.Unknown72;
                *((uint*)(ptr + 76)) = packet.Unknown76;
                *((uint*)(ptr + 80)) = packet.Unknown80;
                *((uint*)(ptr + 84)) = packet.Unknown84;
                *((uint*)(ptr + 88)) = packet.Unknown88;*/
            }
            return buffer;
        }
    }
}
