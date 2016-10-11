using Redux.Enum;
using Redux.Packets.Game;
using System;

namespace Redux.Database.Domain
{
    public class DbChatLog
    {
        public DbChatLog(TalkPacket _packet)
        {
            Time = DateTime.Now;
            Type = _packet.Type;
            From = _packet.Speaker;
            To = _packet.Hearer;
            Message = _packet.Words;
        }
        public DbChatLog()
        {
        }
        public virtual uint UID { get; set; }
        public virtual DateTime Time { get; set; }
        public virtual ChatType Type { get; set; }
        public virtual string From { get; set; }
        public virtual string To { get; set; }
        public virtual string Message { get; set; }
    }
}

