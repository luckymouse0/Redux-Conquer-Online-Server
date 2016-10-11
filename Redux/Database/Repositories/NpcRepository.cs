using System.Collections;
using System.Collections.Generic;
using Redux.Database.Domain;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace Redux.Database.Repositories
{
    public class NpcRepository : Repository<uint, DbNpc>
    {
        public IList<DbNpc> GetNpcsByMap(ushort _map)
        {

            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbNpc>()
                    .Add(Restrictions.Eq("Map", _map))
                    .List<DbNpc>();
            }
        }
    }
}

