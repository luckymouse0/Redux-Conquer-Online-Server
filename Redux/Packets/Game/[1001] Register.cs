using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Game_Server;

namespace Redux.Packets.Game
{
    public unsafe struct RegisterPacket
    {
        private fixed sbyte accountName[Constants.MAX_NAMESIZE];
        private fixed sbyte characterName[Constants.MAX_NAMESIZE]; 
        private fixed sbyte accountPassword[Constants.MAX_NAMESIZE];

        public string AccountName
        {
            get { fixed (sbyte* ptr = accountName) return new string(ptr, 0, Constants.MAX_NAMESIZE, Encoding.Default).TrimEnd('\0'); }
            set
            {
                fixed (sbyte* ptr = accountName)
                {
                    MSVCRT.memset(ptr, 0, Constants.MAX_NAMESIZE);
                    value.CopyTo(ptr);
                }
            }
        }
        public string CharacterName
        {
            get { fixed (sbyte* ptr = characterName) return new string(ptr, 0, Constants.MAX_NAMESIZE, Encoding.Default).TrimEnd('\0'); }
            set
            {
                fixed (sbyte* ptr = characterName)
                {
                    MSVCRT.memset(ptr, 0, Constants.MAX_NAMESIZE);
                    value.CopyTo(ptr);
                }
            }
        }
        public string AccountPassword
        {
            get { fixed (sbyte* ptr = accountPassword) return new string(ptr, 0, Constants.MAX_NAMESIZE, Encoding.Default).TrimEnd('\0'); }
            set
            {
                fixed (sbyte* ptr = accountPassword)
                {
                    MSVCRT.memset(ptr, 0, Constants.MAX_NAMESIZE);
                    value.CopyTo(ptr);
                }
            }
        }

        public ushort Mesh;
        public byte Profession;
        public uint UID;
        public unsafe static implicit operator RegisterPacket(byte* ptr)
        {
            var packet = new RegisterPacket();
            MSVCRT.memcpy(packet.accountName, ptr + 4, 16);
            MSVCRT.memcpy(packet.characterName, ptr + 20, 16);
            MSVCRT.memcpy(packet.accountPassword, ptr + 36, 16);
            packet.Mesh = *((ushort*)(ptr + 52));
            packet.Profession = *((byte*)(ptr + 54));
            packet.UID = *((uint*)(ptr + 58));
            return packet;
        }

    }
}
