using Redux.Database.Domain;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using System.Collections.Generic;
using NHibernate.Transform;

namespace Redux.Database.Repositories
{
    public class RebornRepository : Repository<uint, DbRebornPath>
    {
        public IList<DbRebornPath> GetRebornByPath(uint _path)
        {

            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbRebornPath>()
                    .Add(Restrictions.Eq("RebornPath", _path))
                    .List<DbRebornPath>();
            }
        }      
    }
}

