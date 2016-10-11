using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Packets.Game
{
    public unsafe struct WalkPacket
    {
        public uint UID;
        public byte Direction;
        public byte Mode;
        public ushort Unknown1;
        public static WalkPacket Create(uint _uid, byte _dir, byte _mode = 1)
        {
            var packet = new WalkPacket();
            packet.UID = _uid;
            packet.Direction = _dir;
            packet.Mode = _mode;
            return packet;
        }
        public static implicit operator WalkPacket(byte* ptr)
        {
            WalkPacket packet = new WalkPacket();
            packet.UID = *((uint*)(ptr + 4));
            packet.Direction = *(ptr + 8);
            packet.Mode = *(ptr + 9);
            packet.Unknown1 = *((ushort*)(ptr + 10));
            return packet;
        }

        public static implicit operator byte[](WalkPacket packet)
        {
            var buffer = new byte[20];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, Constants.MSG_WALK);
                *((uint*)(ptr + 4)) = packet.UID;
                *(ptr + 8) = packet.Direction;
                *(ptr + 9) = packet.Mode;
                *((ushort*)(ptr + 10)) = packet.Unknown1;
            }
            return buffer;
        }
    }
}
