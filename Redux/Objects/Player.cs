using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Net.Sockets;
using Redux.Database.Domain;
using Redux.Database;
using Redux.Space;
using Redux.Cryptography;
using Redux.Network;
using Redux.Managers;
using Redux.Enum;
using Redux.Packets.Game;
using Redux.Structures;

namespace Redux.Game_Server
{
    public class Player : Entity
    {
        #region Variables
        private bool disconnected = false;
        public NetworkClient Socket { get; private set; }
        private GameCryptography _cryptographer;
        private readonly ServerKeyExchange _exchange;
        public GameCryptography Crypto { get { return _cryptographer; } }
        public bool UseThreading { get; set; }
        private ConcurrentQueue<byte[]> ToSend;
        public DbCharacter Character;
        public DbAccount Account { get; set; }
        public ConcurrentDictionary<TaskType, Task> Tasks { get; private set; }
        public ConcurrentDictionary<uint, ConquerItem> Inventory { get; private set; }
        public CombatManager CombatManager;
        public AssociateManager AssociateManager;
        public WarehouseManager WarehouseManager;
        public TeamManager Team;

        public Structures.GuildAttr GuildAttribute { get; set; }
        public uint GuildId { get { return GuildAttribute.GuildId; } }
        public Structures.Guild Guild
        {
            get
            {
                return GuildId != 0 ? GuildManager.GetGuild(GuildId) : null;
            }
        }
        public uint MasterGuildId { get { return GuildAttribute.GuildId == 0 ? 0 : Guild.MasterGuildId; } }

        public GuildRank GuildRank { get { return GuildAttribute.GuildId == 0 ? GuildRank.None : GuildAttribute.Rank; } }
        public TradeSequence Trade = null;
        public PlayerShop Shop;

        public bool Mining = false;
        public uint MiningAttempts = 0;

        public long OfflineTrainingTime, LoginTime = DateTime.Now.Ticks, OnlineTraining = Common.Clock;
        public short OnlinePoints = 0;

        public Pet Pet;

        public uint KOCount = 0;

        public Npcs.INpc CurrentNPC;
        public long 
            LastSitAt, 
            LastPingReceived = Common.Clock, 
            LastXpUp = Common.Clock, 
            LastPkPoint = Common.Clock,
            NextMine = Common.Clock, 
            LastStep = Common.Clock,
            LastCool = Common.Clock;
        private ushort transformation, face, body = 0;
        private byte stamina;
        private byte xp = 0;
        private ApplyType _apply;
        private uint _applyTargetId;
        public string NpcInputBox = "";

        #region Nobility
        public DbNobility NobilityRecord;
        public int NobilityRank
        {
            get
            {
                if (Donation == 0)
                    return -1;
                else
                    if ((int)ServerDatabase.Context.Nobility.GetNobilityRank(NobilityRecord.Donation) > 50)
                        return -1;
                    else
                        return NobilityManager.RankingList.IndexOf(UID);
            }
        }
        public long Donation
        {
            get { return NobilityRecord.Donation; }
            set
            {
                int oldrank = NobilityRank;
                NobilityRecord.Donation = value;
                ServerDatabase.Context.Nobility.AddOrUpdate(NobilityRecord);
                NobilityManager.UpdateNobility();
                SpawnPacket.Nobility = (byte)NobilityMedal;
                SpawnPacket.NobilityRank = (uint)NobilityRank;

                if (oldrank != NobilityRank)
                    NobilityManager.UpdatePlayers();
            }
        }

        public NobilityRank NobilityMedal
        {
            get
            {
                if (NobilityRank >= 0 && NobilityRank <= 2)
                    return Enum.NobilityRank.King;
                else if (NobilityRank >= 3 && NobilityRank <= 14)
                    return Enum.NobilityRank.Prince;
                else if (NobilityRank >= 15 && NobilityRank <= 49)
                    return Enum.NobilityRank.Duke;
                else if (Donation >= 200000000)
                    return Enum.NobilityRank.Earl;
                else if (Donation >= 100000000)
                    return Enum.NobilityRank.Baron;
                else if (Donation >= 30000000)
                    return Enum.NobilityRank.Knight;
                else
                    return Enum.NobilityRank.Serf;

            }
        }
        #endregion
        #endregion

        #region Accessors

        public uint VirtuePoints
        {
            get { return Character.VirtuePoints; }
            set { Character.VirtuePoints = value; }
        }

        public new ushort X
        {
            get { return (ushort)Location.X; }
            set
            {
                var loc = Location; loc.X = (int)value; Location = loc;
                if (Character != null && Map != null && !Map.MapInfo.Type.HasFlag(MapTypeFlags.RecordDisable))
                    Character.X = value;
                SpawnPacket.PositionX = value;
            }
        }

        public new ushort Y
        {
            get { return (ushort)Location.Y; }
            set
            {
                var loc = Location; loc.Y = (int)value; Location = loc;
                if (Character != null && Map != null && !Map.MapInfo.Type.HasFlag(MapTypeFlags.RecordDisable))
                    Character.Y = value;
                SpawnPacket.PositionY = value;
            }
        }

        public uint Lookface
        {
            get { return SpawnPacket.Lookface; }
            set
            {
                SpawnPacket.Lookface = value;
                Send(UpdatePacket.Create(UID, UpdateType.Lookface, SpawnPacket.Lookface));
                SendToScreen(SpawnPacket);
            }
        }

        #region Face Parts
        public ushort Transformation
        {
            get { return transformation; }
            set { transformation = value; Lookface = (uint)(transformation * 10000000 + face * 10000 + body); }
        }

        public ushort Face
        {
            get { return face; }
            set { face = value; Character.Lookface = (uint)(face * 10000 + body); Lookface = (uint)(transformation * 10000000 + face * 10000 + body); }
        }

        public ushort Body
        {
            get { return body; }
            set { body = value; Character.Lookface = (uint)(face * 10000 + body); Lookface = (uint)(transformation * 10000000 + face * 10000 + body); }
        }
        #endregion

        public override string Name
        {
            get
            {
                if (Character == null)
                    return "NONE";
                return Character.Name;
            }
            set { Character.Name = value; SpawnPacket.Names.SetString(0, Character.Name); }
        }

        public string Spouse
        {
            get { return Character.Spouse; }
            set { Character.Spouse = value; }
        }

        public override uint Life
        {
            get { return Character.Life; }
            set
            {
                Character.Life = (ushort)Math.Min(CombatStats.MaxLife, value);
                base.Life = Character.Life;
                Send(UpdatePacket.Create(UID, UpdateType.Life, Character.Life));
            }
        }

