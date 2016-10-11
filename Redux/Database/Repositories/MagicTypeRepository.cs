using System.Collections.Generic;
using System;
using Redux.Structures;
using Redux.Database.Domain;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace Redux.Database.Repositories
{
    public class MagicTypeRepository : Repository<uint, DbMagicType>
    {
        public DbMagicType GetMagicTypeBySkill(ConquerSkill _skill)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                          .CreateCriteria<DbMagicType>()
                          .Add(Restrictions.Eq("ID", _skill.ID))
                          .Add(Restrictions.Eq("Level", _skill.Level))
                          .UniqueResult<DbMagicType>();
            }
        }
        public DbMagicType GetMagictypeByIDAndLevel(ushort _id, ushort _level)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                          .CreateCriteria<DbMagicType>()
                          .Add(Restrictions.Eq("ID", _id))
                          .Add(Restrictions.Eq("Level", _level))
                          .UniqueResult<DbMagicType>();
            }
        }        
    }
}
