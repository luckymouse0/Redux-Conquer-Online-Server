using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Game_Server;

namespace Redux.Packets.Game
{    
    public unsafe struct HeroInformationPacket
    {
        public uint Id;
        public uint Lookface;
        public ushort Hair;
        public uint Money;
        public uint CP;
        public ulong Experience;
        public ushort Strength;
        public ushort Agility;
        public ushort Vitality;
        public ushort Spirit;
        public ushort Stats;
        public ushort Life;
        public ushort Mana;
        public short PKPoints;
        public byte Level;
        public byte Class;
        public byte Reborn;
        public byte ShowName;
        public NetStringPacker Strings;

        public static HeroInformationPacket Create(Player user)
        {
            var packet = new HeroInformationPacket();
            packet.Id = user.Character.UID;
            packet.Lookface = user.Character.Lookface;
            packet.Hair = user.Character.Hair;
            packet.Money = user.Character.Money;
            packet.CP = user.Character.CP;
            packet.Experience = user.Character.Experience;
            packet.Strength = user.Character.Strength;
            packet.Agility = user.Character.Agility;
            packet.Vitality = user.Character.Vitality;
            packet.Spirit = user.Character.Spirit;
            packet.Stats = user.Character.ExtraStats;
            packet.Life = user.Character.Life;
            packet.Mana = user.Character.Mana;
            packet.PKPoints = user.Character.Pk;
            packet.Level = user.Character.Level;
            packet.Class = user.Character.Profession;
            packet.ShowName = 1;
            packet.Reborn = user.RebornCount;
            packet.Strings = new NetStringPacker();
            packet.Strings.AddString(user.Character.Name);
            packet.Strings.AddString(user.Character.Spouse);
            return packet;
        }
        public static implicit operator byte[](HeroInformationPacket packet)
        {
            var buffer = new byte[71 + packet.Strings.Length + 8];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, Constants.MSG_HERO_INFORMATION);
                *((uint*)(ptr + 4)) = packet.Id;
                *((uint*)(ptr + 8)) = packet.Lookface;
                *((ushort*)(ptr + 12)) = packet.Hair;
                *((uint*)(ptr + 14)) = packet.Money;
                *((uint*)(ptr + 18)) = packet.CP;
                *((ulong*)(ptr + 22)) = packet.Experience;
                //30-46 unknown                

                //46 ushort a0 40
                //48 ushort dd 00

                *((ushort*)(ptr + 50)) = packet.Strength;
                *((ushort*)(ptr + 52)) = packet.Agility;
                *((ushort*)(ptr + 54)) = packet.Vitality;
                *((ushort*)(ptr + 56)) = packet.Spirit;
                *((ushort*)(ptr + 58)) = packet.Stats;
                *((ushort*)(ptr + 60)) = packet.Life;
                *((ushort*)(ptr + 62)) = packet.Mana;
                *((short*)(ptr + 64)) = packet.PKPoints;
                *((byte*)(ptr + 66)) = packet.Level;
                *((byte*)(ptr + 67)) = packet.Class;
                *((byte*)(ptr + 69)) = packet.Reborn;
                *((byte*)(ptr + 70)) = packet.ShowName;                
                PacketBuilder.AppendNetStringPacker(ptr + 71, packet.Strings);
            }
            return buffer;
        }
     
    }
}
