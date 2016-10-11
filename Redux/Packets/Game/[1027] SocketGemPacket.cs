using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Structures;
using Redux.Enum;
using Redux.Database.Domain;

namespace Redux.Packets.Game
{
    public unsafe struct SocketGemPacket
    {
        public uint CharacterUID;
        public uint ItemID;
        public uint GemID;
        public ushort Location;
        public SocketGemAction Action;

        public static implicit operator byte[](SocketGemPacket packet)
        {
            var buffer = new byte[28];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, Constants.MSG_SOCKET_GEM);
                *((uint*)(ptr + 4)) = packet.CharacterUID;
                *((uint*)(ptr + 8)) = packet.ItemID;
                *((uint*)(ptr + 12)) = packet.GemID;
                *((ushort*)(ptr + 12)) = packet.Location;
                *((SocketGemAction*)(ptr + 18)) = packet.Action;
            }
            return buffer;
        }
        public static implicit operator SocketGemPacket(byte* ptr)
        {
            SocketGemPacket packet = new SocketGemPacket();
            packet.CharacterUID = *((uint*)(ptr + 4));
            packet.ItemID = *((uint*)(ptr + 8));
            packet.GemID = *((uint*)(ptr + 12));
            packet.Location = *((ushort*)(ptr + 16));
            packet.Action = *((SocketGemAction*)(ptr + 18));
            return packet;
        }

    }
}
