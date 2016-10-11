using Redux.Enum;

namespace Redux.Database.Domain
{
    public class DbDropRule
    {
        public virtual uint UID { get; set; }
        public virtual uint MonsterID { get; set; }
        public virtual DropRuleType RuleType { get; set; }
        public virtual double RuleChance { get; set; }
        public virtual int RuleAmount { get; set; }
        public virtual uint RuleValue { get; set; }
    }
}

