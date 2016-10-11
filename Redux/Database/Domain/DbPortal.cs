using Redux.Enum;

namespace Redux.Database.Domain
{
    public class DbPortal
    {
        public virtual uint UID { get; set; }
        public virtual ushort MapID { get; set; }
        public virtual uint PortalID { get; set; }
        public virtual ushort PortalX { get; set; }
        public virtual ushort PortalY { get; set; }
    }
}