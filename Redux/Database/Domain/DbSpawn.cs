using Redux.Enum;

namespace Redux.Database.Domain
{
    public class DbSpawn
    {
        public virtual uint UID { get; set; }
        public virtual ushort Map { get; set; }
        public virtual ushort X1 { get; set; }
        public virtual ushort Y1 { get; set; }
        public virtual ushort X2 { get; set; }
        public virtual ushort Y2 { get; set; }
        public virtual uint MonsterType { get; set; }
        public virtual int AmountPer { get; set; }
        public virtual int AmountMax { get; set; }
        public virtual int Frequency { get; set; }



    }
}