        public override ushort Mana
        {
            get { return Character.Mana; }
            set
            {
                Character.Mana = Math.Min(CombatStats.MaxMana, value);
                base.Mana = Character.Mana;
                Send(UpdatePacket.Create(UID, UpdateType.Mana, Character.Mana));
            }
        }

        public override byte Stamina
        {
            get { return stamina; }
            set
            {
                stamina = value; Send(new UpdatePacket(UID, UpdateType.Stamina, stamina));
            }
        }

        public override byte Xp
        {
            get { return xp; }
            set { xp = value; }
        }

        public override int AttackRange
        {
            get
            {
                return Equipment.GetAttackRange();
            }
        }

        public override int AttackSpeed
        {
            get
            {
                return Equipment.GetAttackSpeed();
            }
        }

        public override ushort WeaponType
        {
            get
            {
                return Equipment.GetBaseWeaponType();
            }
        }

        public ushort Hair
        {
            get { return Character.Hair; }
            set
            {
                Character.Hair = value;
                SpawnPacket.Hair = value;
                SendToScreen(UpdatePacket.Create(UID, UpdateType.Hair, value), true);
            }
        }
        public byte HairColour
        {
            get { return (byte)(Hair / 100); }
            set { Hair = (ushort)((value * 100) + (Hair % 100)); }
        }
        public override byte Level { get { return Character.Level; } set { base.Level = value; Character.Level = value; } }
        public uint Money { get { return Character.Money; } set { Character.Money = value; Send(UpdatePacket.Create(UID, UpdateType.Money, Character.Money)); } }
        public uint WhMoney { get { return Character.WhMoney; } set { Character.WhMoney = value; Send(UpdatePacket.Create(UID, UpdateType.MoneySaved, Character.WhMoney)); } }
        public uint CP { get { return Character.CP; } set { Character.CP = value; Send(UpdatePacket.Create(UID, UpdateType.CP, Character.CP)); } }
        public ulong Experience { get { return Character.Experience; } set { Character.Experience = value; } }
        public ushort Strength { get { return Character.Strength; } set { Character.Strength = value; Send(UpdatePacket.Create(UID, UpdateType.Strength, Character.Strength)); } }
        public ushort Vitality { get { return Character.Vitality; } set { Character.Vitality = value; Send(UpdatePacket.Create(UID, UpdateType.Vitality, Character.Vitality)); } }
        public ushort Agility { get { return Character.Agility; } set { Character.Agility = value; Send(UpdatePacket.Create(UID, UpdateType.Agility, Character.Agility)); } }
        public ushort Spirit { get { return Character.Spirit; } set { Character.Spirit = value; Send(UpdatePacket.Create(UID, UpdateType.Spirit, Character.Spirit)); } }
        public ushort ExtraStats { get { return Character.ExtraStats; } set { Character.ExtraStats = value; Send(UpdatePacket.Create(UID, UpdateType.AdditionalPoint, Character.ExtraStats)); } }
        public byte Direction { get; set; }
        public ActionType Action { get { return SpawnPacket.Action; } set { SpawnPacket.Action = value; } }
        public byte RebornCount { get; set; }
        public short PK { get { return Character.Pk; } set { Character.Pk = value; } }
        #endregion

        #region Constructor
        public Player(NetworkClient client)
            : base()
        {
            Socket = client;
            ToSend = new ConcurrentQueue<byte[]>();
            _cryptographer = new GameCryptography(Common.ENCRYPTION_KEY);
            _exchange = new ServerKeyExchange();
            //GuildAttribute = new Structures.GuildAttr(this);
        }
        #endregion

        #region Functions

        #region Level Functions

        public ProfessionType ProfessionType { get { return (ProfessionType)((Character.Profession % 1000) / 10); } }
        public int ProfessionLevel { get { return Character.Profession % 10; } }

        #region Gain Experience
        public void GainExperience(ulong _exp)
        {
            if (Level >= 140)
                return;
            var bonus = Character.DoubleExpExpires > DateTime.Now ? 2.0 : 1.0;
            if (Character.HeavenBlessExpires > DateTime.Now)
                bonus += .2;
            bonus += CombatStats.RainbowGemPct / 100.0;
            Character.Experience += (ulong)((double)_exp * bonus) * Constants.EXP_RATE;
            var requires = ServerDatabase.Context.LevelExp.GetById(Character.Level);
            bool uplev = false;
            while (Character.Level < 130 && Character.Experience >= requires.Experience)
            {
                uplev = true;
                Character.Experience -= requires.Experience;
                Level++;
                if (RebornCount > 0)
                    ExtraStats += 3;
                requires = ServerDatabase.Context.LevelExp.GetById(Character.Level);
                if (Team != null && Team.Leader != this && Calculations.GetDistance(this, Team.Leader) < 25 && Team.Leader.Level >= Level + 20)
                {
                    uint toAdd = (uint)(Level * 20);
                    Team.Leader.VirtuePoints += toAdd;
                    Team.Leader.SendMessage("Congratulations you have earned " + toAdd + " VirtuePoints by leveling up newbies!", ChatType.Talk2);
                }
            }
            if (uplev)
                SetLevel(Level);
            Send(UpdatePacket.Create(UID, UpdateType.Experience, Experience));
        }
        #endregion

        #region Gain Exp Ball
        public void GainExpBall(double _time = 600)
        {
            if (Level >= 140)
                return;
            var requires = ServerDatabase.Context.LevelExp.GetById(Level);
            if (requires == null)
                return;

            var timeRemaining = (double)requires.UpLevTime * ((double)requires.Experience - (double)Experience) / (double)requires.Experience;

            if (_time >= timeRemaining)
            {
                Experience = 0;
                while (requires != null && _time >= timeRemaining)
                {
                    if (RebornCount > 0)
                        Character.ExtraStats += 3;
                    else
                        Level++;
                    _time -= timeRemaining;
                    requires = ServerDatabase.Context.LevelExp.GetById(Level);
                    timeRemaining = requires.UpLevTime;
                }
                SetLevel(Level);
            }

            if (requires != null && _time > 0)
                Experience += (ulong)((double)requires.Experience / (double)requires.UpLevTime * _time);

            Send(UpdatePacket.Create(UID, UpdateType.Experience, Experience));

        }
        #endregion

