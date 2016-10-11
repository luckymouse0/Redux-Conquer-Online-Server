using Redux.Enum;

namespace Redux.Database.Domain
{
    public class DbGuildAttr
    {
        public virtual uint Id { get; set; }
        public virtual uint GuildId { get; set; }
        public virtual GuildRank Rank { get; set; }
        public virtual int SilverDonation { get; set; }
        public virtual uint JoinDate { get; set; }
    }
}

