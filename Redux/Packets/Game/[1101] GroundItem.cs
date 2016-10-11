using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Database.Domain;
using Redux.Structures;
using Redux.Enum;
using Redux.Game_Server;
namespace Redux.Packets.Game
{
    public unsafe struct GroundItemPacket
    {
        public uint UID;
        public uint ID;
        public ushort X;
        public ushort Y;
        public byte Color;
        public GroundItemAction Action;
        public static GroundItemPacket Create(GroundItem _item, GroundItemAction _action)
        {
            var packet = new GroundItemPacket();
            packet.UID = _item.UID;
            packet.ID = _item.StaticID;
            packet.X = (ushort)_item.Location.X;
            packet.Y = (ushort)_item.Location.Y;
            packet.Color = _item.Item.Color;
            packet.Action = _action;
            return packet;
        }
        public static implicit operator byte[](GroundItemPacket packet)
        {
            var buffer = new byte[28];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, Constants.MSG_GROUND_ITEM);
                *((uint*)(ptr + 4)) = packet.UID;
                *((uint*)(ptr + 8)) = packet.ID;
                *((ushort*)(ptr + 12)) = packet.X;
                *((ushort*)(ptr + 14)) = packet.Y;
                *((byte*)(ptr + 16)) = packet.Color;
                *((GroundItemAction*)(ptr + 18)) = packet.Action;
            }
            return buffer;
        }
        public static implicit operator GroundItemPacket(byte* ptr)
        {
            var packet = new GroundItemPacket();
            packet.UID = *((uint*)(ptr + 4));
            packet.ID = *((uint*)(ptr + 8));
            packet.X = *((ushort*)(ptr + 12));
            packet.Y = *((ushort*)(ptr + 14));
            packet.Color = *((byte*)(ptr + 16));
            packet.Action = *((GroundItemAction*)(ptr + 18));
            return packet;
        }
    }
}
