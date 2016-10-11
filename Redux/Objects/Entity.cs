using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Redux.Space;
using Redux.Enum;
using Redux.Managers;
using Redux.Packets.Game;
using Redux.Structures;
using Redux.Database.Domain;

namespace Redux.Game_Server
{
    public abstract class Entity : ILocatableObject
    {

        public SpawnEntityPacket SpawnPacket;
        public ConcurrentDictionary<uint, uint> VisibleObjects = new ConcurrentDictionary<uint, uint>();
        public Point Location { get; set; }
        public Map Map { get; set; }
        public uint UID { get; set; }
        public CombatStatistics CombatStats;
        public ConcurrentDictionary<Enum.ClientEffect, long> ClientEffects { get; private set; }
        public ConcurrentDictionary<Enum.ClientStatus, Structures.ClientStatus> ClientStatuses { get; private set; }
        public abstract void Send(byte[] _packet);
        public abstract void SendToScreen(byte[] _packet, bool _self = false);
        public virtual ushort WeaponType { get { return 0; } }
        public virtual int AttackSpeed { get { return CombatStats.AttackSpeed; } }
        public virtual int AttackRange { get { return Math.Max(2, CombatStats.AttackRange); } }
        public virtual byte Xp { get; set; }
        public virtual string Name { get; set; }
        public virtual PKMode PkMode { get; set; }
        public EquipmentManager Equipment { get; set; }
        public virtual byte Level { get; set; }
        public abstract void SetDisguise(Database.Domain.DbMonstertype _mob, long _duration);

        public int GetLevelBonusDamageFactor(int l1, int l2)
        {
            int num = l1 - l2;
            int bonus = 1;
            if (num >= 3)
            {
                bonus = 2 + num / 3;
            }
            return bonus;
        }

        public virtual ulong CalculateExperienceGain(Entity _target, uint _dmg)
        {
            if (_target is Player || _target is Pet) return 0;
            if (_target is SOB && !Map.IsTGMap) return 0;
            var exp = (double)Math.Min(_target.Life, _dmg);
            var deltaLevel = Level - _target.Level;
            if (deltaLevel >= 3)//green
            {
                if (deltaLevel >= 3 && deltaLevel <= 5)
                    exp *= .7;
                else if (deltaLevel > 5 && deltaLevel <= 10)
                    exp *= .2;
                else if (deltaLevel > 10 && deltaLevel <= 20)
                    exp *= .1;
                else if (deltaLevel > 20)
                    exp *= .05;
            }
            else if (deltaLevel < -15)
                exp *= 1.8;
            else if (deltaLevel < -8)
                exp *= 1.5;
            else if (deltaLevel < -5)
                exp *= 1.3;
            if (Map.IsTGMap)
                exp /= 10;
            return (ulong)exp;
        }

        public virtual uint CalculateBowDamage(Entity _target, DbMagicType _spell, bool _canDodge = false)
        {
            var dmg = Common.Random.Next(CombatStats.MinimumDamage, CombatStats.MaximumDamage) * (CombatStats.BonusAttackPct + CombatStats.DragonGemPct) / 150;
            if (_target is Player)
                dmg /= 3;
            else if (_target is Monster)
                dmg *= GetLevelBonusDamageFactor(Level, _target.Level);
            if (_spell != null)
                dmg = dmg * (_spell.Power % 1000) / 100;
            if (HasEffect(ClientEffect.Superman))
                if (_target is Monster)
                    dmg *= 10;
                else
                    dmg *= 2;
            dmg = dmg * (100 - _target.CombatStats.BlessPct) / 100;
            dmg = dmg * (100 - _target.CombatStats.TortGemPct) / 100;
            dmg = dmg * (100 - _target.CombatStats.Dodge) / 100;
            if (dmg < 1)
                dmg = 1;
            if (_canDodge && !Common.PercentSuccess(75 - _target.CombatStats.Dodge + CombatStats.Accuracy))
                dmg = 0;
            return (uint)dmg;
        }

        public virtual uint CalculatePhysicalDamage(Entity _target, DbMagicType _spell, bool _canDodge = false)
        {
            var dmg = Common.Random.Next(CombatStats.MinimumDamage, CombatStats.MaximumDamage) * (CombatStats.BonusAttackPct + CombatStats.DragonGemPct) / 100;
            if (_target is Monster)
                dmg *= GetLevelBonusDamageFactor(Level, _target.Level);
            if (_spell != null && _spell.Power > 0)
                dmg = (int)((double)dmg * (double)(_spell.Power % 1000) / (double)100);
            if (HasEffect(ClientEffect.Superman))
                if (_target is Monster)
                    dmg *= 10;
                else
                    dmg *= 2;
            dmg = dmg * (100 - _target.CombatStats.BlessPct) / 100;
            dmg = dmg * (100 - _target.CombatStats.TortGemPct) / 100;
            dmg -= _target.CombatStats.Defense * CombatStats.BonusDefensePct / 100;
            if (dmg < 1)
                dmg = 1;
            if (_canDodge && !Common.PercentSuccess(Math.Max(40, 90 - _target.CombatStats.Dodge + CombatStats.Accuracy)))
                dmg = 0;

            return (uint)dmg;
        }

