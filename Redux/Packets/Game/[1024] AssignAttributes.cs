using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Packets.Game
{
    public unsafe struct AssignAttributesPacket
    {
        public byte Strength, Agility, Vitality, Spirit;

        public static implicit operator AssignAttributesPacket(byte* ptr)
        {
            AssignAttributesPacket packet = new AssignAttributesPacket();
            packet.Strength = *((byte*)(ptr + 4));
            packet.Agility = *((byte*)(ptr + 5));
            packet.Vitality = *((byte*)(ptr + 6));
            packet.Spirit = *((byte*)(ptr + 7));
            return packet;
        }
    }
}
