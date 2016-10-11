using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Space;
using Redux.Enum;
using Redux.Packets.Game;
using Redux.Structures;

namespace Redux.Game_Server
{
    public class GroundItem : ILocatableObject
    {
        //To generate a new ground item we need a conquer item and location
        public GroundItem(ConquerItem _item, uint _uid, Point _location, Map _map, uint _killer = 0, CurrencyType _currency = CurrencyType.None, uint _value = 0)
        {
            Item = _item;
            StaticID = _item.GroundID;
            UID = _uid;
            Location = _location;
            Map = _map;
            KillerUID = _killer;
            DroppedAt = Common.Clock;
            Currency = _currency;
            Value = _value;
            SpawnPacket = GroundItemPacket.Create(this, GroundItemAction.Create);
        }

        public ConquerItem Item { get; set; }
        public uint StaticID { get; private set; }

        public uint UID { get; set; }
        public Point Location { get; set; }
        public Map Map { get; set; }

        public uint KillerUID { get; set; }
        public long DroppedAt { get; private set; }

        public CurrencyType Currency { get; set; }
        public uint Value { get; set; }

        public GroundItemPacket SpawnPacket { get; set; }


        public void AddToMap()
        {
            Map.Insert(this);
            foreach (var entity in Map.QueryScreen<Entity>(this))
            {
                if (!entity.VisibleObjects.ContainsKey(UID))
                    entity.VisibleObjects.TryAdd(UID, UID);
                entity.Send(SpawnPacket);
            }
            Common.MapService.AddFlag((ushort)Map.ID, (ushort)Location.X, (ushort)Location.Y, TinyMap.TileFlag.Item);
        }

        public void RemoveFromMap()
        {
            uint x;
            foreach (var entity in Map.QueryScreen<Entity>(this))
            {
                if (entity.VisibleObjects.ContainsKey(UID))
                    entity.VisibleObjects.TryRemove(UID, out x);
                entity.Send(GroundItemPacket.Create(this, GroundItemAction.Delete));
            }
            Common.MapService.RemoveFlag((ushort)Map.ID, (ushort)Location.X, (ushort)Location.Y, TinyMap.TileFlag.Item);
            Map.Remove(this);
        }
    }
}
