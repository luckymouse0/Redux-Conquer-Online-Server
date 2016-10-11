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
    public unsafe struct BroadcastPacket
    {
        public uint Subtype;
        public uint Unknown;
        public NetStringPacker StringPacker;
        
        public static implicit operator BroadcastPacket(byte* ptr)
        {
            var packet = new BroadcastPacket();
            packet.Subtype = *((uint*)(ptr + 4));
            packet.Unknown = *((uint*)(ptr + 8)); ;
            packet.StringPacker = new NetStringPacker(ptr + 12);
            return packet;
        }

        public static implicit operator byte[](BroadcastPacket packet)
        {
            var buffer = new byte[28 + packet.StringPacker.Length];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, 2050);
                *((uint*)(ptr + 4)) = packet.Subtype;
                *((long*)(ptr + 8)) = packet.Unknown;
                PacketBuilder.AppendNetStringPacker(ptr + 12, packet.StringPacker);

            }
            return buffer;
        }
    }
}
