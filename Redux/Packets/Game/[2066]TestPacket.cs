using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;
using Redux.Game_Server;

namespace Redux.Packets.Game
{
    /*Mentor Information Packet Blueprints : http://prntscr.com/1w2hcp 
     Patch 5500 but should more or less work with some tweaking.*/
    /*Following Example : SpawnEntity packet*/
    #region Mentor
    public unsafe struct TestMentor
    {
        
        public static byte[] AppInfo(uint type, uint type2, Player Client, Player Appr)
        {
            byte[] data = new byte[192 + 8 + Client.Name.Length + Appr.Name.Length];// + packet.MentorName.Length];
            fixed (byte* ptr = data)
            {
                PacketBuilder.AppendHeader(ptr, data.Length, 2066);
                *((uint*)(ptr + 4)) = type;
                *((uint*)(ptr + 8)) = Client.UID;
                if (type != 3)
                {
                    *((uint*)(ptr + 12)) = Appr.UID;
                    *((uint*)(ptr + 16)) = Appr.Lookface;
                }
                else
                {
                    *((uint*)(ptr + 12)) = 0;
                    *((uint*)(ptr + 16)) = 0;
                }
                *((uint*)(ptr + 20)) = 0;
                *((uint*)(ptr + 24)) = 999999;
                
                if (type != 3)
                {
                    *((uint*)(ptr + 28)) = 20131111;
                    *((byte*)(ptr + 32)) = Appr.Level;
                    *((byte*)(ptr + 33)) = Appr.Character.Profession;
                    *((short*)(ptr + 34)) = Appr.PK;
                }
                else
                {
                    *((uint*)(ptr + 28)) = 0;
                    *((byte*)(ptr + 32)) = 0;
                    *((byte*)(ptr + 33)) = 0;
                    *((short*)(ptr + 34)) = 0;
                }
                *((uint*)(ptr + 36)) = 0;
                *((uint*)(ptr + 40)) = 0;
                *((uint*)(ptr + 44)) = 0;
                *((uint*)(ptr + 48)) = 0;
                *((uint*)(ptr + 52)) = type2;
                if (type != 3)
                    *((uint*)(ptr + 56)) = 1;
                else
                    *((uint*)(ptr + 56)) = 0;
                *((uint*)(ptr + 60)) = 0;
                *((long*)(ptr + 64)) = 0;
                *((short*)(ptr + 72)) = 0;//blessing hours
                *((short*)(ptr + 74)) = 0;//+1 Stones
                *((byte*)(ptr + 76)) = 3;
                *((byte*)(ptr + 77)) = (byte)Client.Name.Length;
                for (Byte x = 0; x < (Byte)Client.Name.Length; x++)
                    *((Byte*)(ptr + 78 + x)) = (Byte)Client.Name[x];
                int off = Client.Name.Length;
                if (type != 3)
                {
                    *((byte*)(ptr + 78 + off)) = (byte)Appr.Name.Length;
                    for (Byte x = 0; x < (Byte)Appr.Name.Length; x++)
                        *((Byte*)(ptr + 79 + x + off)) = (Byte)Appr.Name[x];
                    off += Appr.Name.Length;
                    *((byte*)(ptr + 79 + off)) = (byte)Appr.Spouse.Length;
                    for (Byte x = 0; x < (Byte)Appr.Spouse.Length; x++)
                        *((Byte*)(ptr + 80 + x + off)) = (Byte)Appr.Spouse[x];
                }
                else
                {
                    *((byte*)(ptr + 78 + off)) = (byte)0;
                    *((byte*)(ptr + 79 + off)) = (byte)0;
                }
                return data;
            }
        }

        public static byte[] Mentor(uint type, uint type2, Player Client, Player Appr)
        {
            byte[] data = new byte[192 + 8 + Client.Name.Length + Appr.Name.Length];// + packet.MentorName.Length];
            fixed (byte* ptr = data)
            {
                PacketBuilder.AppendHeader(ptr, data.Length, 2066);
                *((uint*)(ptr + 4)) = type;
                *((uint*)(ptr + 8)) = Client.UID;
                *((uint*)(ptr + 12)) = Appr.UID;
                *((uint*)(ptr + 16)) = Client.Lookface;
                *((uint*)(ptr + 20)) = 7;//shared bp
                *((uint*)(ptr + 24)) = 999999;
                *((uint*)(ptr + 28)) = 20131111;
                *((byte*)(ptr + 32)) = Client.Level;
                *((byte*)(ptr + 33)) = Client.Character.Profession;
                *((short*)(ptr + 34)) = Client.PK;
                *((uint*)(ptr + 36)) = 0;
                *((uint*)(ptr + 40)) = 0;
                *((uint*)(ptr + 44)) = 0;
                *((uint*)(ptr + 48)) = 0;
                *((uint*)(ptr + 52)) = type2;
                *((uint*)(ptr + 56)) = 1;
                *((uint*)(ptr + 60)) = 0;
                *((long*)(ptr + 64)) = 0;
                *((short*)(ptr + 72)) = 0;
                *((short*)(ptr + 74)) = 0;
                *((byte*)(ptr + 76)) = 3;
                *((byte*)(ptr + 77)) = (byte)Client.Name.Length;
                for (Byte x = 0; x < (Byte)Client.Name.Length; x++)
                    *((Byte*)(ptr + 78 + x)) = (Byte)Client.Name[x];
                int off = Client.Name.Length;
                *((byte*)(ptr + 78 + off)) = (byte)Appr.Name.Length;
                for (Byte x = 0; x < (Byte)Appr.Name.Length; x++)
                    *((Byte*)(ptr + 79 + x + off)) = (Byte)Appr.Name[x];
                off += Appr.Name.Length;
                *((byte*)(ptr + 79 + off)) = (byte)Client.Spouse.Length;
                for (Byte x = 0; x < (Byte)Client.Spouse.Length; x++)
                    *((Byte*)(ptr + 80 + x + off)) = (Byte)Client.Spouse[x];

                return data;
            }
        }
        public static byte[] OfflineMentor(uint type, uint type2, string Name, uint UID, uint ApprUID, string AppName)
        {
            
            byte[] data = new byte[112 + 8 + Name.Length + AppName.Length];// + packet.MentorName.Length];
            fixed (byte* ptr = data)
            {
                PacketBuilder.AppendHeader(ptr, data.Length, 2066);
                *((uint*)(ptr + 4)) = type;
                *((uint*)(ptr + 8)) = UID;
                *((uint*)(ptr + 12)) = ApprUID;
                *((uint*)(ptr + 24)) = 999999;
                *((uint*)(ptr + 52)) = type2;
                *((byte*)(ptr + 76)) = 3;
                *((byte*)(ptr + 77)) = (byte)Name.Length;
                for (Byte x = 0; x < (Byte)Name.Length; x++)
                    *((Byte*)(ptr + 78 + x)) = (Byte)Name[x];
                int off = Name.Length;
                *((byte*)(ptr + 78 + off)) = (byte)AppName.Length;
                for (Byte x = 0; x < (Byte)AppName.Length; x++)
                    *((Byte*)(ptr + 79 + x + off)) = (Byte)AppName[x];
                return data;
            }
        }

    #endregion
    }
}