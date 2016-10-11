using System.Collections.Generic;
using Redux.Database.Domain;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace Redux.Database.Repositories
{
    public class ProficiencyRepository : Repository<uint, DbProficiency>
    {
        public IList<DbProficiency> GetUserProficiency(uint owner)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbProficiency>()
                    .Add(Restrictions.Eq("Owner", owner))
                    .List<DbProficiency>();
            }
        }
        public bool ProficiencyExists(uint owner, ushort id)
        {
             using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbProficiency>()
                    .Add(Restrictions.Eq("Owner", owner))
                    .Add(Restrictions.Eq("ID", id))
                    .List<DbProficiency>().Count > 0;
             }
        }
    }
}
