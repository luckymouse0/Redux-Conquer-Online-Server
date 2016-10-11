using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;
namespace Redux.Packets.Game
{
    public unsafe struct ItemInformationPacket
    {
        public uint UniqueID;
        public uint StaticID;
        public ushort Durability;
        public ushort MaximumDurability;
        public ItemInfoAction Action;
        public byte Ident;
        public ItemLocation Location;
        public byte Unknown1;
        public byte Gem1;
        public byte Gem2;
        public byte Magic1;
        public byte Magic2;
        public byte Plus;
        public byte Bless;
        public byte Enchant;
        public bool Locked;
        public byte Color;

        public static ItemInformationPacket Create(Structures.ConquerItem item, ItemInfoAction createType = ItemInfoAction.AddItem)
        {
            var packet = new ItemInformationPacket();
            packet.UniqueID = item.UniqueID;
            packet.StaticID = item.StaticID;
            packet.Durability = item.Durability;
            packet.MaximumDurability = item.MaximumDurability;
            packet.Action = createType;
            packet.Location = item.Location;
            packet.Gem1 = item.Gem1;
            packet.Gem2 = item.Gem2;
            packet.Plus = item.Plus;
            packet.Bless = item.Bless;
            packet.Enchant = item.Enchant;
            packet.Color = item.Color;
            packet.Locked = item.Locked;
            return packet;
        }
        public static ItemInformationPacket CreateObserveItem(Structures.ConquerItem _item, uint _owner)
        {
            var packet = new ItemInformationPacket();
            packet.UniqueID = _owner;
            packet.StaticID = _item.StaticID;
            packet.Durability = _item.Durability;
            packet.MaximumDurability = _item.MaximumDurability;
            packet.Action = ItemInfoAction.OtherPlayerEquipment;
            packet.Location = _item.Location;
            packet.Gem1 = _item.Gem1;
            packet.Gem2 = _item.Gem2;
            packet.Plus = _item.Plus;
            packet.Bless = _item.Bless;
            packet.Enchant = _item.Enchant;
            packet.Color = _item.Color;
            packet.Locked = _item.Locked;
            return packet;
        }
        
        public static implicit operator byte[](ItemInformationPacket packet)
        {
            var buffer = new byte[52];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, Constants.MSG_ITEM_INFORMATION);
                *((uint*)(ptr + 4)) = packet.UniqueID;
                *((uint*)(ptr + 8)) = packet.StaticID;
                *((ushort*)(ptr + 12)) = packet.Durability;
                *((ushort*)(ptr + 14)) = packet.MaximumDurability;
                *((ItemInfoAction*)(ptr + 16)) = packet.Action;
                *(ptr + 17) = packet.Ident;
                *(ptr + 18) = (byte)packet.Location;
                *(ptr + 19) = packet.Unknown1;
                *(ptr + 24) = packet.Gem1;
                *(ptr + 25) = packet.Gem2;
                *(ptr + 26) = packet.Magic1;
                *(ptr + 28) = packet.Plus;
                *(ptr + 29) = packet.Bless;
                *(ptr + 30) = packet.Enchant;
                *(ptr + 40) = packet.Color;
            }
            return buffer;
        }
    }
}
