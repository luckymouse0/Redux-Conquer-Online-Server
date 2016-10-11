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
    public unsafe struct OfflineTGPacket
    {

        public uint Type;
        public long TrainingTime;

        public static implicit operator OfflineTGPacket(byte* ptr)
        {
            var packet = new OfflineTGPacket();
            packet.Type = *((uint*)(ptr + 4));
            packet.TrainingTime = *((long*)(ptr + 8));
            
            return packet;
        }

        public static implicit operator byte[](OfflineTGPacket packet)
        {
            var buffer = new byte[28];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, 2044);
                *((uint*)(ptr + 4)) = packet.Type;
                *((long*)(ptr + 8)) = packet.TrainingTime;

            }
            return buffer;
        }
    }
}
