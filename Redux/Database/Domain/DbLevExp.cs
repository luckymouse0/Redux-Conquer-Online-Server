using Redux.Enum;

namespace Redux.Database.Domain
{
    public class DbLevExp
    {
        public virtual uint Id { get; set; }
        public virtual uint Level { get; set; }
        public virtual ulong Experience { get; set; }
        public virtual int UpLevTime { get; set; }
        public virtual int Unknown { get; set; }
    }
}

