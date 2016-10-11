using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;
using Redux.Game_Server;

namespace Redux.Packets.Game
{
    public unsafe struct NpcPacket
    {
        public uint UID;
        public uint Data;
        public NpcEvent Action;
        public ushort Type;

        public static implicit operator NpcPacket(byte* ptr)
        {
            var packet = new NpcPacket();
            packet.UID = *((uint*)(ptr + 4));
            packet.Data = *((uint*)(ptr + 8));
            packet.Action = *((NpcEvent*)(ptr + 12));
            packet.Type = *((ushort*)(ptr + 14));
            return packet;
        }
        public static implicit operator byte[](NpcPacket packet)
        {
            var buffer = new byte[16 + 8];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, Constants.MSG_NPC_INITIAL);
                *((uint*)(ptr + 4)) = packet.UID;
                *((uint*)(ptr + 8)) = packet.Data;
                *((NpcEvent*)(ptr + 12)) = packet.Action;
                *((ushort*)(ptr + 14)) = packet.Type;
            }
            return buffer;
        }
    }
}
