using Redux.Database.Domain;
using NHibernate.Criterion;
using Redux.Enum;
using System.Collections.Generic;

namespace Redux.Database.Repositories
{
    public class GuildAttrRepository : Repository<uint, DbGuildAttr>
    {
        /// <summary>
        /// Removes, at most, one entry from the database where the user id is <paramref name="userId"/> and guild id is <paramref name="guildId"/> and the rank is not <paramref name="guildRank"/>.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="guildId"></param>
        /// <param name="guildRank"></param>
        public void DeleteGuildAttr(uint userId, uint guildId, GuildRank guildRank)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                session
                    .GetNamedQuery("DeleteUserGuildAttr")
                    .SetParameter("userId", userId)
                    .SetParameter("guildId", guildId)
                    .SetParameter("guildRank", guildRank)
                    .ExecuteUpdate();
            }
        }
        public void DeleteAttr(uint guildId)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                session
                    .GetNamedQuery("DeleteGuildAttr")
                    .SetParameter("guildId", guildId)
                    .ExecuteUpdate();
            }
        }
        public DbGuildAttr GetGuildId(uint UID)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbGuildAttr>()
                    .Add(Restrictions.Eq("id", UID))
                    .UniqueResult<DbGuildAttr>();
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

