using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Redux.Database;
using Redux.Database.Domain;
using Redux.Game_Server;
using Redux.Packets.Game;
using Redux.Enum;
using Redux.Structures;

namespace Redux.Managers
{
    public class WarehouseManager
    {
        public WarehouseManager(Player _owner)
        {
            owner = _owner;
            items = new Dictionary<uint, Dictionary<uint, ConquerItem>>();
        }
        private Dictionary<uint, Dictionary<uint, ConquerItem>> items;
        private Player owner;

        public void Process_WarehouseActionPacket(WarehouseActionPacket _packet)
        {
            switch (_packet.Action)
            {
                case WarehouseAction.ListItems:
                    SendContents(_packet.UID);
                    break;

                case WarehouseAction.AddItem:
                    if (owner.Inventory.ContainsKey(_packet.Value))
                    {
                        var item = owner.GetItemByUID(_packet.Value);
                        if (item != null && item.IsStoreable)
                        {
                            item.Location = (ItemLocation)_packet.UID;
                            item.Save();
                            owner.RemoveItem(item);
                            LoadItem(item);
                            owner.Send(ItemActionPacket.Create(item.UniqueID, 0, ItemAction.SellToNPC));
                            SendContents(_packet.UID);
                        }
                    }
                    break;

                case WarehouseAction.RemoveItem:
                    {
                        ConquerItem item;
                        TryGetItem(_packet.UID, _packet.Value, out item);
                        if (item != null)
                        {
                            item.Location = ItemLocation.Inventory;
                            item.Save();
                            owner.AddItem(item);
                            RemoveItem(_packet.UID, _packet.Value);
                            owner.Send(ItemInformationPacket.Create(item));
                            SendContents(_packet.UID);                 
                        }
                        break;
                    }
                default:
                    Console.WriteLine("Unhandled warehouse action type {0} for client {1}", _packet.Action, owner.Name);
                    break;
            }
        }

        public bool HasItem(uint _whID, uint _itemID)
        {
            return items.ContainsKey(_whID) && items[_whID].ContainsKey(_itemID);
        }

        public bool TryGetItem(uint _whID, uint _itemID, out ConquerItem _item)
        {
            _item = null;
            if (HasItem(_whID, _itemID))
            {
                _item = items[_whID][_itemID];
                return true;
            }
            else
                return false;
        }

        public void RemoveItem(uint _whID, uint _itemID)
        {
            if (HasItem(_whID, _itemID))
            {
                items[_whID].Remove(_itemID);
                if (items[_whID].Count == 0)
                    items.Remove(_whID);
            }
        }

        public void SendContents(uint _id)
        {
            var packet = new WarehouseActionPacket()
            {
                UID = _id,
                Action = WarehouseAction.ListItems,
                Type = WarehouseType.Storage,
            };
            if(items.ContainsKey(_id))
                foreach (var item in items[_id].Values)
                packet.AddItem(item);
            owner.Send(packet);
        }

        public void LoadItem(ConquerItem _item)
        {
            if (!items.ContainsKey((uint)_item.Location))
                items.Add((uint)_item.Location, new Dictionary<uint, ConquerItem>());
            if (!items[(uint)_item.Location].ContainsKey(_item.UniqueID))
                items[(uint)_item.Location].Add(_item.UniqueID, _item);
        }
    }
}