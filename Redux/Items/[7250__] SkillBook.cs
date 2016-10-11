using Redux.Enum;
using Redux.Game_Server;
using Redux.Packets.Game;
using Redux.Structures;
using System;

namespace Redux.Items
{
    #region Mana
    #region Thunder
    /// <summary>
    /// Handles item usage for [725000] Thunder (SkillBook)
    /// </summary>
    public class Item_725000 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.Thunder))
            {
                if (client.Spirit >= 20)
                {
                    client.CombatManager.AddOrUpdateSkill(SkillID.Thunder, 0);
                    client.DeleteItem(item);
                }
                else
                    client.SendMessage("You need at least 20 spirit to learn Thunder", ChatType.Talk);
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #region Fire
    /// <summary>
    /// Handles item usage for [725001] Fire (SkillBook)
    /// </summary>
    public class Item_725001 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.Fire))
            {
                if (client.CombatManager.KnowsSkillLevel(SkillID.Thunder, 4))
                {
                    if (client.Level >= 40)
                    {
                        client.CombatManager.AddOrUpdateSkill(SkillID.Fire, 0);
                        client.DeleteItem(item);
                    }
                    else
                        client.SendMessage("You need to be at least level 40 to learn Fire.", ChatType.Talk);
                }
                else
                    client.SendMessage("Before learning Fire your Thunder must be at least level 40!", ChatType.Talk);
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #region Tornado
    /// <summary>
    /// Handles item usage for [725002] Tornado (SkillBook)
    /// </summary>
    public class Item_725002 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.Tornado))
            {
                if (client.CombatManager.KnowsSkillLevel(SkillID.Fire, 3))
                {
                    if (client.ProfessionType == ProfessionType.FireTaoist && client.Level >= 90)
                    {
                        client.CombatManager.AddOrUpdateSkill(SkillID.Tornado, 0);
                        client.DeleteItem(item);
                    }
                    else
                        client.SendMessage("Only Fire Taoist of at least level 90 may learn Tornado.", ChatType.Talk);
                }
                else
                    client.SendMessage("Before learning Tornado your Fire must be at least level 3!", ChatType.Talk);
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #region Cure
    /// <summary>
    /// Handles item usage for [725003] Tornado (SkillBook)
    /// </summary>
    public class Item_725003 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.Cure))
            {
                if (client.Spirit >= 30)
                {
                    client.CombatManager.AddOrUpdateSkill(SkillID.Cure, 0);
                    client.DeleteItem(item);
                }
                else
                    client.SendMessage("You need at least 30 spirit to learn Cure");
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #endregion

    #region XP
    #region FlyingMoon
    /// <summary>
    /// Handles item usage for [725025] FlyingMoon (SkillBook)
    /// </summary>
    public class Item_725025 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.FlyingMoon))
            {
                if (client.ProfessionType == ProfessionType.Warrior && client.Level >= 40)
                {
                    client.CombatManager.AddOrUpdateSkill(SkillID.FlyingMoon, 0);
                    client.DeleteItem(item);
                }
                else
                    client.SendMessage("Only Warriors of at least level 40 can learn FlyingMoon", ChatType.Talk);
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #region NightDevil
    /// <summary>
    /// Handles item usage for [725016] NightDevil (SkillBook)
    /// </summary>
    public class Item_725016 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.NightDevil))
            {
                if (client.Level >= 70)
                {
                    client.CombatManager.AddOrUpdateSkill(SkillID.NightDevil, 0);
                    client.DeleteItem(item);
                }
                client.SendMessage("You need to be at least level 70 to learn NightDevil.", ChatType.Talk);
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #region Lightning
    /// <summary>
    /// Handles item usage for [725004] Lightning (SkillBook)
    /// </summary>
    public class Item_725004 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.Lightning))
            {
                if (client.ProfessionType == ProfessionType.TaoistAll && client.Spirit >= 25)
                {
                    client.CombatManager.AddOrUpdateSkill(SkillID.Lightning, 0);
                    client.DeleteItem(item);
                }
                else
                    client.SendMessage("Only taoist with at least 25 spirit can learn Lightning", ChatType.Talk);
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #region SpeedLightning
    /// <summary>
    /// Handles item usage for [725028] SpeedLightning (SkillBook)
    /// </summary>
    public class Item_725028 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.SpeedLightning))
            {
                if (client.Level >= 70 && client.ProfessionType == ProfessionType.FireTaoist || client.ProfessionType == ProfessionType.WaterTaoist || client.ProfessionType == ProfessionType.TaoistAll)
                {
                    client.CombatManager.AddOrUpdateSkill(SkillID.SpeedLightning, 0);
                    client.DeleteItem(item);
                }
                else
                    client.SendMessage("Only taoist of at least level 70 can learn SpeedLightening", ChatType.Talk);
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #endregion

    #region Stamina
    #region DivineHare
    /// <summary>
    /// Handles item usage for [725015] DivineHare (SkillBook)
    /// </summary>
    public class Item_725015 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.DivineHare))
            {
                if (client.Level >= 54 && client.ProfessionType == ProfessionType.WaterTaoist)
                {
                    client.CombatManager.AddOrUpdateSkill(SkillID.DivineHare, 0);
                    client.DeleteItem(item);
                }
                else
                    client.SendMessage("Only Water Taoists of at least level 54 can learn DivineHare.", ChatType.Talk);
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #region FastBlade
    /// <summary>
    /// Handles item usage for [725005] FastBlade (SkillBook)
    /// </summary>
    public class Item_725005 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.FastBlade))
            {
                if (client.CombatManager.CheckProficiency(410, 5))
                {
                    client.CombatManager.AddOrUpdateSkill(SkillID.FastBlade, 0);
                    client.DeleteItem(item);
                }
                else
                    client.SendMessage("Your blade proficiency needs to be at least level 5.", ChatType.Talk);
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #region ScentSword
    /// <summary>
    /// Handles item usage for [725010] ScentSword (SkillBook)
    /// </summary>
    public class Item_725010 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.ScentSword))
            {
                if (client.CombatManager.CheckProficiency(420, 5))
                {
                    client.CombatManager.AddOrUpdateSkill(SkillID.ScentSword, 0);
                    client.DeleteItem(item);
                }
                else
                    client.SendMessage("Your sword proficiency needs to be at least level 5.", ChatType.Talk);
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #endregion

    #region Weapon
    #region WideStrike
    /// <summary>
    /// Handles item usage for [725011] WideStrike (SkillBook)
    /// </summary>
    public class Item_725011 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.WideStrike))
            {
                client.CombatManager.AddOrUpdateSkill(SkillID.WideStrike, 0);
                client.DeleteItem(item);
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #region SpeedGun
    /// <summary>
    /// Handles item usage for [725012] SpeedGun (SkillBook)
    /// </summary>
    public class Item_725012 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.SpeedGun))
            {
                client.CombatManager.AddOrUpdateSkill(SkillID.SpeedGun, 0);
                client.DeleteItem(item);
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #region Penetration
    /// <summary>
    /// Handles item usage for [725013] Penetration (SkillBook)
    /// </summary>
    public class Item_725013 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.Penetration))
            {
                client.CombatManager.AddOrUpdateSkill(SkillID.Penetration, 0);
                client.DeleteItem(item);
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #region Halt
    /// <summary>
    /// Handles item usage for [725014] Halt (SkillBook)
    /// </summary>
    public class Item_725014 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.Halt))
            {
                client.CombatManager.AddOrUpdateSkill(SkillID.Halt, 0);
                client.DeleteItem(item);
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #region Snow
    /// <summary>
    /// Handles item usage for [725026] Snow (SkillBook)
    /// </summary>
    public class Item_725026 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.Snow))
            {
                client.CombatManager.AddOrUpdateSkill(SkillID.Snow, 0);
                client.DeleteItem(item);
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #region StrandedMonster
    /// <summary>
    /// Handles item usage for [725027] StrandedMonster (SkillBook)
    /// </summary>
    public class Item_725027 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.StrandedMonster))
            {
                client.CombatManager.AddOrUpdateSkill(SkillID.StrandedMonster, 0);
                client.DeleteItem(item);
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #region Phoenix
    /// <summary>
    /// Handles item usage for [725029] Phoenix (SkillBook)
    /// </summary>
    public class Item_725029 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.Phoenix))
            {
                client.CombatManager.AddOrUpdateSkill(SkillID.Phoenix, 0);
                client.DeleteItem(item);
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #region Boom
    /// <summary>
    /// Handles item usage for [725030] Boom (SkillBook)
    /// </summary>
    public class Item_725030 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.Boom))
            {
                client.CombatManager.AddOrUpdateSkill(SkillID.Boom, 0);
                client.DeleteItem(item);
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #region Boreas
    /// <summary>
    /// Handles item usage for [725031] Boreas (SkillBook)
    /// </summary>
    public class Item_725031 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.Boreas))
            {
                client.CombatManager.AddOrUpdateSkill(SkillID.Boreas, 0);
                client.DeleteItem(item);
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #region Seizer
    /// <summary>
    /// Handles item usage for [725040] Seizer (SkillBook)
    /// </summary>
    public class Item_725040 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.Seizer))
            {
                client.CombatManager.AddOrUpdateSkill(SkillID.Seizer, 0);
                client.DeleteItem(item);
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #region Earthquake
    /// <summary>
    /// Handles item usage for [725041] Earthquake (SkillBook)
    /// </summary>
    public class Item_725041 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.Earthquake))
            {
                client.CombatManager.AddOrUpdateSkill(SkillID.Earthquake, 0);
                client.DeleteItem(item);
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #region Rage
    /// <summary>
    /// Handles item usage for [725042] Rage (SkillBook)
    /// </summary>
    public class Item_725042 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.Rage))
            {
                client.CombatManager.AddOrUpdateSkill(SkillID.Rage, 0);
                client.DeleteItem(item);
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #region Celestial
    /// <summary>
    /// Handles item usage for [725043] Celestial (SkillBook)
    /// </summary>
    public class Item_725043 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.Celestial))
            {
                client.CombatManager.AddOrUpdateSkill(SkillID.Celestial, 0);
                client.DeleteItem(item);
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #region Roamer
    /// <summary>
    /// Handles item usage for [725044] Roamer (SkillBook)
    /// </summary>
    public class Item_725044 : IItem
    {
        public override void Run(Player client, ConquerItem item)
        {
            if (!client.CombatManager.KnowsSkill(SkillID.Roamer))
            {
                client.CombatManager.AddOrUpdateSkill(SkillID.Roamer, 0);
                client.DeleteItem(item);
            }
            else
                client.SendMessage("You already know " + item.BaseItem.Name + "!", ChatType.Talk);
        }
    }
    #endregion
    #endregion

    #region Dances
    #endregion
}
