using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Space;
using Redux.Database;
using Redux.Database.Domain;
using Redux.Managers;
using Redux.Enum;
using Redux.Packets.Game;

namespace Redux.Game_Server
{
    public class Npc : ILocatableObject
    {
        public Npc(DbNpc _npc, Map _map)
        {
            BaseNpc = _npc;
            UID = _npc.UID;
            Location = new Point(_npc.X, _npc.Y);
            SpawnPacket = SpawnNpcPacket.Create(this);
            Map = _map;
        }
        public SpawnNpcPacket SpawnPacket;
        public Point Location { get; set; }
        public Map Map { get; set; }
        public uint UID { get; set; }
        public DbNpc BaseNpc { get; set; }
        public NpcType Type { get { return BaseNpc.Type; } }
        public ushort X { get { return (ushort)Location.X; } }
        public ushort Y { get { return (ushort)Location.Y; } }
        private ushort _mesh;
        public ushort Mesh { get { return BaseNpc.Mesh; } set { _mesh = value; } }

        public bool Vending = false;
        public byte Flag;
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                //SpawnPacket.NPCName = _name;
                SpawnPacket.Name = new Packets.NetStringPacker();
                SpawnPacket.Name.AddString(_name);
            }
        }

        public void StartVending(string OwnerName)
        {
            Name = OwnerName;
            Vending = true;
            BaseNpc.Mesh = 0x196;
            BaseNpc.Type = NpcType.BoothNpc;
            Location= new Point(Location.X + 3, Location.Y);
        }
        public void StopVending()
        {
            Name = "";
            Vending = false;
            Mesh = 0x43E;
            Flag = 0x10;
            Location = new Point(Location.X - 3, Location.Y);
        }
    }
}
