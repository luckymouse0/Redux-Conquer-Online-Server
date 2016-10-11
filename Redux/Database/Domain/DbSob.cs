using Redux.Enum;

namespace Redux.Database.Domain
{
    public class DbSob
    {
        public virtual uint UID { get; set; }
        public virtual ushort Mesh { get; set; }
        public virtual uint MaxHp { get; set; }
        public virtual ushort Flag { get; set; }
        public virtual uint Map { get; set; }
        public virtual ushort X { get; set; }
        public virtual ushort Y { get; set; }
        public virtual string Name { get; set; }
        public virtual byte Level { get; set; }        
    }
}

