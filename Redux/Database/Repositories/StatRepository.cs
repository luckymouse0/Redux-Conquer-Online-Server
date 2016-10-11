using Redux.Database.Domain;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace Redux.Database.Repositories
{
    public class StatRepository : Repository<uint, DbStat>
    {
        public DbStat GetByProfessionAndLevel(ushort _professionType, byte _level)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbStat>()
                    .Add(Restrictions.Eq("ProfessionType", _professionType))
                    .Add(Restrictions.Eq("Level", _level))
                    .UniqueResult<DbStat>();
            }
        }
    }
}

