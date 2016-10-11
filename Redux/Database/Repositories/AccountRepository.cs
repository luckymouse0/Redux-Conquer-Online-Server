using Redux.Database.Domain;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace Redux.Database.Repositories
{
    public class AccountRepository : Repository<uint, DbAccount>
    {
        public void ResetLoginTokens()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                var t = session.CreateSQLQuery("UPDATE Accounts SET token=0 WHERE token>0");            
                t.ExecuteUpdate();
            }
        }
        public DbAccount GetByToken(uint token)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbAccount>()
                    .Add(Restrictions.Eq("Token", token))
                    .UniqueResult<DbAccount>();
            }
        }
        public DbAccount GetByName(string name)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbAccount>()
                    .Add(Restrictions.Eq("Username", name))
                    .UniqueResult<DbAccount>();
            }
        }

        public void DeleteCharacter(uint _uid)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                session
                   .GetNamedQuery("DeleteCharacter")
                   .SetParameter("_uid", _uid)
                    .ExecuteUpdate();
            }
        }
    }
}

