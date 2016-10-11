using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;
using Redux.Game_Server;

namespace Redux.Packets.Game
{
    public unsafe struct NpcDialogPacket
    {
        public uint UID;
        public ushort ID;
        public byte Linkback;
        public DialogAction Action;
        public NetStringPacker Strings;

        public ushort PositionX
        {
            get { return (ushort)UID; }
            set { UID = (uint)((PositionY << 16) | value); }
        }

        public ushort PositionY
        {
            get { return (ushort)(UID >> 16); }
            set { UID = (uint)((value << 16) | PositionX); }
        }
        
        public static NpcDialogPacket Create ()
        {        
        	var packet = new NpcDialogPacket();
            packet.Strings = new NetStringPacker();
        	return packet;
        }
        
        public static implicit operator NpcDialogPacket(byte* ptr)
        {
            NpcDialogPacket packet = new NpcDialogPacket();
            packet.UID = *((uint*)(ptr + 4));
            packet.ID = *((ushort*)(ptr + 8));
            packet.Linkback = *(ptr + 10);
            packet.Action = *((DialogAction*)(ptr + 11));
            packet.Strings = new NetStringPacker(ptr);
            return packet;
        }

        public static implicit operator byte[](NpcDialogPacket packet)
        {
            var buffer = new byte[24 + packet.Strings.Length];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, Constants.MSG_NPC_DIALOG);
                *((uint*)(ptr + 4)) = packet.UID;
                *((ushort*)(ptr + 8)) = packet.ID;
                *(ptr + 10) = packet.Linkback;
                *((DialogAction*)(ptr + 11)) = packet.Action;
                if(packet.Strings.Count > 0)
                    PacketBuilder.AppendNetStringPacker(ptr + 12, packet.Strings);
            }
            return buffer;
        }
    }
}
