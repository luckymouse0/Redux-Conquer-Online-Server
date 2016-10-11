using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;

namespace Redux.Packets.Game
{
    public struct UpdateData
    {
        public UpdateType Type;
        public ulong Data;

        #region Data
        public uint DataLow
        {
            get { return (uint)Data; }
            set { Data = (ulong)((DataHigh << 32) | value); }
        }
        public uint DataHigh
        {
            get { return (uint)(Data >> 32); }
            set { Data = (ulong)((value << 32) | DataLow); }
        }
        public long SignedData
        {
            get { return (long)Data; }
            set { Data = (ulong)value; }
        }
        #endregion

        public UpdateData(UpdateType type, ulong data)
        {
            Type = type;
            Data = data;
        }
        public UpdateData(UpdateType _type, uint _dataLow, uint _dataHigh)
        {
            Type = _type;
            Data = 0;
            DataLow = _dataLow;
            DataHigh = _dataHigh;
        }
    }

    public unsafe struct UpdatePacket
    {
        public uint UID;
        public UpdateData Update;
        public static UpdatePacket Create(uint uid, UpdateType type, ulong data)
        {
            return new UpdatePacket(uid, type, data);
        }
        public static UpdatePacket Create(uint uid, UpdateType type, long data)
        {
            return new UpdatePacket(uid, type, (ulong)data);
        }
        public static UpdatePacket Create(uint uid, UpdateType type, uint dataLow, uint dataHigh)
        {
            var value = (ulong)((dataHigh << 32) | dataLow);
            return new UpdatePacket(uid, type, value);
        }
        public UpdatePacket(uint uid)
        {
            UID = uid;
            Update = new UpdateData();
        }
        public UpdatePacket(uint uid, UpdateType type, ulong data)
        {
            UID = uid;
            Update = new UpdateData(type, data);
        }
        public static implicit operator byte[](UpdatePacket packet)
        {
            var buffer = new byte[32];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, Constants.MSG_UPDATE);
                *((uint*)(ptr + 4)) = packet.UID;
                *((uint*)(ptr + 8)) = 1;
                *((UpdateType*)(ptr + 12)) = packet.Update.Type;
                *((ulong*)(ptr + 16)) = packet.Update.Data;
            }
            return buffer;
        }
    }
}