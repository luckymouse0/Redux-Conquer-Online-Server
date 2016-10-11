using System;
using Redux.Database.Domain;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Redux.Enum;
using Redux.Managers;
using Redux.Game_Server;
using Redux.Packets;
using Redux.Structures;

namespace Redux.Game_Server
{
    public struct SaleItem
    {
        public Structures.ConquerItem Item;
        public uint Price;
        public bool CpCost;
        public SaleItem(Structures.ConquerItem _item, uint _cost, bool _cp = false)
        {
            Item = _item;
            Price = _cost;
            CpCost = _cp;
        }
    }
    public class PlayerShop
    {
        public Player Owner;
        public Npc Carpet;
        public bool Vending { get { if (Carpet == null)return false; return Carpet.Vending; } }
        
        public ConcurrentDictionary<uint, SaleItem> Items = new ConcurrentDictionary<uint, SaleItem>();
        public uint ShopID { get { if (Carpet == null)return 0; return Carpet.UID; } }
        public Packets.Game.TalkPacket HawkMsg = new Packets.Game.TalkPacket("");

        public PlayerShop(Player user)
        {
            Owner = user;
        }
        public bool StartVending()
        {
            var locals = Owner.Map.QueryScreen(Owner);
            foreach (var o in locals)
                if (o is Npc && o.Location.X == Owner.X - 2 && o.Location.Y == Owner.Y)
                {
                    var role = o as Npc;
                    if (role != null && role.Mesh == 0x43E)
                    {
                        Carpet = role;
                        break;
                    }
                }
            if (Carpet == null)
                return false;
            Carpet.StartVending(Owner.Name);
            Carpet.Map.Remove(Carpet, false);
            Owner.SpawnPacket.Action = ActionType.Sit;
            Owner.SpawnPacket.Direction = 6;
            Carpet.SpawnPacket.Type = NpcType.BoothNpc;
            Carpet.SpawnPacket.Mesh = 406;
            Carpet.SpawnPacket.X = Carpet.X;
            Carpet.SpawnPacket.Y = Carpet.Y;
            Carpet.Map.Insert(Carpet);
            Owner.SendToScreen(Carpet.SpawnPacket, true);
            return Vending;
        }
        public void StopVending()
        {
            HawkMsg.Words = "";
            if (Carpet != null)
            {
                Carpet.StopVending();
                Carpet.Map.Remove(Carpet, false);
                Carpet.SpawnPacket.Mesh = 1086;
                Carpet.SpawnPacket.Type = NpcType.RoleBoothFlag;
                Carpet.SpawnPacket.X = Carpet.X;
                Carpet.SpawnPacket.Y = Carpet.Y;
                Carpet.SpawnPacket.Name.Clear();
                Carpet.Map.Insert(Carpet);

                Owner.SpawnPacket.Action = ActionType.None;
                Owner.SendToScreen(Carpet.SpawnPacket, true);
                Carpet = null;
            }
            Items.Clear();
        }
    }
}
