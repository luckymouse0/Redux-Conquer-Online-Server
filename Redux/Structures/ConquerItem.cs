using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;
using Redux.Database;
using Redux.Packets.Game;
using Redux.Database.Domain;
using Redux.Game_Server;
namespace Redux.Structures
{
    public class ConquerItem
    {
        #region Constructor
        public ConquerItem(DbItem _item)
        {
            DbItem = _item;
            UniqueID = _item.UniqueID;
            StaticID = _item.StaticID;
            Durability = _item.Durability;
            Location = _item.Location;
            Color = _item.Color;
            Plus = _item.Plus;
            Bless = _item.Bless;
            Enchant = _item.Enchant;
            Gem1 = _item.Gem1;
            Gem2 = _item.Gem2;
            Effect = _item.Effect;
            Locked = _item.Locked;
        }
        public ConquerItem(uint _uniqueID, DbItemInfo _info)
        {
            UniqueID = _uniqueID;
            BaseItem = _info;
            
            staticID = BaseItem.ID;
            Durability = BaseItem.AmountMax;
            Gem1 = _info.Gem1;
            Gem2 = _info.Gem2;
            Effect = _info.Magic1;
            plus = _info.Magic3;
            Bless = Enchant  = 0;
            Locked = false;
            if ((EquipmentSort == 1 || EquipmentSort == 3 || EquipmentSort == 4) && BaseItem.TypeDesc != "Earring")
                Color = (byte)Common.Random.Next(3, 7);
            else
                Color = 3;
        }
        public ConquerItem(uint _uniqueID, uint _staticID, byte _plus = 0, byte _bless = 0, byte _enchant = 0, byte _gem1 = 0, byte _gem2 = 0, bool _locked = false, byte _effect = 0)
        {
        	UniqueID =_uniqueID;
        	StaticID = _staticID;
			Durability = BaseItem.AmountMax;
        	Plus = _plus;
        	Bless = _bless;
        	Enchant = _enchant;
        	Gem1 = _gem1;
        	Gem2 = _gem2;
        	Locked = _locked;
            Effect = _effect;
            if ((EquipmentSort == 1 || EquipmentSort == 3 || EquipmentSort == 4) && BaseItem.TypeDesc != "Earring")
                Color = (byte)Common.Random.Next(3, 7);
            else
                Color = 3;
        }
        #endregion

        #region Item Variables
        public uint UniqueID;
        private uint staticID;
        public byte Color, Bless, Enchant, Gem1, Gem2, Effect, plus = 0;
        public bool Locked;
        public ItemLocation Location;
        public ushort Durability;

        public ushort MaximumDurability { get { return BaseItem.AmountMax; } }
        public DbItem DbItem { get; private set; }
        public DbItemInfo BaseItem { get; private set; }
        public DbItemAddition ItemAddition { get; private set; }

        public override string ToString()
        {
            return BaseItem.Name;
        }
        public void ChangeItemID(uint _id)
        {
            StaticID = _id;
            Durability = MaximumDurability;
            Save();
        }

        public uint StaticID
        {
            get { return staticID; }
            private set
            {
                staticID = value;
                BaseItem = ServerDatabase.Context.ItemInformation.GetById(staticID);
                ItemAddition = ServerDatabase.Context.ItemAddition.GetByItem(this);
            }
        }

        public byte Plus
        {
            get { return plus; }
            set
            {
                plus = value;
                ItemAddition = ServerDatabase.Context.ItemAddition.GetByItem(this);
            }
        }

        #region Helpers (item sort, level, next level, etc)

        public bool IsDropable
        {
            get { return BaseItem != null && !BaseItem.ItemFlags.HasFlag(ItemFlags.DropHint); }
        }
        public bool IsSellable
        {
            get { return BaseItem != null && !BaseItem.ItemFlags.HasFlag(ItemFlags.SellDisable); }
        }
        public bool IsTradeable
        {
            get { return BaseItem != null && !BaseItem.ItemFlags.HasFlag(ItemFlags.TradeDisable); }
        }
        public bool IsStoreable
        {
            get { return BaseItem != null && !BaseItem.ItemFlags.HasFlag(ItemFlags.StorageDisable); }
        }

