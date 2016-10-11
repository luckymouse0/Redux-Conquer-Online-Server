using System.Collections.Generic;
using Redux.Database.Domain;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace Redux.Database.Repositories
{
    public class SkillRepository : Repository<uint, DbSkill>
    {
        public IList<DbSkill> GetUserSkills(uint uid)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbSkill>()
                    .Add(Restrictions.Eq("Owner", uid))
                    .List<DbSkill>();
            }
        }
        public bool SkillExists(uint owner, ushort id)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbSkill>()
                    .Add(Restrictions.Eq("Owner", owner))
                    .Add(Restrictions.Eq("ID", id))
                    .List<DbSkill>().Count > 0;
            }
        }
    }
}
