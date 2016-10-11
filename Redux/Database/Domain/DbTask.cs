using System;
using Redux.Enum;
using Redux.Game_Server;

namespace Redux.Database.Domain
{
    public class DbTask
    {
        public virtual uint UID { get; set; }
        public virtual uint Owner { get; set; }
        public virtual TaskType Type { get; set; }
        public virtual DateTime Expires { get; set; }
        public virtual int Count { get; set; }
        public virtual int Condition { get; set; }
    }
}

