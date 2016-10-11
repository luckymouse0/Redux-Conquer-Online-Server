using Redux.Enum;
using System;
namespace Redux.Database.Domain
{
    public class DbBugReport
    {
        public virtual uint UID { get; set; }
        public virtual string Reporter { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime ReportedAt { get; set; }
        public virtual ReportStatus Status { get; set; }
    }
}