        #region Set Level
        public void SetLevel(int _level)
        {
            Level = (byte)_level;
            if (RebornCount == 0)
            {
                var stats = ServerDatabase.Context.Stats.GetByProfessionAndLevel((ushort)ProfessionType, (byte)(Level < 121 ? Level : 120));
                if (stats != null)
                {
                    Character.Strength = stats.Strength;
                    Character.Vitality = stats.Vitality;
                    Character.Agility = stats.Agility;
                    Character.Spirit = stats.Spirit;
                }
            }

            Send(UpdatePacket.Create(UID, UpdateType.Level, Level));
            Send(UpdatePacket.Create(UID, UpdateType.Strength, Strength));
            Send(UpdatePacket.Create(UID, UpdateType.Vitality, Vitality));
            Send(UpdatePacket.Create(UID, UpdateType.Agility, Agility));
            Send(UpdatePacket.Create(UID, UpdateType.Spirit, Spirit));
            Send(UpdatePacket.Create(UID, UpdateType.AdditionalPoint, ExtraStats));

            SendToScreen(GeneralActionPacket.Create(UID, DataAction.Leveled, 0, 0), true);
            Recalculate(true);
            Save();
        }
        #endregion
        #endregion

        #region Transformation Functions
        public bool IsMale { get { return Lookface % 10 > 2; } }

        #region Set Disguise
        public override void SetDisguise(Database.Domain.DbMonstertype _mob, long _duration)
        {
            if (_mob == null)
            {
                Transformation = 0;
                Recalculate();
                if (Life > MaximumLife)
                    Life = MaximumLife;
            }
            else
            {
                AddStatus(Enum.ClientStatus.TransformationTimeout, (int)_mob.Mesh, _duration);
                CombatStats = CombatStatistics.Create(_mob);
                Transformation = (ushort)_mob.Mesh;
                Send(UpdatePacket.Create(UID, UpdateType.MaxLife, MaximumLife));
                Life = MaximumLife;
            }
        }
        #endregion
        #endregion

        #region Kill
        public override void Kill(uint _dmg, uint _attacker)
        {
            //Drop items
            foreach (var item in Inventory.Values)
                if (!item.Locked && Common.PercentSuccess(5))
                {
                    if (!item.IsDropable)
                        DeleteItem(item);
                    else
                    {
                        var loc = Location;
                        for (var i = 0; i < 9; i++)
                            if (!Map.IsValidItemLocation(loc))
                            {
                                loc.X = Location.X + Common.DeltaX[i];
                                loc.Y = Location.Y + Common.DeltaY[i];
                            }
                        if (Map.IsValidItemLocation(loc))
                        {
                            var log = new DbItemLog(item.DbItem);
                            if (RemoveItem(item))
                            {
                                Database.ServerDatabase.Context.ItemLogs.Add(log);
                                item.Delete();
                                var gi = new GroundItem(item, (uint)Map.ItemCounter.Counter, loc, Map);
                                gi.AddToMap();
                                break;
                            }
                        }
                    }
                }
                
            //Redname drop equipment
            if (HasEffect(ClientEffect.Black) || HasEffect(ClientEffect.Red))
            {
                for (var i = ItemLocation.Helmet; i <= ItemLocation.Talisman; i++)
                {
                    ConquerItem item;
                    if (TryGetEquipmentByLocation(i, out item))
                    {
                        Equipment.UnequipItem((byte)i);
                        var loc = Location;
                        for (var z = 0; z < 9; z++)
                            if (!Map.IsValidItemLocation(loc))
                            {
                                loc.X = Location.X + Common.DeltaX[z];
                                loc.Y = Location.Y + Common.DeltaY[z];
                            }
                        if (Map.IsValidItemLocation(loc))
                        {
                            var log = new DbItemLog(item.DbItem);
                            if (RemoveItem(item))
                            {
                                Database.ServerDatabase.Context.ItemLogs.Add(log);
                                item.Delete();
                                var gi = new GroundItem(item, (uint)Map.ItemCounter.Counter, loc, Map);
                                gi.AddToMap();
                                break;
                            }
                        }
                    }
                }
                if(PK > 99)
                    ChangeMap(6000, 29, 72);
            }

            ///<summary>
            ///PK Point system.
            ///Written by Aceking 10-4-13
            ///</summary>
            //If attacker is a player
            if (PlayerManager.Players.ContainsKey(_attacker) || PetManager.ActivePets.ContainsKey(_attacker))
            {
                Player killer;
                if (PetManager.ActivePets.ContainsKey(_attacker))
                    killer = PetManager.ActivePets[_attacker].PetOwner;
                else
                    killer = PlayerManager.GetUser(_attacker);

                //If not already enemies
                if (!AssociateManager.Enemies.ContainsKey(_attacker))
                    AssociateManager.AddEnemy(killer);

                //Adds killers enemies of
                if (killer != null)
                {
                    if (!killer.AssociateManager.EnemyOf.ContainsKey(UID))
                        killer.AssociateManager.AddEnemyOf(this);
                }

                //If doesnt have blue or black name and map isnt free PK
                if (!HasEffect(ClientEffect.Blue) && !HasEffect(ClientEffect.Black) && Map.IsPKEnabled == true && !Map.IsFreePK && !Map.IsGuildMap)
                {

                    short PkPoints = 10;
                    if (HasEffect(ClientEffect.Red))
                    {
                        PkPoints = 3;
                    }
                    else if (killer.AssociateManager.Enemies.ContainsKey(this.UID))
                    {
                        PkPoints = 5; //5 PK for enemies
                    }
                    else if (killer.Guild != null && this.Guild != null && this.Guild.Id != 0 && Guild.IsEnemied(killer.Guild.Id))
                        PkPoints = 3; //3 PK for guild enemies

                    //If Pk Points = 0, set time to now else PK will  be subtracted immediately
                    if (killer.PK == 0)
                        killer.LastPkPoint = Common.Clock;
                    //Adds pk points
                    killer.PK += PkPoints;
                    killer.Send(Packets.Game.UpdatePacket.Create(killer.UID, UpdateType.Pk, (ulong)killer.PK));

                    //Adds red or black name
                    if (killer.PK >= 100)
                    {
                        if (killer.HasEffect(ClientEffect.Black))
                            killer.RemoveEffect(ClientEffect.Black);
                        killer.AddEffect(ClientEffect.Black, ((killer.PK - 99) * 6) * 60000, true); //Calculates how long until they reach 99 PK points, and uses that as a timer
                    }
                    else if (killer.PK >= 30)
                    {
                        if (killer.HasEffect(ClientEffect.Red))
                            killer.RemoveEffect(ClientEffect.Red);
                        killer.AddEffect(ClientEffect.Red, ((killer.PK - 29) * 6) * 60000, true); //Calculates how long until they reach 99 PK points, and uses that as a timer

                    }
                    killer.AddEffect(ClientEffect.Blue, 180000, true); //3 minutes blue name

                }
            }

            if (Pet != null)
            {
                Pet.RemovePet();
            }

            SpawnPacket.StatusEffects = ClientEffect.Dead;
            OnMove();
            AddEffect(ClientEffect.Ghost, 0);
            AddStatus(Enum.ClientStatus.ReviveTimeout, 0, 18 * Common.MS_PER_SECOND);



            base.Kill(_dmg, _attacker);
        }
        #endregion

