using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Database.Domain
{
    public class DbMonstertype
    {
        public virtual uint ID { get; set; }
        public virtual string Name { get; set; }
        public virtual uint Mesh { get; set; }
        public virtual ushort Life { get; set; }
        public virtual int AttackMax { get; set; }
        public virtual int AttackMin { get; set; }
        public virtual int AttackRange { get; set; }
        public virtual int ViewRange { get; set; }
        public virtual int Accuracy { get; set; }
        public virtual int Defence { get; set; }
        public virtual int Dodge { get; set; }
        public virtual ushort SkillType { get; set; }
        public virtual int SkillAccuracy { get; set; }
        public virtual int MagicDefence { get; set; }
        public virtual int AttackSpeed { get; set; }
        public virtual int MoveSpeed { get; set; }
        public virtual byte Level { get; set; }
        public virtual byte AttackMode { get; set; }
        public virtual byte AIType { get; set; }
        public virtual int SizeAdd { get; set; }
        public virtual int DropHelmet { get; set; }
        public virtual int DropNecklace { get; set; }
        public virtual int DropArmor { get; set; }
        public virtual int DropRing { get; set; }
        public virtual int DropWeapon { get; set; }
        public virtual int DropShield { get; set; }
        public virtual int DropBoots { get; set; }
        public virtual uint DropHP { get; set; }
        public virtual uint DropMP { get; set; }
        public virtual int BonusExp { get; set; }
    }
}
