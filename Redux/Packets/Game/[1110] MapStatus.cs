using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Game_Server;
using Redux.Database.Domain;
namespace Redux.Packets.Game
{
    public unsafe struct MapStatusPacket
    {
        public uint UID;
        public uint ID;
        public uint Type;

        public static MapStatusPacket Create(DbMap _map)
        {
            var packet = new MapStatusPacket();
            packet.UID = _map.UID;
            packet.ID = _map.ID;
            packet.Type =   (uint)_map.Type;
            return packet;
        }
        public static implicit operator byte[](MapStatusPacket packet)
        {
            var buffer = new byte[16 + 8];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, 1110);
                *((uint*)(ptr + 4)) = packet.UID;
                *((uint*)(ptr + 8)) = packet.ID;
                *((uint*)(ptr + 12)) = packet.Type;
            }
            return buffer;
        }

    }
}
