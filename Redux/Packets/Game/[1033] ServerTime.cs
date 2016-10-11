using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redux.Packets.Game
{
    public unsafe struct ServerTimePacket
    {
        public DateTime Time;

        public static ServerTimePacket Create()
        {
            var packet = new ServerTimePacket
            {
                Time = DateTime.UtcNow
                    .AddHours(Constants.TIME_ADJUST_HOUR)
                    .AddMinutes(Constants.TIME_ADJUST_MIN)
                    .AddSeconds(Constants.TIME_ADJUST_SEC)
            };
            return packet;
        }

        public static implicit operator byte[](ServerTimePacket packet)
        {
            var buffer = new byte[36 + 8];
            fixed (byte* ptr = buffer)
            {
                PacketBuilder.AppendHeader(ptr, buffer.Length, Constants.MSG_DATE_TIME);
                *((int*)(ptr + 8)) = packet.Time.Year - 1900;
                *((int*)(ptr + 12)) = packet.Time.Month - 1;
                *((int*)(ptr + 16)) = packet.Time.DayOfYear;
                *((int*)(ptr + 20)) = packet.Time.Day;
                *((int*)(ptr + 24)) = packet.Time.Hour;
                *((int*)(ptr + 28)) = packet.Time.Minute;
                *((int*)(ptr + 32)) = packet.Time.Second;
            }
            return buffer;
        }
    }
}
