using Redux.Enum;
using Redux.Structures;
using System;

namespace Redux.Database.Domain
{
    public class DbItemLog
    {
        public DbItemLog(DbItem _item)
        {
            UniqueID = _item.UniqueID;
            Time = DateTime.Now;
            FormerOwner = _item.Owner;
            NewOwner = 0;
            StaticID = _item.StaticID;
            Durability = _item.Durability;
            Color = _item.Color;
            Plus = _item.Plus;
            Bless = _item.Bless;
            Enchant = _item.Enchant;
            Gem1 = _item.Gem1;
            Gem2 = _item.Gem2;
            Effect = _item.Effect;
        }
        public DbItemLog()
        {
        }
        public virtual uint LogUID { get; set; }
        public virtual uint UniqueID { get; set; }
        public virtual DateTime Time { get; set; }
        public virtual uint FormerOwner { get; set; }
        public virtual uint NewOwner { get; set; }
        public virtual uint StaticID { get; set; }
        public virtual ushort Durability { get; set; }
        public virtual byte Color { get; set; }
        public virtual byte Plus { get; set; }
        public virtual byte Bless { get; set; }
        public virtual byte Enchant { get; set; }
        public virtual byte Gem1 { get; set; }
        public virtual byte Gem2 { get; set; }
        public virtual byte Effect { get; set; }
    }
}

