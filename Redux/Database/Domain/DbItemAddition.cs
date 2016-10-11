using Redux.Enum;

namespace Redux.Database.Domain
{
    public class DbItemAddition
    {
        public virtual uint Key { get; set; }
        public virtual uint BaseID { get; set; }
        public virtual byte Plus { get; set; }
        public virtual ushort Health { get; set; }
        public virtual ushort MaximumDamage { get; set; }
        public virtual ushort MinimumDamage {get;set;}
        public virtual ushort Defense {get;set;}
        public virtual ushort MagicDamage {get;set;}
        public virtual ushort MagicDefense { get; set; }
        public virtual ushort Accuracy { get; set; }
        public virtual byte Dodge {get;set;}
    }
}