        static List<ushort> equipmentTypes = new List<ushort>() { 111, 113, 114, 117, 118, 120, 121, 150, 151, 152, 410, 420, 421, 430, 440, 450, 460, 
            480, 490, 500, 510, 530, 540, 560, 561, 580, 130, 131, 132, 133, 134, 160, 900 };
        public uint GroundID
        {
            get
            {
                return equipmentTypes.Contains(EquipmentType) ? (staticID / 10 * 10 + 5) : staticID;
            }
        }

        public ushort EquipmentType
        {
            get
            {
                return (ushort)(staticID / 1000);
            }
        }
        
        public byte EquipmentSort
        {
            get {return (byte)(staticID % 100000 / 10000);}
            
        }

        public ushort EquipmentLevel
        {
            get { return (ushort)((staticID % 1000) / 10); }
        }

        public byte EquipmentQuality
        {
            get { return (byte)(staticID % 10); }
        }

        public string EquipmentQualityString
        {
            get
            {
                switch (EquipmentQuality)
                {
                    case 9:
                        return "Super";
                    case 8:
                        return "Elite";
                    case 7:
                        return "Unique";
                    case 6:
                        return "Refined";
                    default:
                        return "Normal";
                }
            }
        }

        public bool IsEquipment
        {
            get{return equipmentTypes.Contains(this.EquipmentType);}
        }

        public bool IsHelmet { get { return EquipmentSort == 1 || EquipmentSort == 4; } }
        public bool IsArmor { get { return EquipmentSort == 3; } }
        public bool IsShield { get { return EquipmentType == 900; } }

        #region Calculate Next Item ID (Dragon Ball Upgrade)
        public uint GetNextItemQuality()
        {
            if (EquipmentQuality < 3 || EquipmentQuality == 9)
                return staticID;
            var tempID = staticID;
            if (EquipmentQuality < 5)
                tempID = (tempID / 10) * 10 + 5;
            var newItem = ServerDatabase.Context.ItemInformation.GetById(tempID + 1);
            return newItem == null ? staticID : newItem.ID;
        }
        #endregion

        #region Calculate Chance to Upgrade Quality
        public int ChanceToUpgradeQuality()
        {
            var chance = 100;
            if (EquipmentQuality == 9 || EquipmentQuality < 3)
                return 0;
            var quality = Math.Max((byte)5, EquipmentQuality);
            switch (quality)
            {
                case 6: chance = 50; break;
                case 7: chance = 33; break;
                case 8: chance = 20; break;
                default: chance = 100; break;
            }

            var lvl = BaseItem.LevelReq;
            if (lvl > 70)
                chance = chance * (100 - (lvl - 70)) / 100;

            return Math.Max(1, chance);
        }
        #endregion

        #region Calculate Next Item ID (Meteor Upgrade)
        public uint GetNextItemLevel()
        {
            //Quality too low
            if (EquipmentQuality < 3)
                return staticID;
            //Level too high
            switch (EquipmentLevel)
            {
                case 112:
                case 135:
                case 136:
                case 137:
                case 138:
                case 139:
                    return staticID;
            }
            DbItemInfo newItem = null;
            uint loops = 1;
            while (loops < 10 && newItem == null)
            {
                newItem = ServerDatabase.Context.ItemInformation.GetById(staticID + 10 * loops);
                loops++;
            }
            if (newItem == null || newItem.ID / 1000 != EquipmentType)
                return staticID;
            return newItem.ID;
        }
        #endregion

        #region Get DB Item By StaticID
        public DbItemInfo GetDBItemByStaticID(uint id)
        {
            return ServerDatabase.Context.ItemInformation.GetById(id);
        }
        #endregion

