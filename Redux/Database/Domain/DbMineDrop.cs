using Redux.Enum;

namespace Redux.Database.Domain
{
    public class DbMineDrop
    {
        public virtual uint EntryID { get; set; }
        public virtual uint MapID { get; set; }
        public virtual short Type { get; set; }
        public virtual uint Drop1 { get; set; }
        public virtual uint Drop2 { get; set; }
        public virtual uint Drop3 { get; set; }
        public virtual uint Drop4 { get; set; }
        public virtual uint Drop5 { get; set; }
        public virtual uint Drop6 { get; set; }
    }
}

