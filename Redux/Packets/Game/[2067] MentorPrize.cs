using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;
using Redux.Game_Server;
using Redux.Space;

namespace Redux.Packets.Game
{
    public unsafe struct MentorPrize
    {
        public uint Prize_type;
        public uint mentor_id;
        public ulong Prize_exp;
        public ushort Prize_heaven_blessed;
        public ushort Prize_Plus_Stones;

        public static MentorPrize Create(Player user)
        {
            MentorPrize packet = new MentorPrize();
            
            return packet;
        }

        public static implicit operator MentorPrize(byte* ptr)
        {
            var packet = new MentorPrize();
            packet.Prize_type = *((uint*)(ptr + 4));
            packet.mentor_id = *((uint*)(ptr + 8));
            packet.Prize_exp = *((ulong*)(ptr + 24));
            packet.Prize_heaven_blessed = *((ushort*)(ptr + 32));
            packet.Prize_Plus_Stones = *((ushort*)(ptr + 34));
            return packet;
        }

        public static implicit operator byte[](MentorPrize packet)
        {
            byte[] data = new byte[50];
            fixed (byte* ptr = data)
            {
                PacketBuilder.AppendHeader(ptr, data.Length, 2067);
                *((uint*)(ptr + 4)) = packet.Prize_type;
                *((uint*)(ptr + 8)) = packet.mentor_id;
                *((ulong*)(ptr + 24)) = packet.Prize_exp;
                *((ushort*)(ptr + 32)) = packet.Prize_heaven_blessed;
                *((ushort*)(ptr + 34)) = packet.Prize_Plus_Stones;
            }
            return data;
        }
    }
}
