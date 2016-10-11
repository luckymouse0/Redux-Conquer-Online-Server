/*
 * User: pro4never
 * Date: 9/24/2013
 * Time: 9:29 PM
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;
using Redux.Game_Server;

namespace Redux.Packets.Game
{
    public unsafe struct ComposePacket
    {

        public byte Action;
        public byte Count;
        public List<uint> Values;

        public static implicit operator ComposePacket(byte* ptr)
        {
            ComposePacket packet = new ComposePacket();
            packet.Values = new List<uint>();
            packet.Action = *((byte*)(ptr + 4));
            packet.Count = *((byte*)(ptr + 5));
            for (var count = 0; count < packet.Count; count++)
                packet.Values.Add(*((uint*)(ptr + 8 + count * 4)));
            return packet;
        }
    }
}
