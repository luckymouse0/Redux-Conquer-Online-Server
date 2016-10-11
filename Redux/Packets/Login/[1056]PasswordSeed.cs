using System;

namespace Redux.Packets.Login
{
    public struct PasswordSeedPacket
    {
        public ushort Size;
        public ushort Type;
        public uint Seed;

        public PasswordSeedPacket(int seed)
        {
            Size = 8;
            Type = 1059;
            Seed = (uint)seed;
        }
        public static unsafe implicit operator byte[](PasswordSeedPacket packet)
        {
            var buffer = new byte[packet.Size];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length+8, 1056);
                *((uint*)(ptr + 4)) = packet.Seed;
            }
            return buffer;
        }
    }
}