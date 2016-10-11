using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;
using Redux.Game_Server;

namespace Redux.Packets.Game
{
    public unsafe struct SpawnEntityPacket
    {
        public uint UID;
        public uint Lookface;
        public ClientEffect StatusEffects;
        public ushort GuildID;
        public byte Unknown1;
        public byte GuildRank;
        public uint GarmentType;
        public uint HelmetType;
        public uint ArmorType;
        public uint WeaponLType;
        public uint WeaponRType;
        public uint Unknown2;
        public uint Life;
        public ushort Level;
        public ushort PositionX;
        public ushort PositionY;
        public ushort Hair;
        public byte Direction;
        public ActionType Action;
        public byte RebornCount;
        public byte Unknown4;
        public byte Unknown5;
        public byte Nobility;
        public uint NobilityRank;
        public ushort Unknown6;
        public byte HelmetColor;
        public byte ArmorColor;
        public NetStringPacker Names;

        public static SpawnEntityPacket Create(Player user)
        {
            SpawnEntityPacket packet = new SpawnEntityPacket();
            packet.Names = new NetStringPacker();
            packet.UID = user.UID;
            packet.Lookface = user.Character.Lookface;

            Structures.ConquerItem item;
            if (user.TryGetEquipmentByLocation(ItemLocation.Helmet, out item))
            {
                packet.HelmetType = item.StaticID;
                packet.HelmetColor = item.Color;
            }
            if (user.TryGetEquipmentByLocation(ItemLocation.Armor, out item))
            {
                packet.ArmorType = item.StaticID;
                packet.ArmorColor = item.Color;
            }
            if (user.TryGetEquipmentByLocation(ItemLocation.WeaponR, out item))
                packet.WeaponRType = item.StaticID;
            if (user.TryGetEquipmentByLocation(ItemLocation.WeaponL, out item))
                packet.WeaponLType = item.StaticID;
            if (user.TryGetEquipmentByLocation(ItemLocation.Garment, out item))
                packet.GarmentType = item.StaticID;

            packet.PositionX = user.X;
            packet.PositionY = user.Y;
            packet.Hair = user.Hair;
            packet.Direction = user.Direction;
            packet.Action = user.Action;
            packet.RebornCount = user.RebornCount;
            packet.Level = user.Level;
            packet.Names.AddString(user.Name);
            packet.GuildID = (ushort)user.GuildId;
            packet.GuildRank = (byte)user.GuildRank;
            packet.Nobility = (byte)user.NobilityMedal;
            return packet;
        }

        public static SpawnEntityPacket Create(Monster user)
        {
            SpawnEntityPacket packet = new SpawnEntityPacket();
            packet.Names = new NetStringPacker();
            packet.UID = user.UID;
            packet.Lookface = user.BaseMonster.Mesh;   
            packet.PositionX = user.X;
            packet.PositionY = user.Y;
            packet.Direction = user.Direction;
            packet.Life = user.Life;
            packet.Level = user.BaseMonster.Level;
            packet.Names.AddString(user.BaseMonster.Name);
            return packet;
        }

        public static implicit operator byte[](SpawnEntityPacket packet)
        {
            byte[] data = new byte[100 + packet.Names.Length];//100
            fixed (byte* ptr = data)
            {
                PacketBuilder.AppendHeader(ptr, data.Length, 1014);
                *((uint*)(ptr + 4)) = packet.UID;
                *((uint*)(ptr + 8)) = packet.Lookface;
                *((ClientEffect*)(ptr + 12)) = packet.StatusEffects;
                *((uint*)(ptr + 20)) = packet.GuildID;
                *((byte*)(ptr + 23)) = packet.GuildRank;
                *((uint*) (ptr + 24)) = packet.GarmentType;
                *((uint*)(ptr + 28)) = packet.HelmetType;
                *((uint*)(ptr + 32)) = packet.ArmorType;
                *((uint*)(ptr + 36)) = packet.WeaponRType;
                *((uint*)(ptr + 40)) = packet.WeaponLType;

                //48 uint realted to hp?

                *((ushort*)(ptr + 48)) = (ushort)packet.Life;
                *((ushort*)(ptr + 50)) = packet.Level;
                
                *((ushort*)(ptr + 52)) = packet.PositionX;
                *((ushort*)(ptr + 54)) = packet.PositionY;
                *((ushort*)(ptr + 56)) = packet.Hair;
                *(ptr + 58) = packet.Direction;
                *(ptr + 59) = (byte)packet.Action;
                *(ptr + 60) = packet.RebornCount;
                *((ushort*)(ptr + 62)) = packet.Level;
                *((byte*)(ptr + 84)) = packet.HelmetColor;
                *((byte*)(ptr + 86)) = packet.ArmorColor;
                *((byte*)(ptr + 68)) = packet.Nobility;
                *((uint*)(ptr + 76)) = packet.NobilityRank;
                PacketBuilder.AppendNetStringPacker(ptr + 90, packet.Names);
            }
            return data;
        }
    }
}
