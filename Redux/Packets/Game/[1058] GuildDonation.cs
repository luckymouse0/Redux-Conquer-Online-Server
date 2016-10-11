using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;
using Redux.Structures;

namespace Redux.Packets.Game
{

    public unsafe struct GuildDonationPacket
    {
        public GuildDonationFlags UpdateFlags;
        public int Money;
        public int EMoney;
        public int Guide;
        public int Pk;
        public int Arsenal;
        public uint Rose;
        public uint Orchid;
        public uint Lily;
        public uint Tulip;
        public uint Exploits;
        public uint Unknown48;

        public static GuildDonationPacket Create(Structures.GuildAttrInfoStruct info)
        {
            var packet = new GuildDonationPacket();
            packet.UpdateFlags = GuildDonationFlags.AllDonations;
            packet.Money = 2;// info.SilverDonation;
            packet.EMoney = 3;// info.CPDonation;
            packet.Guide = 4;// info.GuideDonation;
            packet.Pk = 5;// info.PkDonation;
            packet.Arsenal = 6;// info.ArsenalDonation;

            return packet;
        }

        public static implicit operator GuildDonationPacket(byte* ptr)
        {
            var packet = new GuildDonationPacket();
            packet.UpdateFlags = *((GuildDonationFlags*)(ptr + 4));
            return packet;
        }

        public static implicit operator byte[](GuildDonationPacket packet)
        {
            var buffer = new byte[52 + 8];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, 1058);
                *((GuildDonationFlags*)(ptr + 4)) = packet.UpdateFlags;
                *((int*)(ptr + 8)) = packet.Money;
                *((int*)(ptr + 12)) = packet.EMoney;
                *((int*)(ptr + 16)) = packet.Guide;
                *((int*)(ptr + 20)) = packet.Pk;
                *((int*)(ptr + 24)) = packet.Arsenal;
                *((uint*)(ptr + 28)) = packet.Rose;
                *((uint*)(ptr + 32)) = packet.Orchid;
                *((uint*)(ptr + 36)) = packet.Lily;
                *((uint*)(ptr + 40)) = packet.Tulip;
                *((uint*)(ptr + 44)) = packet.Exploits;
                *((uint*)(ptr + 48)) = packet.Unknown48;
            }
            return buffer;
        }
    }
}
