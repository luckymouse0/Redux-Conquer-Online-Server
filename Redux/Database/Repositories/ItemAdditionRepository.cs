using Redux.Database.Domain;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using System;

namespace Redux.Database.Repositories
{
    public class ItemAdditionRepository : Repository<uint, DbItemAddition>
    {  		
        public DbItemAddition GetByItem(Structures.ConquerItem item)
        {
            if (item.Plus == 0)
                return null;
            uint id = item.StaticID;
            id = (id / 10 * 100);    
            id += item.Plus;
            try
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    return session
                        .CreateCriteria<DbItemAddition>()
                        .Add(Restrictions.Eq("Key", id))
                        .UniqueResult<DbItemAddition>();
                }
            }
            catch { return null; }
        }    	
    }
}

