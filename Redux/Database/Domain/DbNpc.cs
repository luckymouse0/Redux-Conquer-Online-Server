using Redux.Enum;

namespace Redux.Database.Domain
{
    public class DbNpc
    {
        public virtual uint UID { get; set; }
        public virtual ushort Mesh { get; set; }
        public virtual ushort Map { get; set; }
        public virtual ushort X { get; set; }
        public virtual ushort Y { get; set; }
        public virtual NpcType Type { get; set; }
    }
}

