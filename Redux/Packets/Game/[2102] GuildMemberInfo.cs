using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;

namespace Redux.Packets.Game
{
    public struct GuildMemberInfo
    {
        public const int SIZE = 25;//48;

        public string Name;
        public GuildRank Rank;
        public int Donation;

        public unsafe GuildMemberInfo(byte* ptr)
        {
            Name = new string((sbyte*)ptr, 0, 4, Encoding.Default);
            Rank = *((GuildRank*)(ptr + 8));
            Donation = *((int*)(ptr + 9));
        }

        public unsafe void Append(byte* ptr)
        {
            Redux.MSVCRT.memset(ptr, 0, 16);
            Name.CopyTo(ptr);

            *((int*)(ptr + 8)) = Donation;
            *((GuildRank*)(ptr + 9)) = Rank;
        }
    }

    public unsafe struct GuildMemberPacket
    {
        public uint Unknown4;
        public int StartIndex;
        public int Amount;
        private IList<GuildMemberInfo> _list;
        public uint UnknownEnd;

        public static bool Create(out GuildMemberPacket packet)
        {
            packet = new GuildMemberPacket();
            packet._list = new List<GuildMemberInfo>();

            return true;
        }

        public bool AddInfo(GuildMemberInfo info)
        {
            _list.Add(info);
            Amount++;

            return true;
        }

        public static implicit operator GuildMemberPacket(byte* ptr)
        {
            var packet = new GuildMemberPacket();
            packet.Unknown4 = *((uint*)(ptr + 4));
            packet.StartIndex = *((int*)(ptr + 8));
            packet.Amount = *((int*)(ptr + 12));
            packet._list = new List<GuildMemberInfo>();
            for (var i = 0; i < packet.Amount; i++)
            {
                packet._list.Add(new GuildMemberInfo(ptr + 16 + i * GuildMemberInfo.SIZE));
            }

            return packet;
        }

        public static implicit operator byte[](GuildMemberPacket packet)
        {
            var buffer = new byte[20 + packet.Amount * GuildMemberInfo.SIZE + 8];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, 2102);
                *((uint*)(ptr + 4)) = packet.Unknown4;
                *((int*)(ptr + 8)) = packet.StartIndex;
                *((int*)(ptr + 12)) = packet.Amount;
                for (var i = 0; i < packet.Amount; i++)
                {
                    packet._list[i].Append(ptr + 16 + i * GuildMemberInfo.SIZE);
                }
            }
            return buffer;
        }
    }
}
