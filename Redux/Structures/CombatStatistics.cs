using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Database.Domain;
using Redux.Game_Server;
namespace Redux.Structures
{
    public struct CombatStatistics
    {
        #region Constructor
        public static CombatStatistics Create(DbMonstertype _monster)
        {
            var stat = new CombatStatistics();
            stat.MaxLife = _monster.Life;
            stat.MinimumDamage = _monster.AttackMin;
            stat.MaximumDamage = _monster.AttackMax;
            stat.Defense = _monster.Defence;
            stat.MagicDamage = _monster.AttackMax;
            stat.MagicResistance = _monster.MagicDefence;
            stat.AttackSpeed = _monster.AttackSpeed;
            stat.AttackRange = _monster.AttackRange;
            stat.Accuracy = _monster.Accuracy;
            stat.Dodge = _monster.Dodge;
            stat.BonusAttackPct = stat.BonusDefensePct = stat.BonusDodgePct = stat.BonusHitratePct = 100;
            return stat;
        }
        #endregion

        #region Variables
        public ushort MaxLife, MaxMana;
        public int MinimumDamage, MaximumDamage, Defense;
        public int MagicDamage, MagicResistance, MagicDefense;
        public int AttackSpeed, AttackRange, Accuracy, Dodge;
        public int DragonGemPct, RainbowGemPct, PhoenixGemPct, FuryGemPct, VioletGemPct, MoonGemPct, KylinGemPct, TortGemPct;
        public int BonusAttackPct, BonusDefensePct, BonusHitratePct, BonusDodgePct;
        public int BlessPct;
        #endregion

        #region Manage Client Status
        public void AddClientStatusStats(Structures.ClientStatus _status)
        {
            switch (_status.Type)
            {
                case Enum.ClientStatus.BonusAttack:
                    BonusAttackPct = _status.Value;
                    break;
                case Enum.ClientStatus.BonusDefense:
                    BonusDefensePct = _status.Value;
                    break;
                case Enum.ClientStatus.BonusHitrate:
                    BonusHitratePct = _status.Value;
                    break;
                case Enum.ClientStatus.BonusDodge:
                    BonusDodgePct = _status.Value;
                    break;
                case Enum.ClientStatus.Intensify:
                    BonusAttackPct += _status.Value;
                    break;
            }
        }

        public void RemoveClientStatusStats(Structures.ClientStatus _status)
        {
            switch (_status.Type)
            {
                case Enum.ClientStatus.BonusAttack:
                    BonusAttackPct = 100;
                    break;
                case Enum.ClientStatus.BonusDefense:
                    BonusDefensePct = 100;
                    break;
                case Enum.ClientStatus.BonusHitrate:
                    BonusHitratePct = 100;
                    break;
                case Enum.ClientStatus.BonusDodge:
                    BonusDodgePct = 100;
                    break;
                case Enum.ClientStatus.Intensify:
                    BonusAttackPct = 100;
                    break;
            }
        }
        #endregion

        #region Manage Item Stats
        public void AddItemStats(ConquerItem item)
        {
            if (item.BaseItem == null)
                return;
            if (item.Location != Enum.ItemLocation.WeaponL)
            {
                MinimumDamage += item.BaseItem.AttackMin;
                MaximumDamage += item.BaseItem.AttackMax;
            }
            else
            {
                MinimumDamage += item.BaseItem.AttackMin / 2;
                MaximumDamage += item.BaseItem.AttackMax / 2;
            }
            Defense += item.BaseItem.DefenseAdd;
            Accuracy += item.BaseItem.AccuracyAdd;
            Dodge += item.BaseItem.DodgeAdd;
            MaxLife += item.BaseItem.HealthAdd;
            MaxLife += item.Enchant;
            MaxMana += item.BaseItem.ManaAdd;
            MagicDamage += item.BaseItem.MagicAttack;
            MagicResistance += item.BaseItem.MagicDefense;
            if (item.Location == Enum.ItemLocation.WeaponR)
                AttackRange = item.BaseItem.AttackRange;
            AttackSpeed += item.BaseItem.AttackSpeed;

            BlessPct += item.Bless;

            AddGemByID(item.Gem1);
            AddGemByID(item.Gem2);
            if (item.ItemAddition == null)
                return;
            MaxLife += item.ItemAddition.Health; if (item.Location != Enum.ItemLocation.WeaponL)
            {
                MinimumDamage += item.ItemAddition.MinimumDamage;
                MaximumDamage += item.ItemAddition.MaximumDamage;
            }
            else
            {
                MinimumDamage += item.ItemAddition.MinimumDamage / 2;
                MaximumDamage += item.ItemAddition.MaximumDamage / 2;
            }
            Defense += item.ItemAddition.Defense;
            MagicDamage += item.ItemAddition.MagicDamage;
            MagicDefense += item.ItemAddition.MagicDefense;
            Accuracy += item.ItemAddition.Accuracy;
            Dodge += item.ItemAddition.Dodge;
        }
        #endregion

        #region Manage Gem Stats
        public void AddGemByID(byte gemID)
        {
            switch (gemID)
            {
                case 1: PhoenixGemPct += 5; break;
                case 2: PhoenixGemPct += 10; break;
                case 3: PhoenixGemPct += 15; break;

                case 11: DragonGemPct += 5; break;
                case 12: DragonGemPct += 10; break;
                case 13: DragonGemPct += 15; break;

                case 21: FuryGemPct += 5; break;
                case 22: FuryGemPct += 10; break;
                case 23: FuryGemPct += 15; break;

                case 31: RainbowGemPct += 10; break;
                case 32: RainbowGemPct += 15; break;
                case 33: RainbowGemPct += 25; break;

                case 41: KylinGemPct += 50; break;
                case 42: KylinGemPct += 100; break;
                case 43: KylinGemPct += 200; break;

                case 51: VioletGemPct += 30; break;
                case 52: VioletGemPct += 50; break;
                case 53: VioletGemPct += 100; break;

                case 61: MoonGemPct += 15; break;
                case 62: MoonGemPct += 30; break;
                case 63: MoonGemPct += 50; break;

                case 71: TortGemPct += 2; break;
                case 72: TortGemPct += 4; break;
                case 73: TortGemPct += 6; break;
    
            }
        }
        #endregion
    }
}
