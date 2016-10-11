using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;
using Redux.Game_Server;
using Redux.Packets.Game;
using Redux.Structures;

namespace Redux.Managers
{
    /// <summary>
    /// Tracks and manages all sanity checks for adding or removing items from active player equipment.
    /// </summary>
    public class EquipmentManager
    {
        #region Constructor
        public EquipmentManager(Player _owner)
        {
            Owner = _owner;
            equippedItems = new ConquerItem[10];
            superGems = new List<int>();
        }
        #endregion

        #region Variables
        public Player Owner { get; private set; }
        private ConquerItem[] equippedItems;
        private List<int> superGems;

        public List<int> SuperGems { get { return superGems; } }
        #endregion

        #region Get Default Item Location
        public byte GetDefaultItemLocation(ushort _type)
        {
            byte loc = 0;
            switch (_type)
            {
                case 111:
                case 113:
                case 114:
                case 117:
                case 118:
                    loc = 1;
                    break;
                case 120:
                case 121:
                    loc = 2;
                    break;
                case 130:
                case 131:
                case 132:
                case 133:
                case 134:
                    loc = 3;
                    break;
                case 410:
                case 420:
                case 421:
                case 430:
                case 440:
                case 450:
                case 460:
                case 480:
                case 481:
                case 490:
                case 500:
                case 510:
                case 530:
                case 540:
                case 560:
                case 561:
                case 562:
                case 580:
                    loc = 4;
                    break;
                case 1050:
                    if (equippedItems[4] != null &&
                        equippedItems[4].EquipmentType == 500)
                        loc = 5;
                    break;
                case 900:
                    loc = 5;
                    break;
                case 150:
                case 151:
                case 152:
                    loc = 6;
                    break;
                case 2100:
                    loc = 7;
                    break;
                case 160:
                    loc = 8;
                    break;
                case 181:
                case 182:
                case 191:
                    loc = 9;
                    break;
            }
            return loc;
        }
        #endregion

        #region Cool Status
        public bool IsSuperArmor()
        {
            return equippedItems[3] != null && equippedItems[3].EquipmentQuality == 9;
        }
        public bool IsFullSuper()
        {
            var superCount = 0;
            foreach (var item in equippedItems)
                if (item != null && item.EquipmentQuality == 9)
                    superCount++;
            return superCount >= 6;
        }
        #endregion

        #region Equip Item
        public bool EquipItem(ConquerItem item, byte location, bool updateClient = true)
        {

            //If we're equipping a 2h weapon we ALWAYS uneq the left hand item
            if (DropManager.TwoHandWeaponTypes.Contains(item.EquipmentType))
                if (equippedItems[5] != null)
                    UnequipItem(5);

            //If we're equipping to left hand then we need to be able to (based on class type and level)
            if (location == 5)
            {
                bool canEq = false;
                switch (Owner.ProfessionType)
                {
                    case ProfessionType.Trojan:
                        canEq = Owner.ProfessionLevel > 1;
                        break;
                    case ProfessionType.Archer:
                        canEq = true;
                        break;
                    case ProfessionType.Warrior:
                        canEq = Owner.ProfessionLevel > 1;
                        break;
                }
                if (!canEq)
                    return false;

                //Now that we know it's physically possible to equip to second weapon slot we need to check the type of item
                if (Owner.ProfessionType == ProfessionType.Warrior)
                    canEq = item.EquipmentType == 900;
                else if (Owner.ProfessionType == ProfessionType.Archer)
                    canEq = item.EquipmentType == 1050;

                if (!canEq)
                    return false;
            }
            else
            {
                byte toLoc = GetDefaultItemLocation(item.EquipmentType);
                if (toLoc != location)
                    location = toLoc;
            }
            //Check all iteminfo req's           
            bool success = (item.BaseItem.GenderReq == 0 || !Owner.IsMale) &&
                Owner.Level >= item.BaseItem.LevelReq &&
                (Owner.RebornCount > 0 || (Owner.Strength >= item.BaseItem.StrengthReq &&
                Owner.Agility >= item.BaseItem.AgilityReq &&
                Owner.Spirit >= item.BaseItem.SpiritReq &&
                CanEquipProfession(item)));
            if (!success)
                return false;     
      
            if (equippedItems[location] != null)
                UnequipItem(location);
            item.Location = (ItemLocation)location;
            item.Save();

            //Track equipped super gems
            if (item.Gem1 % 10 == 3 || item.Gem2 % 10 == 3)
                if (!superGems.Contains(item.Gem1 / 10))
                    superGems.Add(item.Gem1 / 10);

            equippedItems[location] = item;
            if (updateClient)
            {
                Owner.SpawnPacket = SpawnEntityPacket.Create(Owner);
                Owner.Send(ItemActionPacket.Create(item.UniqueID, (byte)item.Location, ItemAction.SetEquipItem));
                Owner.SendToScreen(Owner.SpawnPacket, true);
            }
            return true;
        }

