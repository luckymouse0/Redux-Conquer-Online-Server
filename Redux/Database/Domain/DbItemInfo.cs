using Redux.Enum;

namespace Redux.Database.Domain
{
    public class DbItemInfo
    {
        public virtual uint ID { get; set; }
        public virtual string Name { get; set; }
        public virtual ushort ProfessionReq { get; set; }
        public virtual byte ProficiencyReq { get; set; }
        public virtual byte LevelReq { get; set; }
        public virtual byte GenderReq { get; set; }
        public virtual ushort StrengthReq { get; set; }
        public virtual ushort AgilityReq { get; set; }
        public virtual ushort VitalityReq { get; set; }
        public virtual ushort SpiritReq { get; set; }
        public virtual ItemFlags ItemFlags { get; set; }
        public virtual ushort Weight { get; set; }
        public virtual uint Price { get; set; }
        public virtual uint ActionID { get; set; }
        public virtual ushort AttackMax { get; set; }
        public virtual ushort AttackMin { get; set; }
        public virtual ushort DefenseAdd { get; set; }
        public virtual ushort AccuracyAdd { get; set; }
        public virtual byte DodgeAdd { get; set; }
        public virtual ushort HealthAdd { get; set; }
        public virtual ushort ManaAdd { get; set; }
        public virtual ushort Amount { get; set; }
        public virtual ushort AmountMax { get; set; }
        public virtual byte Ident { get; set; }
        public virtual byte Gem1 { get; set; }
        public virtual byte Gem2 { get; set; }
        public virtual byte Magic1 { get; set; }
        public virtual byte Magic2 { get; set; }
        public virtual byte Magic3 { get; set; }
        public virtual ushort MagicAttack { get; set; }
        public virtual ushort MagicDefense { get; set; }
        public virtual byte AttackRange { get; set; }
        public virtual ushort AttackSpeed { get; set; }
        public virtual byte FrayMode { get; set; }
        public virtual byte RepairMode { get; set; }
        public virtual byte TypeMask { get; set; }
        public virtual ushort PriceCP { get; set; }
        public virtual string TypeDesc { get; set; }
        public virtual string Description { get; set; }
    }
}

