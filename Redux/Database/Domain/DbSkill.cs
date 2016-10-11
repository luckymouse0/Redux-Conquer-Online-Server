using Redux.Enum;

namespace Redux.Database.Domain
{
    public class DbSkill
    {
        public virtual uint UID { get; set; }
        public virtual uint Owner { get; set; }
        public virtual ushort ID { get; set; }
        public virtual ushort Level { get; set; }
        public virtual uint Experience { get; set; }
        public virtual ushort PreviousLevel { get; set; }
    }
}