        #region Revive
        public void Revive(bool _resetLocation = true)
        {
            //ONLY deal with updating statuses and healing. All spell effects should be handled in combat manager
            Life = MaximumLife;
            Stamina = 100;
            Xp = 0;
            LastXpUp = Common.Clock;

            RemoveEffect(ClientEffect.Dead);
            RemoveEffect(ClientEffect.Ghost);

            RemoveStatus(Enum.ClientStatus.ReviveTimeout);
            Transformation = 0;

            if (_resetLocation && Map.MapInfo.SpawnX != 0)//Send us to respawn location!            
                if (Map.ID == 1038 && GuildWar.Running == true)                
                    ChangeMap(6001, 29, 72);                
                else
                        ChangeMap(Map.MapInfo.SpawnID, Map.MapInfo.SpawnX, Map.MapInfo.SpawnY);
            else
                ChangeMap(MapID, X, Y);

            AddStatus(Enum.ClientStatus.ReviveProtection, 0, 5 * Common.MS_PER_SECOND);
        }
        #endregion

        #region Recalculate
        public void Recalculate(bool heal = false)
        {
            CombatStats = new Structures.CombatStatistics();
            CombatStats.AttackRange = 2;
            CombatStats.MinimumDamage = CombatStats.MaximumDamage = Strength;
            CombatStats.BonusAttackPct = CombatStats.BonusDefensePct = CombatStats.BonusDodgePct = CombatStats.BonusHitratePct = 100;
            //Max Health
            CombatStats.MaxLife = (ushort)(Constants.STAT_MAXLIFE_STR * Strength +
                              Constants.STAT_MAXLIFE_AGI * Agility +
                              Constants.STAT_MAXLIFE_VIT * Vitality +
                              Constants.STAT_MAXLIFE_SPI * Spirit);
            if (ProfessionType == Enum.ProfessionType.Trojan)
                CombatStats.MaxLife = (ushort)Common.MulDiv(CombatStats.MaxLife, Common.GetTrojanLifeBonus(ProfessionLevel), 100);

            //Max Mana
            CombatStats.MaxMana = (ushort)(Constants.STAT_MAXMANA_STR * Strength +
                              Constants.STAT_MAXMANA_AGI * Agility +
                              Constants.STAT_MAXMANA_VIT * Vitality +
                              Constants.STAT_MAXMANA_SPI * Spirit);
            if (ProfessionType == Enum.ProfessionType.WaterTaoist || ProfessionType == Enum.ProfessionType.FireTaoist)
                CombatStats.MaxMana = (ushort)Common.MulDiv(CombatStats.MaxMana, Common.GetTaoistManaBonus(ProfessionLevel), 100);


            //Calculate stats for each gear
            ConquerItem item;
            for (byte loc = 1; loc < 9; loc++)
                if (Equipment.TryGetItemBySlot(loc, out item))
                    CombatStats.AddItemStats(item);

            SendMessage(string.Format("Damage {0}-{1} Defense {2} Magic Resistance {3} Magic Defense {4} Maximum Health {5} Maximum Mana {6}", CombatStats.MinimumDamage, CombatStats.MaximumDamage,
                CombatStats.Defense, CombatStats.MagicResistance, CombatStats.MagicDamage, CombatStats.MaxLife, CombatStats.MaxMana));

            //Correct max hp stats in client.
            Send(UpdatePacket.Create(UID, UpdateType.MaxLife, MaximumLife));
            Send(UpdatePacket.Create(UID, UpdateType.MaxMana, MaximumMana));

            if (heal)
            {
                Life = CombatStats.MaxLife;
                Mana = CombatStats.MaxMana;
            }
        }
        #endregion

        #region Create DB Entry
        public void CreateDbCharacter(string name, ushort body, byte profession)
        {
            Character = new DbCharacter();
            Character.UID = UID;
            Character.Name = name;
            Character.Spouse = "None";
            #region Random
            ushort face = (ushort)Common.Random.Next(201, 250);
            if (body == 1003 || body == 1004)
                face = (ushort)Common.Random.Next(50);
            Character.Lookface = (uint)(body + (face * 10000));
            byte colour = (byte)Common.Random.Next(3, 9);
            Character.Hair = (ushort)((colour * 100) + Common.Random.Next(30, 51));
            #endregion
            Character.Level = 1;
            Character.Money = 1000;
            Character.CP = 0;
            Character.Experience = 0;
            Character.Map = 1002;
            Character.X = 438;
            Character.Y = 381;
            Character.Pk = 0;
            Character.Profession = profession;
            Character.Profession1 = 0;
            Character.Profession2 = 0;
            Character.Profession3 = 0;
            Character.QuizPoints = 0;
            Character.VirtuePoints = 0;
            Character.Online = false;
            Character.OfflineTGEntered = DateTime.MinValue;
            var stats = ServerDatabase.Context.Stats.GetByProfessionAndLevel((ushort)ProfessionType, (byte)(Level < 121 ? Level : 120));
            if (stats != null)
            {
                Character.Strength = stats.Strength;
                Character.Vitality = stats.Vitality;
                Character.Agility = stats.Agility;
                Character.Spirit = stats.Spirit;
            }
            Character.Life = (ushort)(Constants.STAT_MAXLIFE_STR * Strength +
                            Constants.STAT_MAXLIFE_AGI * Agility +
                            Constants.STAT_MAXLIFE_VIT * Vitality +
                            Constants.STAT_MAXLIFE_SPI * Spirit);

            Character.Mana = (ushort)(Constants.STAT_MAXMANA_STR * Strength +
                              Constants.STAT_MAXMANA_AGI * Agility +
                              Constants.STAT_MAXMANA_VIT * Vitality +
                              Constants.STAT_MAXMANA_SPI * Spirit);


            CreateDBItem(132005, ItemLocation.Armor);
            if (Character.Profession == 100)
            {
                CreateDBItem(421301, ItemLocation.WeaponR);
                for (var i = 0; i < 5; i++)
                    CreateDBItem(1001000);
                ConquerSkill.Create(Character.UID, 1000, 0).Save();
                ConquerSkill.Create(Character.UID, 1005, 0).Save();

            }
            else if (Character.Profession == 40)
            {
                CreateDBItem(500301, ItemLocation.WeaponR);
                for (var i = 0; i < 5; i++)
                    CreateDBItem(1050000);
            }
            else
                CreateDBItem(410301, ItemLocation.WeaponR);
            for (var i = 0; i < 5; i++)
                CreateDBItem(1000000);

            Database.ServerDatabase.Context.Characters.CreateEntry(Character);
        }
        
