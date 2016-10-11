using System.Collections;
using System;
using System.Collections.Generic;
using Redux.Database.Domain;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using Redux.Structures;

namespace Redux.Database.Repositories
{
    public class MineDropRepository : Repository<uint, DbMineDrop>
    {
        public IList<DbMineDrop> GetDropByMap(ushort _map)
        {

            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbMineDrop>()
                    .Add(Restrictions.Eq("MapID", _map))
                    .List<DbMineDrop>();
            }
        }
    }
}

