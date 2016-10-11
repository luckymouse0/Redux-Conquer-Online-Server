using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;

namespace Redux.Packets.Game
{
    public unsafe struct GeneralActionPacket
    {
        public static GeneralActionPacket Create(uint _uid, uint _data1, uint _data2, ushort _data3, DataAction _action)
        {
            var packet = new GeneralActionPacket();
            packet.UID = _uid;
            packet.Data1 = _data1;
            packet.Data2 = _data2;
            packet.Data3 = _data3;
            packet.Action = _action;
            return packet;
        }

        public static GeneralActionPacket Create(uint _uid, DataAction _action, ushort _low1, ushort _high1)
        {
            var packet = new GeneralActionPacket();
            packet.UID = _uid;
            packet.Data1Low = _low1;
            packet.Data1High = _high1;
            packet.Action = _action;
            return packet;
        }
        public static GeneralActionPacket Create(uint _uid, DataAction _action, uint _data1)
        {
            return Create(_uid, _data1, 0, 0, _action);
        }
        public uint Timestamp;
        public uint UID;
        public uint Data1;
        public uint Data2;
        public ushort Data3;
        public DataAction Action;

        #region Data1
        /// <summary>
        /// Offset [12]
        /// </summary>
        public ushort Data1Low
        {
            get { return (ushort)Data1; }
            set { Data1 = (uint)((Data1High << 16) | value); }
        }

        /// <summary>
        /// Offset [14]
        /// </summary>
        public ushort Data1High
        {
            get { return (ushort)(Data1 >> 16); }
            set { Data1 = (uint)((value << 16) | Data1Low); }
        }

        #endregion

        #region Data2
        /// <summary>
        /// Offset [16]
        /// </summary>
        public ushort Data2Low
        {
            get { return (ushort)Data2; }
            set { Data2 = (uint)((Data2High << 16) | value); }
        }

        /// <summary>
        /// Offset [18]
        /// </summary>
        public ushort Data2High
        {
            get { return (ushort)(Data2 >> 16); }
            set { Data2 = (uint)((value << 16) | Data2Low); }
        }

        #endregion



        public static implicit operator GeneralActionPacket(byte* ptr)
        {
            var packet = new GeneralActionPacket();
            packet.Timestamp = *((uint*)(ptr + 4));
            packet.UID = *((uint*)(ptr + 8));
            packet.Data1 = *((uint*)(ptr + 12));
            packet.Data2 = *((uint*)(ptr + 16));
            packet.Data3 = *((ushort*)(ptr + 20));
            packet.Action = *((DataAction*)(ptr + 22));
            return packet;
        }

        public static implicit operator byte[](GeneralActionPacket packet)
        {
            var buffer = new byte[28 + 8];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, 1010);
                *((uint*)(ptr + 4)) = packet.Timestamp;
                *((uint*)(ptr + 8)) = packet.UID;
                *((uint*)(ptr + 12)) = packet.Data1;
                *((uint*)(ptr + 16)) = packet.Data2;
                *((ushort*)(ptr + 20)) = packet.Data3;
                *((DataAction*)(ptr + 22)) = packet.Action;
            }
            return buffer;
        }

    }
}
