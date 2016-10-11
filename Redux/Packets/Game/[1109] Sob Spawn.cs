using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Game_Server;
using Redux.Managers;

namespace Redux.Packets.Game
{
    public unsafe struct SpawnSob
    {
        public uint UID;
        public uint MaxHealth;
        public uint Health;
        public ushort X;
        public ushort Y;
        public ushort Mesh;
        public ushort Flag;
        public ushort Type;
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

        public static SpawnSob Create(SOB who)
        {
            SpawnSob obj = new SpawnSob();
            obj.UID = who.UID;
            obj.MaxHealth = who.MaximumLife;
            obj.Health = (uint)who.Life; ;
            obj.X = who.X;
            obj.Y = who.Y;
            obj.Mesh = who.Mesh;
            obj.Flag = who.Flag;
            obj.Type = 17;
            obj.Name = who.Name;
            //obj.Name = new NetStringPacker();
            //obj.Name.AddString(who.Name.ToString());
            return obj;
        }
        public static implicit operator byte[](SpawnSob packet)
        {
            //string name;
            int len = 32 + 8;
            //if (packet.Name.Count > 0)
                //if(packet.Name.GetString(0, out name))
                    if(packet.Name != "NONE")
                        len += packet.Name.Length;
            var buffer = new byte[len];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, 1109);
                *((uint*)(ptr + 4)) = packet.UID;
                *((uint*)(ptr + 8)) = packet.MaxHealth; 
                *((uint*)(ptr + 12)) = packet.Health;
                *((ushort*)(ptr + 16)) = packet.X;
                *((ushort*)(ptr + 18)) = packet.Y;
                *((ushort*)(ptr + 20)) = packet.Mesh;
                *((ushort*)(ptr + 22)) = packet.Flag;
                //if (packet.Name.Count > 0)
                    //if (packet.Name.GetString(0, out name))
                        if (packet.Name != "NONE")
                        {
                            *((byte*)(ptr + 26)) = 1;
                            *((byte*)(ptr + 27)) = (byte)packet.Name.Length;
                            MSVCRT.memcpy(ptr + 28, packet._name, 16);
                            //PacketBuilder.AppendNetStringPacker(ptr + 28, packet.Name);
                            //Packets.WriteString(packet.Name, 32, buffer);
                            
                        }
            }
            return buffer;
        } 
    }
}
