using Redux.Database.Domain;
using NHibernate.Criterion;

namespace Redux.Database.Repositories
{
    public class CharacterRepository : Repository<uint, DbCharacter>
    {
        public void ResetOnlineCharacters()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                var t = session.CreateSQLQuery("UPDATE Characters SET Online=0 WHERE Online>0");
                t.ExecuteUpdate();
            }
        }
        public DbCharacter GetByUID(uint uid)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbCharacter>()
                    .Add(Restrictions.Eq("UID", uid))
                    .UniqueResult<DbCharacter>();
            }
        }
        public DbCharacter GetByName(string name)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbCharacter>()
                    .Add(Restrictions.Eq("Name", name))
                    .UniqueResult<DbCharacter>();
            }
        }
        public void CreateEntry(DbCharacter character)
        {
            Add(character);
        }
    }
}