        public virtual uint CalculateSkillDamage(Entity _target, DbMagicType _spell, bool _canDodge = false)
        {
            if (_spell == null)
                return 1;
            var dmg = (CombatStats.MagicDamage + _spell.Power) * (200 + CombatStats.PhoenixGemPct) / 100;
            if (_target is Monster)
                dmg *= GetLevelBonusDamageFactor(Level, _target.Level);
            dmg = dmg * (100 - _target.CombatStats.MagicResistance) / 100;
            dmg -= _target.CombatStats.MagicDefense;
            if (dmg < 0)
                dmg = 1;
            if (_canDodge && !Common.PercentSuccess(_spell.Percent))
                dmg = 0;
            return (uint)dmg;
        }

        public Entity()
        {
            CombatStats = new CombatStatistics();
            ClientStatuses = new ConcurrentDictionary<Enum.ClientStatus, Structures.ClientStatus>();
            ClientEffects = new ConcurrentDictionary<ClientEffect, long>();
        }
        public bool Alive { get { return Life > 0; } }
        public uint MaximumLife { get { return CombatStats.MaxLife; } }
        public ushort MaximumMana { get { return CombatStats.MaxMana; } }

        public virtual uint Life { get; set; }
        public virtual ushort Mana { get; set; }
        public virtual byte Stamina { get; set; }

        public virtual void ReceiveDamage(uint _dmg, uint _attacker)
        {
            if (Alive)
            {
                if (Life > _dmg)
                {
                    Life -= (ushort)_dmg;
                    //Adds blue name for PK
                    //If both are players
                    if (PlayerManager.Players.ContainsKey(_attacker) && (PlayerManager.Players.ContainsKey(this.UID)))
                    {
                        var attacker = Map.Search<Player>(_attacker);
                        if (!this.HasEffect(ClientEffect.Blue) && !this.HasEffect(ClientEffect.Black) && this.Map.IsPKEnabled == true && !this.Map.IsFreePK && !this.Map.IsGuildMap)
                        {
                            attacker.AddEffect(ClientEffect.Blue, 10000); //10 second blue name
                        }

                    }
                    else if (((this is Pet || PlayerManager.Players.ContainsKey(UID)) && (PetManager.ActivePets.ContainsKey(_attacker) || PlayerManager.Players.ContainsKey(_attacker))))
                    {
                        var attacker = Map.Search<Entity>(_attacker);
                        if (attacker != null)
                        {
                            if (attacker is Pet)
                                attacker = (attacker as Pet).PetOwner;

                            Entity receiver = this;
                            if (this is Pet)
                                receiver = (Entity)(this as Pet).PetOwner;
                            if (!receiver.HasEffect(ClientEffect.Blue) && !receiver.HasEffect(ClientEffect.Black) && receiver.Map.IsPKEnabled == true && !receiver.Map.IsFreePK && !receiver.Map.IsGuildMap)
                            {
                                attacker.AddEffect(ClientEffect.Blue, 10000); //10 second blue name
                            }
                        }

                    }
                    //Checks if target is a guard. Will need to add checks for summons in the future when they are implemented.
                    else if (this is Monster && PlayerManager.Players.ContainsKey(_attacker))
                    {
                        if ((this as Monster).BaseMonster.ID == 900)//Guard
                        {
                            var attacker = Map.Search<Player>(_attacker);
                            if (attacker != null)
                                attacker.AddEffect(ClientEffect.Blue, 3000);
                        }
                    }

                }
                else
                {
                    if (this is Monster && PlayerManager.Players.ContainsKey(_attacker))
                        if ((this as Monster).BaseMonster.ID == 900)//Guard
                        {
                            var attacker = Map.Search<Player>(_attacker);
                            if (attacker != null)
                                attacker.AddEffect(ClientEffect.Blue, 10000);
                        }
                    Life = 0;
                }

            }
        }
        internal bool sentDeath = false;
        public virtual void Kill(uint _dieType, uint _attacker)
        {
            if (!sentDeath)
            {
                sentDeath = true;
                SendToScreen(InteractPacket.Create(_attacker, UID, 0, 0, InteractAction.Kill, _dieType), true);
            }
            Life = 0;
        }
        public ushort MapID
        {
            get { return (ushort)Map.ID; }
        }

