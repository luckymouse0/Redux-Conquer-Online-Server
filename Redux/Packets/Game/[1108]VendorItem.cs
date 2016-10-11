using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;
using Redux.Game_Server;

namespace Redux.Packets.Game
{
    public unsafe struct VendorItem
    {
        public uint TargetID;
        public uint Price;
        public uint Unknown2;
        public uint Unknown3;
        public ushort Unknown4;
        public byte Unknown5;
        public ushort ViewType;
        public Structures.ConquerItem Item;

        public static VendorItem Create(uint owner, Structures.ConquerItem item)
        {
            var packet = new VendorItem();
            packet.TargetID = owner;
            packet.Item = item;
            packet.Price = packet.Unknown2 = packet.Unknown3 = 0;
            packet.Unknown4 = 0;
            packet.Unknown5 = 0;
            packet.ViewType = 4;//I assume this is a subtype
            return packet;
        }
        public static implicit operator byte[](VendorItem packet)
        {
            var buffer = new byte[84 + 8];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, 1108);
                *((uint*)(ptr + 4)) = packet.Item.UniqueID;
                *((uint*)(ptr + 8)) = packet.TargetID;
                *((uint*)(ptr + 12)) = packet.Price;
                *((uint*)(ptr + 16)) = packet.Item.StaticID;
                *((ushort*)(ptr + 20)) = packet.Item.Durability;
                *((ushort*)(ptr + 22)) = packet.Item.MaximumDurability;
                *((ushort*)(ptr + 24)) = packet.ViewType;
                *((ushort*)(ptr + 26)) = (ushort)packet.Item.Location;
                *((uint*)(ptr + 28)) = packet.Unknown2;
                *((byte*)(ptr + 32)) = packet.Item.Gem1;
                *((byte*)(ptr + 33)) = packet.Item.Gem2;
                *((uint*)(ptr + 34)) = packet.Unknown3;
                //*((ushort*)(ptr + 38)) = packet.Unknown4;
                //*((byte*)(ptr + 40)) = packet.Unknown5;
                *((byte*)(ptr + 36)) = packet.Item.Plus;
                *((byte*)(ptr + 37)) = packet.Item.Bless;
                *((uint*)(ptr + 38)) = packet.Item.Enchant;
                *((byte*)(ptr + 48)) = packet.Item.Color;
                
            }
            return buffer;
        }
    }
}
