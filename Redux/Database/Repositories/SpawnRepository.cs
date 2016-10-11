using System.Collections;
using System;
using System.Collections.Generic;
using Redux.Database.Domain;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using Redux.Structures;

namespace Redux.Database.Repositories
{
    public class SpawnRepository : Repository<uint, DbSpawn>
    {
       
        public IList<DbSpawn> GetSpawnsByMap(ushort _map)
        {

            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbSpawn>()
                    .Add(Restrictions.Eq("Map", _map))
                    .List<DbSpawn>();
            }
        }
    }
}

