using Redux.Enum;
using System;
namespace Redux.Database.Domain
{
    public class DbCharacter
    {
        public virtual uint UID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Spouse { get; set; }
        public virtual uint Lookface { get; set; }
        public virtual ushort Hair { get; set; }
        public virtual byte Level { get; set; }
        public virtual uint Money { get; set; }
        public virtual uint WhMoney { get; set; }
        public virtual uint CP { get; set; }
        public virtual ulong Experience { get; set; }
        public virtual ushort Strength { get; set; }
        public virtual ushort Agility { get; set; }
        public virtual ushort Spirit { get; set; }
        public virtual ushort Vitality { get; set; }
        public virtual ushort ExtraStats { get; set; }
        public virtual ushort Life { get; set; }
        public virtual ushort Mana { get; set; }
        public virtual uint Map { get; set; }
        public virtual ushort X { get; set; }
        public virtual ushort Y { get; set; }
        public virtual short Pk { get; set; }
        public virtual byte Profession { get; set; }
        public virtual byte Profession1 { get; set; }
        public virtual byte Profession2 { get; set; }
        public virtual byte Profession3 { get; set; }
        public virtual uint QuizPoints { get; set; }
        public virtual uint VirtuePoints { get; set; }
        public virtual bool Online { get; set; }
        public virtual DateTime HeavenBlessExpires { get; set; }
        public virtual DateTime DoubleExpExpires { get; set; }
        public virtual uint TrainingTime { get; set; }
        public virtual DateTime OfflineTGEntered { get; set; }
        public virtual uint LuckyTimeRemaining { get; set; }
    }
}

