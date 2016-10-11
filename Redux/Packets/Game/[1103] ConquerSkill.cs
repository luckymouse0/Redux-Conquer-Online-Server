using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Database.Domain;
using Redux.Structures;

namespace Redux.Packets.Game
{
    public unsafe struct ConquerSkillPacket
    {
        public uint Experience;
        public ushort ID;
        public ushort Level;
        public static ConquerSkillPacket Create(ConquerSkill skill)
        {
            var packet = new ConquerSkillPacket();
            packet.ID = skill.ID;
            packet.Level = skill.Level;
            packet.Experience = skill.Experience;
            return packet;
        }
        public static implicit operator byte[](ConquerSkillPacket packet)
        {
            var buffer = new byte[22];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, Constants.MSG_CONQUER_SKILL);
                *((uint*)(ptr + 4)) = packet.Experience;
                *((ushort*)(ptr + 8)) = packet.ID;
                *((ushort*)(ptr + 10)) = packet.Level;
            }
            return buffer;
        }
    }
}
