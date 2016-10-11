using Redux.Database.Domain;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace Redux.Database.Repositories
{
    public class PortalRepository : Repository<uint, DbPortal>
    {
        public DbPortal GetPortalByPassage(DbPassage _passage)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbPortal>()
                    .Add(Restrictions.Eq("MapID", _passage.ExitMap))
                    .Add(Restrictions.Eq("PortalID", _passage.ExitID))
                    .UniqueResult<DbPortal>();
            }
        }
    }
}

