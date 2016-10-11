using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;

namespace Redux.Packets.Game
{
    public unsafe struct InteractPacket
    {
        public uint Timestamp;
        public uint UID;
        public uint Target;
        public ushort X;
        public ushort Y;
        public InteractAction Action;
        public uint Value;
        public static InteractPacket Create(uint _uid, uint _target, ushort _x, ushort _y, InteractAction _action, uint _value)
        {
            var packet = new InteractPacket();
            packet.UID = _uid;
            packet.Target = _target;
            packet.X = _x;
            packet.Y = _y;
            packet.Action = _action;
            packet.Value = _value;
            packet.Timestamp = (uint)Common.Clock;
            if (packet.Action == InteractAction.MagicAttack) EncodeMagicAttack(&packet);
            return packet;
        }
        public ushort MagicType
        {
            get { return (ushort)Value; }
            set { Value = (uint)((MagicLevel << 16) | value); }
        }
        public ushort MagicLevel
        {
            get { return (ushort)(Value >> 16); }
            set { Value = (uint)((value << 16) | MagicType); }
        }
        public static implicit operator InteractPacket(byte* ptr)
        {
            var packet = new InteractPacket();
            packet.Timestamp = *((uint*)(ptr + 4));
            packet.UID = *((uint*)(ptr + 8));
            packet.Target = *((uint*)(ptr + 12));
            packet.X = *((ushort*)(ptr + 16));
            packet.Y = *((ushort*)(ptr + 18));
            packet.Action = *((InteractAction*)(ptr + 20));
            packet.Value = *((uint*)(ptr + 24));

            if (packet.Action == InteractAction.MagicAttack) DecodeMagicAttack(&packet);
            return packet;
        }

        public static implicit operator byte[](InteractPacket packet)
        {
            var buffer = new byte[40];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, Constants.MSG_INTERACT);
                *((uint*)(ptr + 4)) = packet.Timestamp;
                *((uint*)(ptr + 8)) = packet.UID;
                *((uint*)(ptr + 12)) = packet.Target;
                *((ushort*)(ptr + 16)) = packet.X;
                *((ushort*)(ptr + 18)) = packet.Y;
                *((InteractAction*)(ptr + 20)) = packet.Action;
                *((uint*)(ptr + 24)) = packet.Value;
            }
            return buffer;
        }
        public static void EncodeMagicAttack(InteractPacket* packet)
        {
            packet->MagicType = (ushort)(Common.ExchangeShortBits(packet->MagicType - (uint)0x14be, 3) ^ packet->UID ^ 0x915d);
            packet->Target = Common.ExchangeLongBits(((packet->Target - 0x8b90b51a) ^ packet->UID ^ 0x5f2d2463), 32 - 13);
            packet->X = (ushort)(Common.ExchangeShortBits(packet->X - (uint)0xdd12, 1) ^ packet->UID ^ 0x2ed6);
            packet->Y = (ushort)(Common.ExchangeShortBits((packet->Y - (uint)0x76de), 5) ^ packet->UID ^ 0xb99b);
            packet->MagicLevel = (ushort)((packet->MagicLevel + 0x100 * (packet->Timestamp % 0x100)) ^ 0x3721);
        }

        public static void DecodeMagicAttack(InteractPacket* packet)
        {
            packet->MagicType = (ushort)(0xFFFF & (Common.ExchangeShortBits((packet->MagicType ^ packet->UID ^ 0x915d), 16 - 3) + 0x14be));
            packet->MagicLevel = (ushort)(((byte)packet->MagicLevel) ^ 0x21);
            packet->Target = (Common.ExchangeLongBits(packet->Target, 13) ^ packet->UID ^ 0x5f2d2463) + 0x8b90b51a;
            packet->X = (ushort)(0xFFFF & (Common.ExchangeShortBits((packet->X ^ packet->UID ^ 0x2ed6), 16 - 1) + 0xdd12));
            packet->Y = (ushort)(0xFFFF & (Common.ExchangeShortBits((packet->Y ^ packet->UID ^ 0xb99b), 16 - 5) + 0x76de));
        }

    }
}
