using System.Collections;
using System;
using System.Collections.Generic;
using Redux.Database.Domain;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using Redux.Structures;

namespace Redux.Database.Repositories
{
    public class TaskRepository : Repository<uint, DbTask>
    {
        public IList<DbTask> GetTasksByPlayerUID(uint uit)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbTask>()
                    .Add(Restrictions.Eq("Owner", uit))
                    .List<DbTask>();
            }
        }
    }
}

