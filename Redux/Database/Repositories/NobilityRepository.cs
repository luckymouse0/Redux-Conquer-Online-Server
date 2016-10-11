using Redux.Database.Domain;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using System.Collections.Generic;
using NHibernate.Transform;

namespace Redux.Database.Repositories
{
    public class NobilityRepository : Repository<uint, DbNobility>
    {
        public DbNobility GetByName(string name)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbNobility>()
                    .Add(Restrictions.Eq("Name", name))
                    .UniqueResult<DbNobility>();
            }
        }

        public DbNobility GetByUID(uint UID)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbNobility>()
                    .Add(Restrictions.Eq("UID", UID))
                    .UniqueResult<DbNobility>();
            }
        }

        public long GetNobilityRank(long Donation)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .GetNamedQuery("GetNobilityRank")
                    .SetParameter("donationvalue", Donation)
                    .UniqueResult<long>();
            }
        }

        public IList<DbNobility> NobilityPages()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .GetNamedQuery("NobilityPages")
                    .SetResultTransformer(Transformers.AliasToBean<DbNobility>())
                    .List<DbNobility>();
            }
        }
    }
}