        #region Profession Santiy Check
        public bool CanEquipProfession(ConquerItem _item)
        {
            if (_item.BaseItem.ProfessionReq == 0)
                return true;
            if (Owner.RebornCount > 0 && _item.BaseItem.LevelReq <= 70)
                return true;
            if (_item.BaseItem.ProfessionReq > 100)
                return Owner.ProfessionType == ProfessionType.Taoist ||
                    Owner.ProfessionType == ProfessionType.FireTaoist ||
                    Owner.ProfessionType == ProfessionType.WaterTaoist ||
                    Owner.ProfessionType == ProfessionType.TaoistAll;
            else
                return Owner.ProfessionType == (ProfessionType)(_item.BaseItem.ProfessionReq / 10) &&
                    Owner.ProfessionLevel >= _item.BaseItem.ProfessionReq % 10;
        }
        #endregion
        #endregion

        #region Unequip Item
        public void UnequipItem(byte location)
        {
            var item = equippedItems[location];
            equippedItems[location] = null;
            if (item != null && Owner.AddItem(item))
            {
                //Track equipped super gems
                if (item.Gem1 % 10 == 3 || item.Gem2 % 10 == 3)
                    if (superGems.Contains(item.Gem1 / 10))
                        superGems.Remove(item.Gem1 / 10);
                Owner.SpawnPacket = SpawnEntityPacket.Create(Owner);
                if (DropManager.TwoHandWeaponTypes.Contains(item.EquipmentType) && equippedItems[5] != null)
                    UnequipItem(5);
                Owner.Send(ItemActionPacket.Create(item.UniqueID, (byte)item.Location, ItemAction.UnequipItem));
                Owner.SendToScreen(Owner.SpawnPacket);
                item.Location = 0;
                item.Save();
            }
        }
        #endregion

        #region Get Item by Location
        public ConquerItem GetItemBySlot(ItemLocation location)
        {
            if ((int)location >= equippedItems.Length)
                return null;
            return equippedItems[(int)location];
        }

        public bool TryGetItemBySlot(byte location, out ConquerItem item)
        {
            item = equippedItems[location];
            return item != null;
        }
        #endregion

        #region Combat Stat Helper Methods
        #region Get Atack Range
        public int GetAttackRange()
        {
            if (equippedItems[4] != null)
                return equippedItems[4].BaseItem.AttackRange;
            return 1;
        }
        #endregion

        #region Get Attack Speed
        public int GetAttackSpeed()
        {
            if (equippedItems[4] != null)
                return equippedItems[4].BaseItem.AttackSpeed;
            return 800;
        }
        #endregion

        #region Get Base Weapon Type
        public ushort GetBaseWeaponType()
        {
            ushort type = 0;
            if (equippedItems[4] != null)
                type = equippedItems[4].EquipmentType;
            return type;
        }
        #endregion
        #endregion

        #region Get Enumerator
        public IEnumerable<ConquerItem> GetEnumerator()
        {
            return equippedItems.GetEnumerator() as IEnumerable<ConquerItem>;
        }
        #endregion
    }
}
