using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;
using Redux.Game_Server;
using Redux.Space;

namespace Redux.Packets.Game
{

    public unsafe struct MentorOfflinePacket
    {
        public string[] Strings;
        public uint MentorType;
        public uint Type2;
        public uint MentorID;
        public uint ApprenticeID;
        public uint LookFace; //MentorMesh
        public uint SharedBP;
        public uint Unknown;
        public uint Enroll;
        public uint MentorLevel;
        public byte MentorClass;
        public short MentorPKP;
        public byte MentorOnline;
        public uint ApprenticeExp;
        public ushort ApprenticeBless;
        public ushort ApprenticeComposing;
        public byte StringCount;
        #region MentorName
        private fixed sbyte _mentorname[Constants.MAX_NAMESIZE];
        public string MentorName
        {
            get
            {
                fixed (sbyte* ptr = _mentorname)
                {
                    return new string(ptr).TrimEnd('\0');
                }
            }
            set
            {
                fixed (sbyte* ptr = _mentorname)
                {
                    MSVCRT.memset(ptr, 0, Constants.MAX_NAMESIZE);
                    value.CopyTo(ptr);
                }
            }
        }
        #endregion

        public static implicit operator byte[](MentorOfflinePacket packet)
        {
            byte[] data = new byte[166];// + packet.MentorName.Length];
            fixed (byte* ptr = data)
            {
                PacketBuilder.AppendHeader(ptr, data.Length, 2066);
                *((uint*)(ptr + 4)) = packet.MentorType;
                *((uint*)(ptr + 8)) = packet.MentorID;//client UID
                *((uint*)(ptr + 12)) = packet.ApprenticeID;
                *((uint*)(ptr + 24)) = 999999;
                *((uint*)(ptr + 52)) = packet.Type2;
                *((byte*)(ptr + 76)) = packet.StringCount;
                *((byte*)(ptr + 77)) = (byte)packet.MentorName.Length;


                MSVCRT.memcpy(ptr + 78, packet._mentorname, 16);
                /*((byte*)(ptr + 78 + packet.MentorName.Length)) = (byte)packet.MentorSpouse.Length;
                MSVCRT.memcpy(ptr + 79 + packet.MentorName.Length, packet._mentorspouse, packet.MentorSpouse.Length);
                *((byte*)(ptr + 79 + packet.MentorName.Length + packet.MentorSpouse.Length)) = (byte)packet.ApprenticeName.Length;
                MSVCRT.memcpy(ptr + 80 + packet.MentorName.Length + packet.MentorSpouse.Length, packet._apprenticename, packet.ApprenticeName.Length);*/
                int Counter = 0;

                /*for (Int32 i = 0; i < packet.Strings.Length; i++)
                {
                    *((Byte*)(ptr + 77 + Counter)) = (Byte)packet.Strings[i].Length;
                    for (Byte x = 0; x < (Byte)packet.Strings[i].Length; x++)
                        *((Byte*)(ptr + 78 + Counter + x)) = (Byte)packet.Strings[i][x];
                    Counter += packet.Strings[i].Length + 1;
                }*/
            }
            return data;
        }
    }
    public unsafe struct MentorInformationPacket
    {
        public string[] Strings;
        public uint MentorType;
        public uint Type2;
        public uint MentorID;
        public uint ApprenticeID;
        public uint LookFace; //MentorMesh
        public uint SharedBP;
        public uint Unknown;
        public uint Enroll;
        public uint MentorLevel;
        public byte MentorClass;
        public short MentorPKP;
        public byte MentorOnline;
        public uint ApprenticeExp;
        public ushort ApprenticeBless;
        public ushort ApprenticeComposing;
        public byte StringCount;
        public uint Unknown2, Unknown3, Unknown4, Unknown5, Unknown6, Unknown7, Unknown8;
        #region MentorName
        private fixed sbyte _mentorname[Constants.MAX_NAMESIZE];
        public string MentorName
        {
            get
            {
                fixed (sbyte* ptr = _mentorname)
                {
                    return new string(ptr).TrimEnd('\0');
                }
            }
            set
            {
                fixed (sbyte* ptr = _mentorname)
                {
                    MSVCRT.memset(ptr, 0, Constants.MAX_NAMESIZE);
                    value.CopyTo(ptr);
                }
            }
        }
        #endregion
        #region ApprenticeName
        private fixed sbyte _apprenticename[Constants.MAX_NAMESIZE];
        public string ApprenticeName
        {
            get
            {
                fixed (sbyte* ptr = _apprenticename)
                {
                    return new string(ptr).TrimEnd('\0');
                }
            }
            set
            {
                fixed (sbyte* ptr = _apprenticename)
                {
                    MSVCRT.memset(ptr, 0, Constants.MAX_NAMESIZE);
                    value.CopyTo(ptr);
                }
            }
        }
        #endregion
        #region MentorSpouse
        private fixed sbyte _mentorspouse[Constants.MAX_NAMESIZE];
        public string MentorSpouse
        {
            get
            {
                fixed (sbyte* ptr = _mentorspouse)
                {
                    return new string(ptr).TrimEnd('\0');
                }
            }
            set
            {
                fixed (sbyte* ptr = _mentorspouse)
                {
                    MSVCRT.memset(ptr, 0, Constants.MAX_NAMESIZE);
                    value.CopyTo(ptr);
                }
            }
        }
        #endregion

