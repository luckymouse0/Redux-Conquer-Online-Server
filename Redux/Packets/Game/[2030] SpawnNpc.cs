using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;
using Redux.Game_Server;
using Redux.Space;

namespace Redux.Packets.Game
{
    public unsafe struct SpawnNpcPacket
    {
        public uint UID;
        public ushort X;
        public ushort Y;
        public ushort Mesh;
        public NpcType Type;
        public uint Unknown1;

        public NetStringPacker Name;

        public static SpawnNpcPacket Create(uint _id,ushort _mesh, Point _location, NpcType _type = NpcType.Task)
        {
            var packet = new SpawnNpcPacket();
            packet.UID = _id;
            packet.X = (ushort)_location.X;
            packet.Y = (ushort)_location.Y;
            packet.Mesh = _mesh;
            packet.Type = _type;
            return packet;
        }
        public static SpawnNpcPacket Create(Npc _npc)
        {
            var packet = new SpawnNpcPacket();
            packet.UID = _npc.UID;
            packet.X = _npc.X;
            packet.Y = _npc.Y;
            packet.Mesh = _npc.Mesh;
            packet.Type = _npc.Type;
            if (_npc.Name != null && _npc.Name.Length > 0)
            {
                packet.Name = new NetStringPacker();
                packet.Name.AddString(_npc.Name);
            }

            return packet;
        }
        public static implicit operator byte[] (SpawnNpcPacket _data)
        {
            byte[] packet = new byte[48];
            fixed (byte* ptr = packet)
            {
                PacketBuilder.AppendHeader(ptr, packet.Length, Constants.MSG_NPC_SPAWN);
                *((uint*)(ptr + 4)) = _data.UID;
                *((ushort*)(ptr + 8)) = _data.X;
                *((ushort*)(ptr + 10)) = _data.Y;
                *((ushort*)(ptr + 12)) = _data.Mesh;
                *((NpcType*)(ptr + 14)) = _data.Type;
                *((uint*)(ptr + 16)) = _data.Unknown1;
                //*((uint*)(ptr + 18)) = (uint)(_data.NPCName.Length > 0 ? 1 : 0);
                if (_data.Name != null)
                    PacketBuilder.AppendNetStringPacker(ptr + 18, _data.Name);

                /*if (_data.NPCName != null && _data.NPCName.Length > 0)
                {
                    *((uint*)(ptr + 19)) = (uint)_data.NPCName.Length;
                    Redux.MSVCRT.memcpy(ptr + 20, _data._npcname, 16);
                }*/
            }
            return packet;
        }
    }
}