        #endregion

        #region Populate from DB
        public bool Populate(DbCharacter _character)
        {
            Character = _character;
            Character.Online = true;
            Save();
            RebornCount = 0;
            if (Character.Profession1 > 0)
            {
                RebornCount++;
                if (Character.Profession2 > 0)
                {
                    RebornCount++;
                    if (Character.Profession3 > 0)
                    {
                        RebornCount++;
                    }
                }
            }
            UID = Character.UID;

            face = (ushort)(Character.Lookface / 10000);
            body = (ushort)(Character.Lookface % 10000);
            Location = new Point(Character.X, Character.Y);
            AddStatus(Enum.ClientStatus.ReviveProtection, 0, 10 * Common.MS_PER_SECOND);
            if (!PlayerManager.AddPlayer(this)) return false;
            Console.WriteLine("{0} is logging in.", Character.Name);

            Send(Packets.Game.HeroInformationPacket.Create(this));
            Inventory = new ConcurrentDictionary<uint, ConquerItem>();

            Tasks = new ConcurrentDictionary<TaskType, Task>();
            foreach (var task in Database.ServerDatabase.Context.Tasks.GetTasksByPlayerUID(UID))
                if (!Tasks.ContainsKey(task.Type))
                    Tasks.TryAdd(task.Type, new Task(task));

            var guildinfo = ServerDatabase.Context.GuildAttributes.GetGuildId(UID);
            NobilityRecord = ServerDatabase.Context.Nobility.GetByUID(UID);
            if (NobilityRecord == null)
            {
                NobilityRecord = new DbNobility();
                NobilityRecord.Donation = 0;
                NobilityRecord.UID = UID;
            }
            WarehouseManager = new Managers.WarehouseManager(this);
            Equipment = new EquipmentManager(this);
            GuildAttribute = new Structures.GuildAttr(this);
            foreach (var item in Database.ServerDatabase.Context.Items.GetItemsByPlayer(UID))
            {
                var coItem = new ConquerItem(item);
                if (coItem.Location == 0)
                {
                    Inventory.TryAdd(item.UniqueID, coItem);
                    Send(ItemInformationPacket.Create(coItem));
                }
                else if (coItem.Location < ItemLocation.WAREHOUSE_START)
                {
                    if (!Equipment.EquipItem(coItem, (byte)coItem.Location, false))
                    {
                        coItem.Location = 0;
                        Inventory.TryAdd(item.UniqueID, coItem);
                    }
                    Send(ItemInformationPacket.Create(coItem));
                }
                else
                    WarehouseManager.LoadItem(coItem);
            }
            
            SpawnPacket = SpawnEntityPacket.Create(this);

            if (Character.OfflineTGEntered != DateTime.MinValue)
            {
                X = 60;
                Y = 54;
            }
            if (PK >= 100)
                AddEffect(ClientEffect.Black, ((PK - 99) * 6) * 60000, true);
            else if (PK >= 30)
                AddEffect(ClientEffect.Red, ((PK - 29) * 6) * 60000, true);
            Recalculate();

            MapManager.AddPlayer(this, (Character.OfflineTGEntered == DateTime.MinValue) ? Character.Map : 601);

            if (Life < 1)
                Revive();
            return true;
        }
        #endregion       

        #region Item Functions

        #region Equip Item
        public void HandleItemEquipPacket(ItemActionPacket packet)
        {
            if (!Inventory.ContainsKey(packet.UID))
                return;
            var item = Inventory[packet.UID];
            if (Equipment.GetDefaultItemLocation(item.EquipmentType) == 0)
                Items.Manager.ProcessItem(this, Inventory[packet.UID]);
            else if (Equipment.EquipItem(item, (byte)packet.ID) && RemoveItem(item))
            {
                if (CombatManager != null && CombatManager.IsActive)
                    CombatManager.AbortAttack(true);
                if (Mining)
                    StopMining();
                Recalculate();
            }
        }
        #endregion

        #region Unequip Item
        public void HandleItemUnequipPacket(ItemActionPacket packet)
        {
            Equipment.UnequipItem((byte)packet.ID);
            Recalculate();
        }
        #endregion

        #region HasItem by ID
        /// <summary>
        /// Check if we have enough of a given item ID in inventory
        /// </summary>
        /// <param name="_id">StaticID</param>
        /// <param name="_count">Amount Required</param>
        /// <returns></returns>
        public bool HasItem(uint _id, int _count = 1)
        {
            var found = 0;
            foreach (var item in Inventory.Values)
                if (item.StaticID == _id)
                {
                    found++;
                    if (found >= _count)
                        break;
                }
            return found >= _count;
        }
        #endregion

        #region Get Inventory Item By UID
        public ConquerItem GetItemByUID(uint uid)
        {
            ConquerItem item = null;
            if (Inventory.ContainsKey(uid))
                item = Inventory[uid];
            return item;
        }
        #endregion

        #region Get Inventory Item By ID
        public ConquerItem GetItemByID(uint _id)
        {
            ConquerItem item = null;
            foreach (var i in Inventory.Values)
                if (i.StaticID == _id)
                {
                    item = i;
                    break;
                }
            return item;
        }
        #endregion

        #region Try Get Equipment By Item Location
        public bool TryGetEquipmentByLocation(ItemLocation location, out ConquerItem item)
        {
            item = Equipment != null ? Equipment.GetItemBySlot(location) : null;
            return item != null;
        }
        #endregion

        #region Add Item To Inventory
        public bool AddItem(ConquerItem item)
        {
            return Inventory.Count < 40 && !Inventory.ContainsKey(item.UniqueID) && Inventory.TryAdd(item.UniqueID, item);
        }
        #endregion

