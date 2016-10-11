using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;

namespace Redux.Packets.Game
{
    public unsafe struct GuildPackets
    {
        public GuildAction Action;
        public uint Data;
        public int RequiredLevel;
        public int RequiredMetempsychosis;
        public int RequiredProfession;
        public NetStringPacker Strings;

        public static GuildPackets Create(GuildAction action, uint targetId, uint masterId)
        {
            var packet = new GuildPackets();
            if (action == GuildAction.None) return packet;

            packet.Action = action;
            packet.Data = targetId;
            packet.Strings = new NetStringPacker();

            return packet;
        }

        public static implicit operator GuildPackets(byte* ptr)
        {
            var packet = new GuildPackets();
            packet.Action = *((GuildAction*)(ptr + 4));
            packet.Data = *((uint*)(ptr + 8));
            packet.Strings = new NetStringPacker(ptr + 24);
            return packet;
        }

        public static implicit operator byte[](GuildPackets packet)
        {
            var buffer = new byte[12 + packet.Strings.Length + 8];//28
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, 1107);
                *((GuildAction*)(ptr + 4)) = packet.Action;
                *((uint*)(ptr + 8)) = packet.Data;
                PacketBuilder.AppendNetStringPacker(ptr + 24, packet.Strings);
            }
            return buffer;
        }
    }
}
