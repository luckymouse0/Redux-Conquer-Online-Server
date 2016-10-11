using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;

namespace Redux.Packets.Game
{
    public unsafe struct ItemActionPacket
    {
        public uint UID;
        public uint ID;
        public ItemAction ActionType;
        public uint Timestamp;
        public uint Amount;
        public static ItemActionPacket Create(uint _uid, uint _id, ItemAction _action)
        {
            var packet = new ItemActionPacket();
            packet.UID = _uid;
            packet.ID = _id;
            packet.ActionType = _action;
            packet.Timestamp = (uint)Common.Clock;
            return packet;
        }
        public static implicit operator ItemActionPacket(byte* ptr)
        {
            ItemActionPacket packet = new ItemActionPacket();
            packet.UID = *((uint*)(ptr + 4));
            packet.ID = *((uint*)(ptr + 8));
            packet.ActionType = *((ItemAction*)(ptr + 12));
            packet.Timestamp = *((uint*)(ptr + 16));
            packet.Amount = *((uint*)(ptr + 20));
            return packet;
        }

        public static implicit operator byte[](ItemActionPacket packet)
        {
            var data = new byte[24 + 8];
            fixed(byte* ptr = data)
            {
                PacketBuilder.AppendHeader(ptr, data.Length, 1009);
                *((uint*)(ptr + 4)) = packet.UID;
                *((uint*)(ptr + 8)) = packet.ID;
                *((ItemAction*)(ptr + 12)) = packet.ActionType;
                *((uint*)(ptr + 16)) = packet.Timestamp;
                *((uint*)(ptr + 20)) = packet.Amount;
            }
            return data;
        }
    }
}