        public ushort X
        {
            get { return (ushort)Location.X; }
            set
            {
                var loc = Location; loc.X = (int)value; Location = loc;
                SpawnPacket.PositionX = value;
            }
        }

        public ushort Y
        {
            get { return (ushort)Location.Y; }
            set
            {
                var loc = Location; loc.Y = (int)value; Location = loc;
                SpawnPacket.PositionY = value;
            }
        }

        public void UpdateSurroundings(bool clear = false)
        {
            if (clear)
                VisibleObjects = new ConcurrentDictionary<uint, uint>();
            List<uint> newObjects = new List<uint>();

            foreach (var target in Map.QueryScreen(this))
                if (target.UID != UID)
                    newObjects.Add(target.UID);

            //Remove existing objects if they are no longer visible
            foreach (var id in VisibleObjects.Keys)
                if (!newObjects.Contains(id))
                    MapManager.DespawnByUID(this, id);

            //Remove new objects if they exist already
            foreach (var id in VisibleObjects.Keys)
                if (newObjects.Contains(id))
                    newObjects.Remove(id);

            //Spawn new objects
            foreach (var id in newObjects)
                MapManager.SpawnByUID(this, id);
        }


        #region Status Effect Handling
        public bool RemoveEffect(ClientEffect _effect, bool _sync = true)
        {
            bool success = HasEffect(_effect);
            if (success)
            {
                long expires = 0;
                ClientEffects.TryRemove(_effect, out expires);
                success = expires > 0;
                SpawnPacket.StatusEffects &= ~_effect;
                if (_sync)
                {
                    Send(UpdatePacket.Create(UID, UpdateType.StatusEffects, (ulong)SpawnPacket.StatusEffects));
                    SendToScreen(SpawnPacket);
                }
                if (_effect == ClientEffect.XpStart)
                    Xp = 0;
            }
            return success;
        }
        public bool AddEffect(ClientEffect _effect, long _timeout, bool _sync = true)
        {
            if (_effect == ClientEffect.Fly && !Map.IsFlyEnabled)
                return false;
            var timeout = _timeout > 0 ? Common.Clock + _timeout : 0;
            bool success = true;
            if (ClientEffects.ContainsKey(_effect))
                ClientEffects[_effect] = timeout;
            else
                success = ClientEffects.TryAdd(_effect, timeout);
            if (success && !(this is SOB))
            {
                SpawnPacket.StatusEffects |= _effect;
                if (_sync)
                {
                    Send(UpdatePacket.Create(UID, UpdateType.StatusEffects, (ulong)SpawnPacket.StatusEffects));
                    SendToScreen(SpawnPacket);
                }
            }

            return success;
        }
        public bool HasEffect(ClientEffect _effect)
        {
            return SpawnPacket.UID > 0 && SpawnPacket.StatusEffects.HasFlag(_effect);
        }
        public bool IsInXP
        {
            get { return HasEffect(ClientEffect.Superman) || HasEffect(ClientEffect.Cyclone); }
        }
        #endregion

        #region Status Handling
        public void AddStatus(Enum.ClientStatus _status, int _value, long _timeout)
        {
            var timeout = _timeout > 0 ? Common.Clock + _timeout : 0;
            if (ClientStatuses.ContainsKey(_status))
            { ClientStatuses[_status].Value = _value; ClientStatuses[_status].Timeout = timeout; }
            else
            {
                var status = new Structures.ClientStatus(_status, _value, timeout);
                ClientStatuses.TryAdd(_status, status);
                CombatStats.AddClientStatusStats(status);
            }
        }
        public void RemoveStatus(Enum.ClientStatus _status)
        {
            Structures.ClientStatus status;
            ClientStatuses.TryRemove(_status, out status);
            if (status != null)
            {
                if (status.Type == Enum.ClientStatus.TransformationTimeout)
                    SetDisguise(null, 0);
                else
                    CombatStats.RemoveClientStatusStats(status);
            }

        }
        public bool HasStatus(Enum.ClientStatus _status)
        {
            return ClientStatuses.ContainsKey(_status);
        }
        #endregion

        public void On_Entity_Timer()
        {
            foreach (var effect in ClientEffects)
                if (Common.Clock > effect.Value && effect.Value > 0)
                    RemoveEffect(effect.Key);
            
            foreach (var status in ClientStatuses)
                if (Common.Clock > status.Value.Timeout && status.Value.Timeout > 0)
                    
                    RemoveStatus(status.Key);
        }
    }
}
