using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Game_Server;
using Redux.Enum;

namespace Redux.Packets.Game
{
    public unsafe struct TradePacket
    {
        /// <summary>
        /// 4
        /// </summary>
        public uint Target;
        /// <summary>
        /// 8
        /// </summary>
        public TradeType Subtype;
        /// <summary>
        /// 12
        /// </summary>
        public uint Unknown1;
        public ushort Unknown1Low
        {
            get { return (ushort)Unknown1; }
            set { Unknown1 = (uint)((Unknown1High << 16) | value); }
        }

        public ushort Unknown1High
        {
            get { return (ushort)(Unknown1 >> 16); }
            set { Unknown1 = (uint)((value << 16) | Unknown1Low); }
        }

        public static implicit operator TradePacket(byte* ptr)
        {
            TradePacket packet = new TradePacket();
            packet.Target = *((uint*)(ptr + 4));
            packet.Subtype = (TradeType)(*((uint*)(ptr + 8)));
            packet.Unknown1 = *((uint*)(ptr + 12));
            return packet;
        }

        public static TradePacket Create(Player owner)
        {
            TradePacket packet = new TradePacket();

            if (owner.Trade != null)
                packet.Target = owner.Trade.Target.UID;

            return packet;
        }
        public static implicit operator byte[](TradePacket packet)
        {
            var buffer = new byte[20];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, 1056);
                *((uint*)(ptr + 4)) = packet.Target;
                *((uint*)(ptr + 8)) = (uint)packet.Subtype;
                *((uint*)(ptr + 12)) = packet.Unknown1;
            }
            return buffer;
        }
    }
}
