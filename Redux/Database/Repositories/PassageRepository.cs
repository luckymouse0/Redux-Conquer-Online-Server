using Redux.Database.Domain;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using Redux.Game_Server;
namespace Redux.Database.Repositories
{
    public class PassageRepository : Repository<uint, DbPassage>
    {
        public DbPortal GetPortalByMapAndID(ushort _map, uint _id)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return ServerDatabase.Context.Portals.GetPortalByPassage(session.CreateCriteria<DbPassage>()
                    .Add(Restrictions.Eq("EnterMap", _map))
                    .Add(Restrictions.Eq("EnterID", _id))
                    .List<DbPassage>()[0]);
            }
        }
        
    }
}

