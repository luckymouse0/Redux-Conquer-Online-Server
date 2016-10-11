using Redux.Database.Domain;
using NHibernate.Criterion;
using Redux.Enum;
using System.Collections.Generic;

namespace Redux.Database.Repositories
{
    public class EventsRepository : Repository<uint, DbEvents>
    {
        /// <summary>
        /// Removes, at most, one entry from the database where the user id is <paramref name="userId"/> and guild id is <paramref name="guildId"/> and the rank is not <paramref name="guildRank"/>.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="guildId"></param>
        /// <param name="guildRank"></param>
        public DbEvents GetWinner()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbEvents>()
                    .Add(Restrictions.Eq("ID", (uint)1))
                    .UniqueResult<DbEvents>();
            }
        }

        public IList<DbGuildAttr> GetMembers(uint GuildId)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbGuildAttr>()
                    .Add(Restrictions.Eq("GuildId", GuildId))
                    .List<DbGuildAttr>();
            }
        }
    }
}

