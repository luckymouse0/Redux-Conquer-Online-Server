using Redux.Enum;

namespace Redux.Database.Domain
{
    public class DbMagicType
    {
        public virtual uint UID { get; set; }
        public virtual ushort ID { get; set; }
        public virtual SkillSort Type { get; set; }
        public virtual string Name { get; set; }
        public virtual bool Crime { get; set; }
        public virtual bool Ground { get; set; }
        public virtual bool Multi { get; set; }
        public virtual TargetType Target {get;set;}
        public virtual ushort Level { get; set; }
        public virtual ushort UseMP { get; set; }
        public virtual int Power { get; set; }
        public virtual ushort IntoneSpeed { get; set; }
        public virtual byte Percent { get; set; }
        public virtual uint StepSecs { get; set; }
        public virtual byte Range { get; set; }
        public virtual byte Distance { get; set; }
        public virtual int Status {get;set;}
        public virtual uint NeedProf { get; set; }
        public virtual uint NeedExp { get; set; }
        public virtual byte NeedLevel { get; set; }
        public virtual byte UseXP { get; set; }
        public virtual ushort WeaponSubtype { get; set; }
        public virtual ushort ActiveTimes { get; set; }
        public virtual byte AutoActive { get; set; }
        public virtual bool FloorAttr { get; set; }
        public virtual bool AutoLearn {get;set;}
        public virtual byte LearnLevel { get; set; }
        public virtual bool DropWeapon { get; set; }
        public virtual byte UseStamina { get; set; }
        public virtual bool WeaponHit { get; set; }
        public virtual byte UseItem { get; set; }
        public virtual ushort NextMagic { get; set; }
        public virtual int DelayMS { get; set; }
        public virtual byte UseItemNum { get; set; }

    }
}