        public static MentorInformationPacket Create(Player user)
        {
            MentorInformationPacket packet = new MentorInformationPacket();
            packet.MentorName = user.Name;
            packet.MentorSpouse = "Spouse";
            packet.MentorID = user.UID;
            packet.MentorLevel = user.Level;
            //packet.MentorOnline = user.Character.Online;
            packet.MentorClass = (byte)user.ProfessionType;
            packet.MentorPKP = user.PK; //should be ushort but PKP is short
            packet.LookFace = user.Lookface;
            return packet;
        }

        public static implicit operator MentorInformationPacket(byte* ptr)
        {
            var packet = new MentorInformationPacket();
            packet.MentorType = *((uint*)(ptr + 4));
            packet.MentorID = *((uint*)(ptr + 8));
            packet.ApprenticeID = *((uint*)(ptr + 12));
            packet.LookFace = *((uint*)(ptr + 16));
            packet.SharedBP = *((uint*)(ptr + 20));
            packet.Unknown = *((uint*)(ptr + 24));
            packet.Enroll = *((uint*)(ptr + 28));
            packet.MentorLevel = *((uint*)(ptr + 32));
            packet.MentorClass = *((byte*)(ptr + 33));
            packet.MentorPKP = *((short*)(ptr + 34));
            packet.MentorOnline = *((byte*)(ptr + 56));
            packet.ApprenticeExp = *((uint*)(ptr + 64));
            packet.ApprenticeBless = *((ushort*)(ptr + 72));
            packet.ApprenticeComposing = *((ushort*)(ptr + 74));
            packet.StringCount = *((byte*)(ptr + 76));
            Redux.MSVCRT.memcpy(packet._mentorname, ptr + 77, 16);
            Redux.MSVCRT.memcpy(packet._mentorspouse, ptr + 79 + packet.MentorName.Length, 16);
            Redux.MSVCRT.memcpy(packet._apprenticename, ptr + 81, 16);
            packet.Unknown2 = *((uint*)(ptr + 57));
            packet.Unknown3 = *((uint*)(ptr + 58));
            packet.Unknown4 = *((uint*)(ptr + 59));
            packet.Unknown5 = *((uint*)(ptr + 60));
            packet.Unknown6 = *((uint*)(ptr + 61));
            packet.Unknown7 = *((uint*)(ptr + 62));
            packet.Unknown8 = *((uint*)(ptr + 63));

            return packet;
        }

        public static implicit operator byte[](MentorInformationPacket packet)
        {
            byte[] data = new byte[166];// + packet.MentorName.Length];
            fixed (byte* ptr = data)
            {
                PacketBuilder.AppendHeader(ptr, data.Length, 2066);
                *((uint*)(ptr + 4)) = packet.MentorType;
                *((uint*)(ptr + 8)) = packet.MentorID;//client UID
                *((uint*)(ptr + 12)) = packet.ApprenticeID;
                *((uint*)(ptr + 16)) = packet.LookFace;
                *((uint*)(ptr + 20)) = packet.SharedBP;
                *((uint*)(ptr + 24)) = 999999;
                *((uint*)(ptr + 28)) = packet.Enroll;
                *((byte*)(ptr + 32)) = (byte)packet.MentorLevel;
                *((byte*)(ptr + 33)) = packet.MentorClass;
                *((short*)(ptr + 34)) = packet.MentorPKP; // should be ushort but PKP is short
                /**((uint*)(ptr + 36)) = 0;
                *((uint*)(ptr + 40)) = 0;
                *((uint*)(ptr + 44)) = 0;
                *((uint*)(ptr + 48)) = 0;*/
                *((uint*)(ptr + 52)) = packet.Type2;
                *((uint*)(ptr + 56)) = packet.MentorOnline;
                *((uint*)(ptr + 60)) = 0;
                *((long*)(ptr + 64)) = packet.ApprenticeExp;
                *((ushort*)(ptr + 72)) = packet.ApprenticeBless;
                *((ushort*)(ptr + 74)) = packet.ApprenticeComposing;
                *((byte*)(ptr + 76)) = packet.StringCount;
                *((byte*)(ptr + 77)) = (byte)packet.MentorName.Length;

                
                MSVCRT.memcpy(ptr + 78, packet._mentorname, 16);
                /*((byte*)(ptr + 78 + packet.MentorName.Length)) = (byte)packet.MentorSpouse.Length;
                MSVCRT.memcpy(ptr + 79 + packet.MentorName.Length, packet._mentorspouse, packet.MentorSpouse.Length);
                *((byte*)(ptr + 79 + packet.MentorName.Length + packet.MentorSpouse.Length)) = (byte)packet.ApprenticeName.Length;
                MSVCRT.memcpy(ptr + 80 + packet.MentorName.Length + packet.MentorSpouse.Length, packet._apprenticename, packet.ApprenticeName.Length);*/
                int Counter = 0;

                /*for (Int32 i = 0; i < packet.Strings.Length; i++)
                {
                    *((Byte*)(ptr + 77 + Counter)) = (Byte)packet.Strings[i].Length;
                    for (Byte x = 0; x < (Byte)packet.Strings[i].Length; x++)
                        *((Byte*)(ptr + 78 + Counter + x)) = (Byte)packet.Strings[i][x];
                    Counter += packet.Strings[i].Length + 1;
                }*/
            }
            return data;
        }


    }
}