        #region Delete Conquer Item (from inventory and database)
        /// <summary>
        /// Permanently delete a known item from the game (db+inventory removal)
        /// </summary>
        /// <param name="_item">Item to Remove</param>
        /// <param name="_updateClient">Optional update packet to client</param>
        /// <returns></returns>
        public bool DeleteItem(ConquerItem _item, bool _updateClient = true)
        {
            if (_item == null || !Inventory.ContainsKey(_item.UniqueID))
                return false;
            if (_updateClient)
                Send(ItemActionPacket.Create(_item.UniqueID, _item.StaticID, ItemAction.RemoveInventory));
            _item.Delete();
            return Inventory.TryRemove(_item.UniqueID, out _item);
        }
        #endregion

        #region Remove Conquer Item (from inventory)
        /// <summary>
        /// Remove a known item from the client inventory
        /// </summary>
        /// <param name="_item">Item to Remove</param>
        /// <param name="_updateClient">Optional update packet to client.</param>
        /// <returns></returns>
        public bool RemoveItem(ConquerItem _item, bool _updateClient = true)
        {
            if (_item == null || !Inventory.ContainsKey(_item.UniqueID))
                return false;
            if (_updateClient)
                Send(ItemActionPacket.Create(_item.UniqueID, _item.StaticID, ItemAction.RemoveInventory));
            return Inventory.TryRemove(_item.UniqueID, out _item);
        }
        #endregion

        #region Delete Item by Static ID
        public bool DeleteItem(uint _id, bool _updateClient = true)
        {
            return DeleteItem(GetItemByID(_id), _updateClient);
        }
        #endregion

        #region CreateDBItem (offline item add)
        public void CreateDBItem(uint _staticID, ItemLocation location = ItemLocation.Inventory, byte _plus = 0, byte _bless = 0, byte _enchant = 0, byte _gem1 = 0, byte _gem2 = 0, bool _locked = false, byte _effect = 0)
        {
            var item = new ConquerItem((uint)Common.ItemGenerator.Counter, _staticID, _plus, _bless, _enchant, _gem1, _gem2, _locked, _effect);
            item.Location = location;
            Database.ServerDatabase.Context.Items.CreateNewItem(UID, item);
        }
        #endregion
        #region Create Item (By User Input)
        public ConquerItem CreateItem(uint _staticID, byte _plus = 0, byte _bless = 0, byte _enchant = 0, byte _gem1 = 0, byte _gem2 = 0, bool _locked = false, byte _effect = 0)
        {
            var item = new ConquerItem((uint)Common.ItemGenerator.Counter, _staticID, _plus, _bless, _enchant, _gem1, _gem2, _locked, _effect);
            if (AddItem(item))
            {
                item.SetOwner(this);
                Send(ItemInformationPacket.Create(item));
            }
            return item;
        }
        #endregion

        #region Create Item (By Item Info)
        public ConquerItem CreateItem(DbItemInfo _itemInfo)
        {
            var item = new ConquerItem((uint)Common.ItemGenerator.Counter, _itemInfo);
            if (AddItem(item))
            {
                item.SetOwner(this);
                Send(ItemInformationPacket.Create(item));
            }
            return item;
        }
        #endregion

        #endregion

        #region Delayed Action Timeout
        public void On_Player_Timer()
        {
            if (Map == null)
                return;
            if (Character == null)
                return;
            if(CombatManager == null)
            {
                Console.WriteLine("ERROR: " + Name + " has a null combat manager. Re-creating");
                CombatManager = new CombatManager(this);
            }
            if (!Constants.DEBUG_MODE && Common.Clock - LastPingReceived > Common.MS_PER_SECOND * 45)
            { Disconnect(); Console.WriteLine("Connection timeout for {0} with {1} ms latency", Name, Common.Clock - LastPingReceived); return; }
            if (Alive)
            {
                if ((Character.HeavenBlessExpires > DateTime.Now && stamina < 150) || stamina < 100)
                {
                    byte toGain = 3;
                    if ((Action == ActionType.Sit || Action == ActionType.Lie) && Common.Clock - LastSitAt > Common.MS_PER_SECOND)
                        toGain = 11;
                    Stamina = (byte)Math.Min((Character.HeavenBlessExpires > DateTime.Now) ? 150 : 100, stamina + toGain);
                }
                if (!HasEffect(ClientEffect.XpStart) && Common.Clock - LastXpUp > Common.MS_PER_SECOND * 3)
                {
                    Xp = (byte)(Math.Min(100, Xp + 1));
                    if (xp == 100 && CombatManager.HasXPSkills)
                        AddEffect(ClientEffect.XpStart, 20000);
                    LastXpUp = Common.Clock;
                }
                if (PK > 0 && Common.Clock - LastPkPoint > Common.MS_PER_MINUTE * 6)//If last Pk point has been 6 minutes
                {
                    PK -= 1;//Minus 1 PK point
                    LastPkPoint = Common.Clock;//Set last reduction time to now
                    Send(UpdatePacket.Create(UID, UpdateType.Pk, (ulong)PK));

                    //If Between 30 and 99 and does not have Red Name.....then Add redname
                    if (PK >= 30 && PK < 100 && !HasEffect(ClientEffect.Red))
                    {
                        RemoveEffect(ClientEffect.Black);
                        AddEffect(ClientEffect.Red, ((PK - 29) * 6) * 60000, true);//Adds red name
                    }

                    //If under 30 PK, remove redname
                    if (PK < 30 && HasEffect(ClientEffect.Red))
                        RemoveEffect(ClientEffect.Red);
                }

                if (Mining && Common.Clock > NextMine && Map.MapInfo.Type.HasFlag(MapTypeFlags.MineEnable))
                {
                    if (Equipment.GetBaseWeaponType() != 562)
                        StopMining();
                    else
                    {

                        Send(GeneralActionPacket.Create(UID, DataAction.Mine, 0));
                        MiningAttempts += 1;
                        NextMine = Common.Clock + (Common.MS_PER_SECOND * 3);

                        if (Inventory.Count >= 40)
                        {
                            SendMessage("Your inventory is full. You can not mine anymore items.", ChatType.System);
                            StopMining();
                        }
                        else
                        {
                            foreach (var rule in Map.MineRules)
                                if (Common.PercentSuccess(rule.RuleChance))
                                {
                                    switch (rule.RuleType)
                                    {
                                        case DropRuleType.MineOre:
                                            {
                                                var quality = (uint)Common.Random.Next(10);
                                                var item = CreateItem(rule.RuleValue + quality);
                                                SendMessage("You have just mined a " + item.ToString(), ChatType.System);
                                                break;
                                            }
                                        case DropRuleType.MineGem:
                                            {
                                                var id = rule.RuleValue;
                                                if (Common.PercentSuccess(Constants.CHANCE_SUPER_GEM))
                                                    id += 2;
                                                else if (Common.PercentSuccess(Constants.CHANCE_REFINED_GEM))
                                                    id++;
                                                var gem = CreateItem(id);
                                                SendMessage("Congratulations! You just mined a " + gem.EquipmentQualityString + gem.ToString() + "!", ChatType.System);
                                                break;
                                            }
                                        case DropRuleType.MineMisc:
                                            {
                                                var item = CreateItem(rule.RuleValue);
                                                SendMessage("You just mined a " + item.ToString() + "!", ChatType.System);
                                                break;
                                            }
                                    }
                                    break;
                                }
                        }
                    }



                }

                if (Character.HeavenBlessExpires > DateTime.Now && Common.Clock > OnlineTraining && MapID != 601 && MapID != 1039)
                {
                    OnlinePoints += 1;
                    if (OnlinePoints == 10)
                    {
                        Send(UpdatePacket.Create(UID, UpdateType.OnlineTraining, 4));
                        GainExpBall(10);
                    }
                    else
                        Send(UpdatePacket.Create(UID, UpdateType.OnlineTraining, 3));

                    OnlineTraining = Common.Clock + Common.MS_PER_MINUTE;
                }

                #region Lucky Time
                //if we are casting difuse then include new players in our effect
                if (HasEffect(ClientEffect.LuckDiffuse))
                {
                    foreach (var player in Map.QueryBox<Player>(this, 3))
                    {
                        if (player == this)
                            continue;
                        if (!player.HasEffect(ClientEffect.LuckAbsorb) && !player.HasEffect(ClientEffect.LuckDiffuse))
                        {
                            player.AddEffect(ClientEffect.LuckAbsorb, -1);
                            player.Action = Action;
                        }
                    }
                }
                if (HasEffect(ClientEffect.LuckDiffuse))
                {
                    if (Character.LuckyTimeRemaining < 7200)
                    {
                        Character.LuckyTimeRemaining += 3;
                        Send(UpdatePacket.Create(UID, UpdateType.LuckyTime, Character.LuckyTimeRemaining * 1000));
                    }
                }
                else if (HasEffect(ClientEffect.LuckAbsorb))
                {
                    if (Character.LuckyTimeRemaining < 7200)
                    {
                        Character.LuckyTimeRemaining++;
                        Send(UpdatePacket.Create(UID, UpdateType.LuckyTime, Character.LuckyTimeRemaining * 1000));
                    }
                }
                else if (Character.LuckyTimeRemaining > 0)
                {
                    Character.LuckyTimeRemaining--;
                    Send(UpdatePacket.Create(UID, UpdateType.LuckyTime, Character.LuckyTimeRemaining * 1000));
                }
                #endregion
            }
        }
        #endregion

