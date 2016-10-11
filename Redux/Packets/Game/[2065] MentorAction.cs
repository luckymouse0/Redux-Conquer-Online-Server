using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;
using Redux.Game_Server;
using Redux.Space;

namespace Redux.Packets.Game
{
    public unsafe struct MentorActionPacket
    {
        public MentorAction type;
        public uint UID;
        public uint dwparam;
        public byte dynamic;
        public byte Online;
        //public NetStringPacker Name;
        #region Name
        private fixed sbyte _name[Constants.MAX_NAMESIZE];
        public string Name
        {
            get
            {
                fixed (sbyte* ptr = _name)
                {
                    return new string(ptr).TrimEnd('\0');
                }
            }
            set
            {
                fixed (sbyte* ptr = _name)
                {
                    MSVCRT.memset(ptr, 0, Constants.MAX_NAMESIZE);
                    value.CopyTo(ptr);
                }
            }
        }
        #endregion

        public static MentorActionPacket Create(Player user)
        {
            MentorActionPacket packet = new MentorActionPacket();
            packet.UID = user.UID;
            packet.type = MentorAction.AcceptRequestMentor;
            packet.Name = user.Name;
            return packet;
        }

        public static implicit operator MentorActionPacket(byte* ptr)
        {
            var packet = new MentorActionPacket();
            packet.type = *((MentorAction*)(ptr + 4));
            packet.UID = *((uint*)(ptr + 8));
            packet.dwparam = *((uint*)(ptr + 12));
            packet.dynamic = (byte)*((uint*)(ptr + 16));//byte
            packet.Online = (byte)*((uint*)(ptr + 20));
            //packet.Name = *(((ptr + 22);
            //Redux.MSVCRT.memcpy(packet._name, ptr + 22, 16);
            return packet;
        }

        public static implicit operator byte[](MentorActionPacket packet)
        {
            byte[] data = new byte[60];
            fixed (byte* ptr = data)
            {
                PacketBuilder.AppendHeader(ptr, data.Length, 2065);
                *((MentorAction*)(ptr + 4)) = (MentorAction)packet.type;
                *((uint*)(ptr + 8)) = packet.UID;
                *((uint*)(ptr + 12)) = packet.dwparam;
                *((byte*)(ptr + 16)) = packet.dynamic;
                *((byte*)(ptr + 20)) = 1;
                *((byte*)(ptr + 21)) = (byte)packet.Name.Length;
                //PacketBuilder.AppendNetStringPacker(ptr + 22, packet.Name);
                MSVCRT.memcpy(ptr + 22, packet._name, 16);
            }
            return data;
        }
    }
}
