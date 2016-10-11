using System.Collections.Generic;
using Redux.Database.Domain;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace Redux.Database.Repositories
{
    public class SobRepository : Repository<uint, DbSob>
    {
        public DbSob GetByUID(uint uid)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbSob>()
                    .Add(Restrictions.Eq("UID", uid))
                    .UniqueResult<DbSob>();
            }
        }

        public IList<DbSob> GetSOBByMap(uint _map)
        {

            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbSob>()
                    .Add(Restrictions.Eq("Map", _map))
                    .List<DbSob>();
            }
        }

        public void SetGwWinner(string name)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                var t = session.CreateSQLQuery("UPDATE sobs SET Name=" + name + " WHERE UID=6700");
                
                t.ExecuteUpdate();
            }
        }
    }
}