        #region Packet Queue Timeout
        public void On_Packet_Timer()
        {
            if (!Socket.Alive)
                return;
            if (ToSend.Count == 0)
                return;
            byte[] packet;
            if (ToSend.TryDequeue(out packet))
                DirectSend(packet);
        }
        #endregion

        #region Movement Functions

        #region Change Map
        public void ChangeMap(ushort _id)
        {
            var m = Database.ServerDatabase.Context.Maps.GetById((uint)_id);
            if (m != null)
                ChangeMap(m.SpawnID, m.SpawnX, m.SpawnY);
        }

        public void ChangeMap(ushort _id, ushort _x, ushort _y)
        {
            if (!Common.MapService.MapData.ContainsKey(_id))
            {
                Console.WriteLine("ERROR: No such map ID as {0}", _id);
                return;
            }
            if (!Common.MapService.Valid(_id, _x, _y))
            {
                Console.WriteLine("ERROR: Invalid coords {0}-{1} on map id {2}", _x, _y, _id);
                return;
            } 
            
            //Cancel mining/attacks/lucky time
            OnMove();

            if (Pet != null)
                Pet.RemovePet();

            //Save current map location if needed
            var newMap = ServerDatabase.Context.Maps.GetById(_id);

            //Update online training as required
            if (newMap.ID == 601 || newMap.ID == 1039)
                Send(UpdatePacket.Create(UID, UpdateType.OnlineTraining, 1));
            else if ((MapID == 601 || MapID == 1039) && (newMap.ID != 601 || newMap.ID != 1039))
                Send(UpdatePacket.Create(UID, UpdateType.OnlineTraining, 2));

            MapManager.RemovePlayer(this);

            if (Character != null && Map != null)
                if (newMap.Type.HasFlag(MapTypeFlags.RecordDisable))
                {
                    Character.X = Map.MapInfo.SpawnX;
                    Character.Y = Map.MapInfo.SpawnY;
                    Character.Map = Map.MapInfo.SpawnID;
                }
                else
                    Character.Map = _id;
            Location = new Point(_x, _y);
            SpawnPacket.PositionX = _x;
            SpawnPacket.PositionY = _y;
            Send(new GeneralActionPacket()
            {
                UID = this.UID,
                Data1 = _id,
                Data2Low = X,
                Data2High = Y,
                Action = DataAction.Teleport,
            });
            MapManager.AddPlayer(this, _id);
            AddStatus(Enum.ClientStatus.ReviveProtection, 0, 5 * Common.MS_PER_SECOND);
            Send(MapStatusPacket.Create(Map.MapInfo));


        }
        #endregion

        #region Handle Jump Packet
        public void HandleJump(GeneralActionPacket packet)
        {
            if (Common.MapService.Valid((ushort)Map.ID, X, Y, packet.Data1Low, packet.Data1High))
            {
                X = packet.Data1Low;
                Y = packet.Data1High;
                OnMove();
                SendToScreen(packet, true);
                UpdateSurroundings();


                if (Pet != null && Common.Clock - Pet.LastMove > 900)
                    Pet.LastMove = Common.Clock + 300;
            }
            else

                Send(new GeneralActionPacket()
                {
                    UID = this.UID,
                    Data1 = Map.ID,
                    Data2Low = X,
                    Data2High = Y,
                    Action = DataAction.NewCoordinates,
                });
        }
        #endregion

