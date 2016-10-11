using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;

namespace Redux.Packets.Game
{
    public unsafe struct SkillEffectPacket
    {
        public uint UID;
        public uint Data;
        public ushort Type;
        public ushort Level;
        private List<uint> TargetIDs;
        private List<uint> TargetDMGs;

        public int TargetCount
        {
            get { return TargetIDs.Count; }
        }
        public Dictionary<uint, uint> GetTargets()
        {
            var targets = new Dictionary<uint, uint>();
            for (var i = 0; i < TargetCount; i++)            
                targets.Add(TargetIDs[i], TargetDMGs[i]);
            return targets;
            
        }
        public ushort DataLow
        {
            get { return (ushort)Data; }
            set { Data = (uint)((DataHigh << 16) | value); }
        }

        public ushort DataHigh
        {
            get { return (ushort)(Data >> 16); }
            set { Data = (uint)((value << 16) | DataLow); }
        }
        public SkillEffectPacket(uint _uid)
        {
            UID = Data = _uid;
            Type = Level = 0;
            TargetIDs = new List<uint>();
            TargetDMGs = new List<uint>();
        }

        public static SkillEffectPacket Create(uint _uid, uint _data, ushort _type, ushort _level)
        {
            return new SkillEffectPacket()
            {
                UID = _uid,
                Data = _data,
                Type = _type,
                Level = _level,
                TargetIDs = new List<uint>(),
                TargetDMGs = new List<uint>(),
            };
        }
        public static SkillEffectPacket Create(uint _uid, int _x, int _y, ushort _type, ushort _level)
        {
            return new SkillEffectPacket()
            {
                UID = _uid,
                DataLow = (ushort)_x,
                DataHigh = (ushort)_y,
                Type = _type,
                Level = _level,
                TargetIDs = new List<uint>(),
                TargetDMGs = new List<uint>(),
            };
        }

        public void ClearTargets()
        {
            TargetIDs = new List<uint>();
            TargetDMGs = new List<uint>();
        }
        public void AddTarget(uint _id, uint _data)
        {
            if (TargetIDs.Contains(_id))
                return;
            TargetIDs.Add(_id);
            TargetDMGs.Add(_data);
        }

        public bool TargetExists(uint _id)
        {
            return TargetIDs.Contains(_id);
        }

        public int GetTargetCount()
        {
            return TargetIDs.Count;
        }

        public static implicit operator byte[](SkillEffectPacket packet)
        {
            const int TARGET_SIZE = 12;
            var buffer = new byte[36 + packet.TargetIDs.Count * TARGET_SIZE];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, Constants.MSG_SKILL_EFFECT);
                *((uint*)(ptr + 4)) = packet.UID;
                *((uint*)(ptr + 8)) = packet.Data;
                *((ushort*)(ptr + 12)) = packet.Type;
                *((ushort*)(ptr + 14)) = packet.Level;
                *((int*)(ptr + 16)) = packet.TargetIDs.Count;
                for (byte i = 0; i < packet.TargetIDs.Count; i++)
                {
                    *((uint*)(ptr + 20 + i * TARGET_SIZE)) = packet.TargetIDs[i];
                    *((uint*)(ptr + 24 + i * TARGET_SIZE)) = packet.TargetDMGs[i];
                }
            }
            return buffer;
        }
        

    }
}
