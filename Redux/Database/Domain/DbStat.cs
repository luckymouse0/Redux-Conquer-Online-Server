using Redux.Enum;

namespace Redux.Database.Domain
{
    public class DbStat
    {
        public virtual uint UID { get; set; }
        public virtual ushort ProfessionType { get; set; }
        public virtual byte Level { get; set; }
        public virtual ushort Vitality { get; set; }
        public virtual ushort Strength { get; set; }
        public virtual ushort Agility { get; set; }
        public virtual ushort Spirit { get; set; }
    }
}

