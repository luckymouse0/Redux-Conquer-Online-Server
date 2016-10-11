using System.Collections;
using System.Collections.Generic;
using Redux.Database.Domain;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace Redux.Database.Repositories
{
    public class DropRuleRepository : Repository<uint, DbDropRule>
    {
        public IList<DbDropRule> GetRulesByMonsterType(uint _monsterType)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbDropRule>()
                    .Add(Restrictions.Eq("MonsterID", _monsterType))
                    .List<DbDropRule>();
            }
        }
    }
}