        #region On Move
        public void OnMove()
        {
            if (Map != null)
            {
                if (HasEffect(ClientEffect.LuckDiffuse))
                {
                    RemoveEffect(ClientEffect.LuckDiffuse);
                    foreach (var player in Map.QueryScreen<Player>(this))
                    {
                        bool found = false;
                        foreach (var pSurroundings in player.Map.QueryBox<Player>(player, 3))
                            if (pSurroundings.HasEffect(ClientEffect.LuckDiffuse))
                            { found = true; break; }
                        if (!found)
                            player.RemoveEffect(ClientEffect.LuckAbsorb);
                    }
                }
                if (HasEffect(ClientEffect.LuckAbsorb))
                {
                    foreach (var player in Map.QueryScreen<Player>(this))
                    {
                        var found = false;
                        if (player.HasEffect(ClientEffect.LuckDiffuse) && Calculations.GetDistance(this, player) < 3)
                        {
                            found = true;
                            break;
                        }
                        if (!found)
                            RemoveEffect(ClientEffect.LuckAbsorb);
                    }
                }
            }


            if (HasStatus(Enum.ClientStatus.ReviveProtection))
                RemoveStatus(Enum.ClientStatus.ReviveProtection);
            if (HasStatus(Enum.ClientStatus.Intensify))
            {
                RemoveStatus(Enum.ClientStatus.Intensify);
                RemoveEffect(ClientEffect.Intensify);
            }
            LastSitAt = Common.Clock;
            Action = ActionType.None;
            if (CombatManager != null && CombatManager.IsActive)
                CombatManager.AbortAttack(true);
            if (Mining)
                StopMining();
            
        }
        #endregion

        #endregion

        #region Send Chat Packet
        public void SendMessage(string msg, ChatType type = ChatType.GM)
        {
            Send(new TalkPacket(type, msg));
        }
        public void SendSysMessage(string msg)
        {
            Send(new TalkPacket(ChatType.System, msg, ChatColour.Red));
        }
        #endregion

        #region Direct Send Packet
        public void DirectSend(byte[] _packet)
        {
            if (UseThreading)
                Buffer.BlockCopy(Common.SERVER_SEAL, 0, _packet, _packet.Length - 8, 8);
            lock (_cryptographer)
                _cryptographer.Encrypt(_packet);
            Socket.Send(_packet);
        }
        #endregion

        #region Enqueue Packet
        public override void Send(byte[] _data)
        {
            if (disconnected || ToSend.Count > 100 && !Socket.Alive || _data == null || _data.Length < 4)
            {
                Console.WriteLine("Terminating Socket");
                Disconnect(false);
                return;
            }
            if (ToSend != null)
                ToSend.Enqueue(_data.UnsafeClone());
        }
        #endregion

        #region Send Packet To Screen
        public override void SendToScreen(byte[] _data, bool _self = false)
        {
            if (Map == null)
                return;
            foreach (var id in VisibleObjects.Keys)
            {
                var p = Map.Search<Player>(id);
                if (p != null)
                    p.Send(_data.UnsafeClone());
            }
            if (_self)
                Send(_data);
        }
        #endregion

        #region DHKeyExchange
        public void StartExchange()
        {
            DirectSend(_exchange.CreateServerKeyPacket());
        }

        public unsafe void CompleteExchange(byte[] buffer)
        {
            if (buffer.Length > 36)
            {
                byte[] publicKey;

                fixed (byte* ptr = buffer)
                {
                    var length = *((int*)(ptr + 7));
                    var junk = *((int*)(ptr + 11));
                    var publicKeyLength = *((int*)(ptr + 15 + junk));

                    publicKey = new byte[publicKeyLength];
                    for (var i = 0; i < publicKeyLength; i++)
                    {
                        publicKey[i] = *(ptr + 19 + junk + i);
                    }
                }
                _exchange.HandleClientKeyPacket(Encoding.ASCII.GetString(publicKey), ref _cryptographer);
                UseThreading = true;
            }
        }
        #endregion

        #region Stop Mining
        public void StopMining()
        {
            Mining = false;
            MiningAttempts = 0;
        }
        #endregion

        #region Disconnect
        public void Disconnect(bool save = true)
        {
            if (disconnected)
                return;
            disconnected = true;
            RemoveEffect(ClientEffect.LuckDiffuse);
            OnMove();
            if (AssociateManager != null) //New players
                AssociateManager.Close();

            if (Pet != null)
                Pet.RemovePet();

            //Shops
            if (Shop != null && Shop.Vending)
                Shop.StopVending();

            //Trade
            if (Trade != null)
                if (Trade.Target != null)
                {
                    var packet = TradePacket.Create(this);
                    packet.Subtype = TradeType.HideTable;


                    if (Trade.Owner.UID == this.UID)
                    {
                        packet.Target = Trade.Owner.UID;
                        Trade.Target.Send(packet);
                        Trade.Target.Trade = null;
                    }
                    else
                    {
                        packet.Target = Trade.Target.UID;
                        Trade.Owner.Send(packet);
                        Trade.Owner.Trade = null;
                    }
                }

            //Team
            if (Team != null)
            {
                if (Team.Leader == this)
                    Team.SendToTeam(TeamInteractPacket.Create(UID, TeamInteractType.Dismiss));

                if (Team.Members.Contains(this))
                {
                    Team.Members.Remove(this);
                    Team.SendToTeam(TeamInteractPacket.Create(UID, TeamInteractType.LeaveTeam));
                }
            }

            PlayerManager.RemovePlayer(this);
            MapManager.RemovePlayer(this);

            if (Character != null)
            {
                Character.Online = false;
                if (save)
                    Save();
            }
            if (Socket.Alive)
            {
                Socket.Disconnect();
                ToSend = null;
            }


        }
        #endregion

        #region Save
        public void Save()
        {
            ServerDatabase.Context.Characters.Update(Character);
        }
        #endregion

        #region Apply

        public void SetApply(ApplyType type, uint targetId)
        {
            _apply = type;
            _applyTargetId = targetId;
        }

        public uint FetchApply(ApplyType type)
        {
            if (_apply == type)
            {
                _apply = ApplyType.None;
                return _applyTargetId;
            }

            return 0;
        }

        public bool FetchApply(ApplyType type, uint targetId)
        {
            if (_apply == type && _applyTargetId == targetId)
            {
                _apply = ApplyType.None;
                return true;
            }

            return false;
        }

        public int IsOnline(String Name)
        {
            if (PlayerManager.GetUser(Name) != null)
            {
                return 1;
            }
            else
                return 0;
        }


        #endregion
        #endregion
    }
}
