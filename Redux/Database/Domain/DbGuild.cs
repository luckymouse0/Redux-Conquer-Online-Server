using Redux.Enum;

namespace Redux.Database.Domain
{
    public class DbGuild
    {
        public virtual uint Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Announce { get; set; }
        public virtual uint AnnounceDate { get; set; }
        public virtual uint LeaderId { get; set; }
        public virtual string LeaderName { get; set; }
        public virtual long Money { get; set; }
        public virtual uint MasterGuildId { get; set; }
        public virtual bool DeleteFlag { get; set; }
        public virtual int Amount { get; set; }

        public virtual uint Enemy0 { get; set; }
        public virtual uint Enemy1 { get; set; }
        public virtual uint Enemy2 { get; set; }
        public virtual uint Enemy3 { get; set; }
        public virtual uint Enemy4 { get; set; }

        public virtual uint Ally0 { get; set; }
        public virtual uint Ally1 { get; set; }
        public virtual uint Ally2 { get; set; }
        public virtual uint Ally3 { get; set; }
        public virtual uint Ally4 { get; set; }
    }
}

