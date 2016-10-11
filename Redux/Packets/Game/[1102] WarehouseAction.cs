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
    public unsafe struct WarehouseActionPacket
    {       
        public uint UID;
        public WarehouseAction Action;
        public WarehouseType Type;
        public ushort Unknown;
        public uint Value;
        private List<ConquerItem> Items;

        public void AddItem(ConquerItem _item)
        {
            if (Items == null)
                Items = new List<ConquerItem>();
            Items.Add(_item);
            Value = (uint)Items.Count;
        }

        public static implicit operator WarehouseActionPacket(byte* ptr)
        {
            var packet = new WarehouseActionPacket();
            packet.UID = *((uint*)(ptr + 4));
            packet.Action = *((WarehouseAction*)(ptr + 8));
            packet.Type = *((WarehouseType*)(ptr + 9));
            packet.Unknown = *((ushort*)(ptr + 10));
            packet.Value = *((uint*)(ptr + 12));          
            return packet;
        }

        public static implicit operator byte[](WarehouseActionPacket data)
        {
            const int SIZE = 24;
            var packet = new byte[24 + SIZE * (data.Action == WarehouseAction.ListItems ? data.Value : 1)];
            fixed (byte* ptr = packet)
            {
                PacketBuilder.AppendHeader(ptr, packet.Length, Constants.MSG_WAREHOUSE_ACTION);
                *((uint*)(ptr + 4)) = data.UID;
                *((WarehouseAction*)(ptr + 8)) = data.Action;
                *((WarehouseType*)(ptr + 9)) = data.Type;
                *((ushort*)(ptr + 10)) = data.Unknown;
                *((uint*)(ptr + 12)) = data.Value;
                if (data.Action == WarehouseAction.ListItems && data.Items != null)
                    for (var i = 0; i < data.Value; i++)
                    {
                        *((uint*)(ptr + 16 + i * SIZE)) = data.Items[i].UniqueID;
                        *((uint*)(ptr + 20 + i * SIZE)) = data.Items[i].StaticID;
                        *((byte*)(ptr + 25 + i * SIZE)) = data.Items[i].Gem1;
                        *((byte*)(ptr + 26 + i * SIZE)) = data.Items[i].Gem2;
                        *((byte*)(ptr + 29 + i * SIZE)) = data.Items[i].plus;
                        *((byte*)(ptr + 30 + i * SIZE)) = data.Items[i].Bless;
                        *((byte*)(ptr + 32 + i * SIZE)) = data.Items[i].Enchant;
                        *((byte*)(ptr + 39 + i * SIZE)) = data.Items[i].Color;
                    }
            }
                  
            return packet;
            }

    }
}
