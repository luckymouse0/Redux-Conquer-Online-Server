using Redux.Enum;

namespace Redux.Database.Domain
{
    public class DbGuildMemberInfo
    {
        public virtual string Name { get; set; }
        public virtual GuildRank Rank { get; set; }
        public virtual int Level { get; set; }
        public virtual long Donation { get; set; }
    }
}