        #region Calculate Chance to Upgrade Level
        public int ChanceToUpgradeLevel()
        {
            int chance = 100;
            var newID = GetNextItemLevel();
            if (newID == staticID)
                return 0;
            if (IsShield || IsArmor || IsHelmet)
            {
                switch (EquipmentLevel)
                {
                    case 5: chance = 50; break;
                    case 6: chance = 40; break;
                    case 7: chance = 30; break;
                    case 8:
                    case 9: chance = 20; break;
                    default: chance = 500; break;
                }
                switch (EquipmentQuality)
                {
                    case 6: chance = Common.MulDiv(chance, 90, 100); break;
                    case 7: chance = Common.MulDiv(chance, 70, 100); break;
                    case 8: chance = Common.MulDiv(chance, 30, 100); break;
                    case 9: chance = Common.MulDiv(chance, 10, 100); break;
                }
            }
            else
            {
                switch (EquipmentLevel)
                {
                    case 11: chance = 95; break;
                    case 12: chance = 90; break;
                    case 13: chance = 85; break;
                    case 14: chance = 80; break;
                    case 15: chance = 75; break;
                    case 16: chance = 70; break;
                    case 17: chance = 65; break;
                    case 18: chance = 60; break;
                    case 19: chance = 55; break;
                    case 20: chance = 50; break;
                    case 21: chance = 45; break;
                    case 22: chance = 40; break;
                    default: chance = 500; break;
                } 
                switch (EquipmentQuality)
                {
                    case 6: chance = Common.MulDiv(chance, 90, 100); break;
                    case 7: chance = Common.MulDiv(chance, 70, 100); break;
                    case 8: chance = Common.MulDiv(chance, 30, 100); break;
                    case 9: chance = Common.MulDiv(chance, 10, 100); break;
                }

            } 
            chance = Math.Min(100, Math.Max(2, chance));
            return chance;

        }
        #endregion

        #region Down Level Item
        public void DownLevelItem()
        {
            if (!equipmentTypes.Contains(EquipmentType))
                return;
            var newLvl = 0;
            switch (EquipmentType)
            {
                case 111:
                case 113:
                case 114:
                case 117:
                case 118:
                case 120:
                case 121:
                case 130:
                case 131:
                case 133:
                case 134:
                    newLvl = 0;
                    break;
                case 150:
                case 151:
                case 152:
                case 500:
                case 160:
                    newLvl = 1;
                    break;
                case 410:
                case 420:
                case 421:
                case 430:
                case 440:
                case 450:
                case 460:
                case 480:
                case 490:
                case 510:
                case 530:
                case 540:
                case 560:
                case 561:
                    newLvl = 2;
                    break;
            }
            var newID = (uint)(staticID / 1000 * 1000 + newLvl * 10 + staticID % 10);
            if (ServerDatabase.Context.ItemInformation.GetById(newID) != null)
                ChangeItemID(newID);
            else
                Console.WriteLine("ERROR: Could not down level item {0} with owner {1}", staticID, DbItem.Owner);
        }
        #endregion

        #endregion

        #endregion

        #region Database Functions
        #region Set Owner (in database)
        public void SetOwner(Player _client)
        {
            DbItem = ServerDatabase.Context.Items.CreateNewItem(_client.UID, this);
        }
        #endregion

        #region Save To Database
        public void Save()
        {
            if (DbItem != null)
            {
                DbItem.UniqueID = UniqueID;
                DbItem.StaticID = staticID;
                DbItem.Durability = Durability;
                DbItem.Location = Location;
                DbItem.Plus = plus;
                DbItem.Bless = Bless;
                DbItem.Enchant = Enchant;
                DbItem.Gem1 = Gem1;
                DbItem.Gem2 = Gem2;
                DbItem.Effect = Effect;
                DbItem.Locked = Locked;
                DbItem.Color = Color;
                ServerDatabase.Context.Items.AddOrUpdate(DbItem);
            }
        }
        #endregion

        #region Delete From Database
        public void Delete()
        {
            if (DbItem != null)
                Database.ServerDatabase.Context.Items.Remove(DbItem);
        }
        #endregion
        #endregion
    }
}
