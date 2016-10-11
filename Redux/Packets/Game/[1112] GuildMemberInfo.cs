using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;
using Redux.Structures;

namespace Redux.Packets.Game
{

    public unsafe struct GuildMemberInformation
    {
        GuildAction Type;
        public GuildRank Rank;
        public int Donation;
        private fixed sbyte _name[16];

        public string Name
        {
            get
            {
                fixed (sbyte* ptr = _name)
                {
                    return new string(ptr, 0, 16, Encoding.Default).TrimEnd('\0');
                }
            }
            set
            {
                fixed (sbyte* ptr = _name)
                {

                    Redux.MSVCRT.memset(ptr, 0, 16);
                    value.CopyTo(ptr);
                }
            }
        }

        public static GuildMemberInformation Create(string Name)
        {
            var packet = new GuildMemberInformation();
            packet.Name = Name;// GuildDonationFlags.AllDonations;
            packet.Rank = GuildRank.DeputyLeader;// info.SilverDonation;
            packet.Donation = 3;// info.CPDonation;
           

            return packet;
        }

        public static implicit operator GuildMemberInformation(byte* ptr)
        {
            var packet = new GuildMemberInformation();
            packet.Type = *((GuildAction*)(ptr + 2));
            packet.Donation = *((int*)(ptr + 4));
            packet.Rank = *((GuildRank*)(ptr + 8));
            Redux.MSVCRT.memcpy(packet._name, ptr + 9, 16);
            return packet;
        }

        public static implicit operator byte[](GuildMemberInformation packet)
        {
            var buffer = new byte[28];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, 1112);
                *((GuildAction*)(ptr + 2)) = packet.Type;
                *((int*)(ptr + 4)) = packet.Donation;
                *((GuildRank*)(ptr + 8)) = packet.Rank;
                Redux.MSVCRT.memcpy(ptr + 9, packet._name, 16);
                
            }
            return buffer;
        }
    }
}
