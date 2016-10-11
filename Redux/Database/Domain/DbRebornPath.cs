namespace Redux.Database.Domain
{
    public class DbRebornPath
    {
        public virtual uint UID { get; set; }
        public virtual uint RebornPath { get; set; }
        public virtual bool IsForget { get; set; }
        public virtual ushort SkillId { get; set; }
    }
}

