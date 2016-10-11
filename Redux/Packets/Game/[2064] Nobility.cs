using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;
using Redux.Game_Server;

namespace Redux.Packets.Game
{

    public unsafe struct Nobility
    {
        public const uint
            Donate = 1,
            List = 2,
            Icon = 3,
            NextRank = 4;
        public NobilityAction Type;
        public long Donation;

        private fixed sbyte _string3[35];
        public string String3
        {
            get
            {
                fixed (sbyte* ptr = _string3)
                {
                    return new string(ptr).TrimEnd('\0');
                }
            }
            set
            {
                fixed (sbyte* ptr = _string3)
                {
                    MSVCRT.memset(ptr, 0, 35);
                    value.CopyTo(ptr);
                }
            }
        }

        public long Data1
        {
            get { return (uint)Donation; }
            set { Donation = (Data2 << 32) | value; }
        }

        public uint Data2
        {
            get { return (uint)(Donation >> 32); }
            set { Donation = (value << 32) | Data1; }
        }
        public ushort Data2Low
        {
            get { return (ushort)Data2; }
            set { Data2 = (uint)((Data2High << 16) | value); }
        }
        public ushort Data2High
        {
            get { return (ushort)(Data2 >> 16); }
            set { Data2 = (uint)((value << 16) | Data2Low); }
        }
        public uint Data3;
        public uint Data4;
        public uint Data5;
        public uint Data6;
        //public string [] Strings;
        public NetStringPacker Strings;
        public static Nobility UpdateIcon(Player user)
        {
            Nobility packet = new Nobility();
            packet.Type = NobilityAction.Info;
            packet.Data1 = user.UID;
            packet.String3 = user.UID + " " + user.Donation + " " + (byte)user.NobilityMedal + " " + user.NobilityRank;
            //EXAMPLE STRING = user.UID + " " + 100000000 + " " + 12 + " " + 1;   ID DONATION MEDAL RANK
            return packet;
        }
        public static implicit operator Nobility(byte* ptr)
        {
            var packet = new Nobility();
            packet.Type = *((NobilityAction*)(ptr + 4));
            packet.Data1 = *((uint*)(ptr + 8));
            packet.Data2 = *((uint*)(ptr + 12));
            packet.Data3 = *((uint*)(ptr + 16));
            packet.Data4 = *((uint*)(ptr + 20));
            packet.Data5 = *((uint*)(ptr + 24));
            packet.Data6 = *((uint*)(ptr + 30));
            //packet.Strings = new NetStringPacker(ptr + 32);
            return packet;
        }


        public static implicit operator byte[](Nobility packet)
        {
            var buffer = new byte[40 + 8 + Size(packet)];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, 2064);
                *((NobilityAction*)(ptr + 4)) = packet.Type;
                *((long*)(ptr + 8)) = packet.Data1;

                switch (packet.Type)
                {
                    case NobilityAction.List:
                        {


                            *((Int16*)(ptr + 8)) = (Int16)packet.Data1; ; //Page n° (Param)
                            *((Int16*)(ptr + 10)) = (Int16)Managers.NobilityManager.PageCount; //Page Count (Param)
                            *((Int32*)(ptr + 12)) = (Int32)0x00; //(Param)2
                            if (packet.Strings != null)
                                PacketBuilder.AppendNetStringPacker(ptr + 16, packet.Strings);

                            break;
                        }
                    case NobilityAction.Info:
                        {
                            *((uint*)(ptr + 16)) = 1;//packet.Unknown16;
                            *((uint*)(ptr + 17)) = (uint)packet.String3.Length;
                            MSVCRT.memcpy(ptr + 18, packet._string3, packet.String3.Length);
                            break;
                        }
                }
            }
            return buffer;
        }

        public static uint Size(Nobility packet)
        {
            switch (packet.Type)
            {
                case NobilityAction.Info:
                    return (uint)(40 + packet.String3.Length);
                case NobilityAction.Donate:
                    return 40;
                case NobilityAction.QueryRemainingSilver:
                    return 40;
                case NobilityAction.List:
                    var page = Managers.NobilityManager.GetPage((int)packet.Data1);
                    if (page != null)
                        return (uint)(page.Length);
                    else
                        return 60;
                default:
                    return 40;
            }
        }
    }
}
