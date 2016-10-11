using Redux.Enum;

namespace Redux.Database.Domain
{
    public class DbMap
    {
        public virtual uint UID { get; set; }
        public virtual uint ID { get; set; }
        public virtual string Name { get; set; }
        public virtual MapTypeFlags Type { get; set; }
        public virtual ushort SpawnID { get; set; }
        public virtual ushort SpawnX { get; set; }
        public virtual ushort SpawnY { get; set; }
    }
}
