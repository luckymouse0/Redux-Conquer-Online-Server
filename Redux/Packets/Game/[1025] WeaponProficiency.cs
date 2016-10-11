using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Structures;
using Redux.Database.Domain;

namespace Redux.Packets.Game
{
    public unsafe struct WeaponProficiencyPacket
    {
        public ushort ID;
        public ushort Level;
        public uint Experience;
        public static WeaponProficiencyPacket Create(ConquerProficiency prof)
        {
            var packet = new WeaponProficiencyPacket();
            packet.ID = prof.ID;
            packet.Level = prof.Level;
            packet.Experience = prof.Experience;
            return packet;
        }
        public static implicit operator byte[](WeaponProficiencyPacket packet)
        {
            var buffer = new byte[22];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, Constants.MSG_PROFICIENCY);
                *((ushort*)(ptr + 4)) = packet.ID;
                *((ushort*)(ptr + 8)) = packet.Level;
                *((uint*)(ptr + 12)) = packet.Experience;
            }
            return buffer;
        }
    }
}
