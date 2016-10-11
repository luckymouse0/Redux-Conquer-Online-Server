using System.Collections.Generic;
using Redux.Database.Domain;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace Redux.Database.Repositories
{
    public class AssociateRepository : Repository<uint, DbAssociate>
    {
        public IList<DbAssociate> GetUserAssociates(uint id)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbAssociate>()
                    .Add(Restrictions.Eq("UID", id))
                    .List<DbAssociate>();
            }
        }

        public void Remove(uint ownerid, uint associateid, Enum.AssociateType type)
        {
            DbAssociate rel;
            using (var session = NHibernateHelper.OpenSession())
            {
                rel = session
                     .CreateCriteria<DbAssociate>()
                     .Add(Restrictions.Eq("UID", ownerid))
                     .Add(Restrictions.Eq("AssociateID", associateid))
                     .Add(Restrictions.Eq("Type", (byte) type))
                     .UniqueResult<DbAssociate>();
            }
            ServerDatabase.Context.Associates.Remove(rel);
        }

        public bool AssociateExists(uint ownerid, uint associateid, Enum.AssociateType type)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbAssociate>()
                    .Add(Restrictions.Eq("UID", ownerid))
                    .Add(Restrictions.Eq("AssociateID", associateid))
                     .Add(Restrictions.Eq("Type", (byte)type))
                    .List<DbAssociate>().Count > 0;
            }
        }
    }
}

