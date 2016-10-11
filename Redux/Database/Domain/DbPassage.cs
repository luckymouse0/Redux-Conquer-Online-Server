using Redux.Enum;

namespace Redux.Database.Domain
{
    public class DbPassage
    {
        public virtual uint UID { get; set; }
        public virtual ushort EnterMap { get; set; }
        public virtual uint EnterID { get; set; }
        public virtual ushort ExitMap { get; set; }
        public virtual uint ExitID { get; set; }
    }
}