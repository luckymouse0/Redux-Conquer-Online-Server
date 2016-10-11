/*
 * User: pro4never
 * Date: 9/24/2013
 * Time: 9:29 PM
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;
using Redux.Game_Server;

namespace Redux.Packets.Game
{
    public unsafe struct OfflineInfoPacket
    {

        public uint TimeUsed, TimeRemaining, Level;
        public uint Exp;

        public static OfflineInfoPacket Create(Player client)
        {
            OfflineInfoPacket packet = new OfflineInfoPacket();



            uint Time = (uint)Math.Min(client.Character.TrainingTime, DateTime.Now.Subtract(client.Character.OfflineTGEntered).TotalMinutes);
            
            var Info = packet.CalcExp(client, Time);

            if (Info != null && Info.Count == 2)
            {
                packet.Level = (uint)Info[0];
                packet.Exp = Info[1];
            }
            
            packet.TimeUsed = Time;
            packet.TimeRemaining = client.Character.TrainingTime - Time;
            return packet;
        }

        public static implicit operator OfflineInfoPacket(byte* ptr)
        {
            var packet = new OfflineInfoPacket();
            packet.TimeUsed = *((uint*)(ptr + 4));
            packet.TimeRemaining = *((uint*)(ptr + 8));
            packet.Level = *((uint*)(ptr + 12));
            packet.Exp = *((uint*)(ptr + 16));
            
            return packet;
        }

        public static implicit operator byte[](OfflineInfoPacket packet)
        {
            var buffer = new byte[80];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, 2043);
                *((short*)(ptr + 4)) = (short)packet.TimeUsed;
                *((uint*)(ptr + 8)) = packet.Level;
                *((uint*)(ptr + 12)) = packet.Exp;
                *((short*)(ptr + 6)) =(short)packet.TimeRemaining;

            }
            return buffer;
        }

        #region Calculate Exp Ball
        public List<uint> CalcExp(Player client, double _time)
        {
            
            var Experience = client.Experience;
            var UserLevel = client.Level;
            List<uint> Info = new List<uint>();

            if (UserLevel >= 140)
                return null;
            var requires = Database.ServerDatabase.Context.LevelExp.GetById(UserLevel);
            if (requires == null)
                return null;

            var timeRemaining = (double)requires.UpLevTime * ((double)requires.Experience - (double)client.Experience) / (double)requires.Experience;

            if (_time >= timeRemaining)
            {
                Experience = 0;
                while (requires != null && _time >= timeRemaining)
                {
                    UserLevel++;
                    
                    _time -= timeRemaining;
                    requires = Database.ServerDatabase.Context.LevelExp.GetById(UserLevel);
                    timeRemaining = requires.UpLevTime;
                }
                
            }

            if (requires != null && _time > 0)
                Experience += (ulong)((double)requires.Experience / (double)requires.UpLevTime * _time);

            Info.Add(UserLevel);
            double exp = (((double)Experience / (double)requires.Experience));
            Info.Add((uint)(exp * 10000000));


            return Info;

        }
        #endregion
    }
}
