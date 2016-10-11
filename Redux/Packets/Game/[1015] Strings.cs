using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Database.Domain;
using Redux.Structures;
using Redux.Enum;
using Redux.Game_Server;
namespace Redux.Packets.Game
{
    public unsafe struct StringsPacket
    {
        public uint UID;
        public StringAction Type;
        public NetStringPacker Strings;

        public static StringsPacket Create(uint _uid, StringAction _type, string _value)
        {
            var packet = new StringsPacket();
            packet.UID = _uid;
            packet.Type =_type;
            packet.Strings = new NetStringPacker();
            packet.Strings.AddString(_value);
            return packet;
        }

        public static implicit operator StringsPacket(byte* ptr)
        {
            var packet = new StringsPacket();
            packet.UID = *((uint*)(ptr + 4));
            packet.Type = *((StringAction*)(ptr + 8));
            packet.Strings = new NetStringPacker(ptr + 9);    
            return packet;
        }

        public static implicit operator byte[](StringsPacket packet)
        {
            var buffer = new byte[11 + packet.Strings.Length + 8];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, Constants.MSG_STRINGS);
                *((uint*)(ptr + 4)) = packet.UID;
                *((StringAction*)(ptr + 8)) = packet.Type;
                PacketBuilder.AppendNetStringPacker(ptr + 9, packet.Strings);
            }
            return buffer;
        }
    }
}
