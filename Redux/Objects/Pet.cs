using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Database.Domain;
using Redux.Managers;
using Redux.Packets.Game;
using Redux.Space;
using Redux.Enum;

namespace Redux.Game_Server
{
    public class Pet:Monster
    {
        public Player PetOwner;
        public Entity Target;
        public bool Remove;

        public Pet(Player owner, DbMonstertype _type) : base(_type)
        {
            PetOwner = owner;
            Map = owner.Map;
            UID = (uint)Map.PetCounter.Counter;
            if(_type.SkillType > 0)
                CombatEngine.skills.TryAdd(_type.SkillType, Structures.ConquerSkill.Create(UID, _type.SkillType, 0, 0));
            SpawnPacket = SpawnEntityPacket.Create(this);
            PetManager.ActivePets.TryAdd(UID, this);
        }

        public Point NextLocation(Entity To)
        {
            Point loc;
            int Attempts = 0;

            loc.X = (ushort)Common.Random.Next(To.X - 2, To.X + 2);
            loc.Y = (ushort)Common.Random.Next(To.Y - 2, To.Y + 2);
            while (!Map.IsValidMonsterLocation(loc))
            {
                loc.X = (ushort)Common.Random.Next(To.X - 2, To.X + 2);
                loc.Y = (ushort)Common.Random.Next(To.Y - 2, To.Y + 2);

                Attempts++;
                if (Attempts >= 10)
                {
                    if (Map.IsValidMonsterLocation(To.Location))
                        loc = To.Location;
                    else
                    {
                        Map.Remove(this);
                        Remove = true;
                        
                    }
                }
            }

            return loc;
        }
        public void Pet_Timer()
        {
            if (Remove == true)
                return;

            if (PetOwner != null)
            {
                if (Alive)
                {
                    #region Movement
                    if (Target == null || Space.Calculations.GetDistance(this, PetOwner) > 15 || Space.Calculations.GetDistance(this, Target) > BaseMonster.ViewRange)
                    {
                        int OwnerDistance = Space.Calculations.GetDistance(this, PetOwner);

                        if (OwnerDistance > 15)
                        {
                            if (Map.IsValidMonsterLocation(PetOwner.Location))
                            {
                                Point newloc = NextLocation(PetOwner);

                                X = (ushort)newloc.X;
                                Y = (ushort)newloc.Y;
                                SendToScreen(this.SpawnPacket);
                                UpdateSurroundings();
                                LastMove = Common.Clock;
                            }
                        }
                        else if (OwnerDistance >= 8 && OwnerDistance <= 15 && Common.Clock > LastMove + 900)
                        {

                            
                            if (Remove != true)
                            {
                                Jump(PetOwner);
                            }
                        }
                        else if (OwnerDistance < 8 && OwnerDistance >= 3)
                        {
                            if (Common.Clock - LastMove > BaseMonster.MoveSpeed)
                            {
                                if (Remove != true)
                                {
                                    Walk(PetOwner);
                                }
                            }
                        }
                    }
                    #endregion
                    else
                    {
                        #region Targeting

                        if (!CombatEngine.IsValidTarget(Target) || !Target.Alive)
                        {
                            //Target = null;
                            return;
                        }
                        int TargetDistance = Space.Calculations.GetDistance(this, Target);

                        if (TargetDistance <= BaseMonster.AttackRange)
                        {
                            if (Common.Clock - LastAttack > BaseMonster.AttackSpeed)
                            {
                                if (BaseMonster.Mesh == 920)
                                {
                                    LastAttack = Common.Clock;
                                    /*var skill = Database.ServerDatabase.Context.MagicType.GetById(BaseMonster.SkillType);
                                    dmg = CalculateSkillDamage(Target, skill, skill.Percent < 100);
                                   var packet = SkillEffectPacket.Create(UID, Target.UID, BaseMonster.SkillType, 0);
                                   packet.AddTarget(Target.UID, dmg);

                                   SendToScreen(packet);*/
                                    var pack = InteractPacket.Create(UID, Target.UID, Target.X, Target.Y, InteractAction.MagicAttack, 0);
                                    pack.MagicType = BaseMonster.SkillType;
                                    pack.MagicLevel = 0;
                                    pack.Target = Target.UID;
                                    CombatEngine.ProcessInteractionPacket(pack);  
                                }
                                else if (!Target.HasEffect(ClientEffect.Fly))
                                {
                                    
                                    
                                        LastAttack = Common.Clock;
                                        //dmg = CalculatePhysicalDamage(Target, null, true);
                                        //SendToScreen(InteractPacket.Create(UID, Target.UID, (ushort)Target.Location.X, (ushort)Target.Location.Y, InteractAction.Attack, dmg));
                                        CombatEngine.ProcessInteractionPacket(InteractPacket.Create(UID, Target.UID, Target.X, Target.Y, InteractAction.Attack, 0));  
                                    
                                }

                                /*if (dmg > 0)
                                {
                                    Target.ReceiveDamage(dmg, UID);

                                    var expGain = CalculateExperienceGain(Target, dmg);
                                    ((Player)PetOwner).GainExperience(expGain);
                                }*/
                            }
                        }
                        else if (TargetDistance > BaseMonster.AttackRange && TargetDistance <= BaseMonster.ViewRange)
                        {
                            if (TargetDistance - BaseMonster.AttackRange <= 4)
                                Walk(Target);
                            else
                                Jump(Target);
                        }
                        
                    }
                    #endregion
                }
            }
            else
            {
                Map.Remove(this, false);
            }
        }

        public void Walk(Entity To)
        {
            var dir = Calculations.GetDirection(Location, To.Location);
            Point tryMove = new Point(Location.X + Common.DeltaX[dir], Location.Y + Common.DeltaY[dir]);
            if (Common.MapService.Valid(MapID, (ushort)tryMove.X, (ushort)tryMove.Y) && !Common.MapService.HasFlag(MapID, (ushort)tryMove.X, (ushort)tryMove.Y, TinyMap.TileFlag.Monster))
            {
                //Send to screen new walk packet
                SendToScreen(WalkPacket.Create(UID, dir));
                Common.MapService.RemoveFlag(MapID, X, Y, TinyMap.TileFlag.Monster);
                X = (ushort)tryMove.X;
                Y = (ushort)tryMove.Y;
                Common.MapService.AddFlag(MapID, X, Y, TinyMap.TileFlag.Monster);
                LastMove = Common.Clock;
            }
        }

        public void Jump(Entity To)
        {

            Point newloc = NextLocation(To);

            var pack = GeneralActionPacket.Create(UID, DataAction.Jump, (ushort)newloc.X, (ushort)newloc.Y);
            pack.Data2Low = X;
            pack.Data2High = Y;
            X = (ushort)newloc.X;
            Y = (ushort)newloc.Y;
            LastMove = Common.Clock;
            SendToScreen(pack);
        }

        public override void Kill(uint _dieType, uint _attacker)
        {
            SendToScreen(InteractPacket.Create(_attacker, UID, X, Y, InteractAction.Kill, _dieType), true);
            Life = 0;
            DiedAt = Common.Clock;
            SpawnPacket.StatusEffects = ClientEffect.Dead;
            AddEffect(ClientEffect.Fade, 0);
            PetOwner.Pet = null;
            Target = null;
            Map.Remove(this, false);
        }

        public new void Faded()
        {
            Common.MapService.RemoveFlag(MapID, X, Y, TinyMap.TileFlag.Monster);
            var packet = GeneralActionPacket.Create(UID, DataAction.RemoveEntity, 0, 0);
            foreach (var p in Map.QueryPlayers(this))
                if (p.VisibleObjects.ContainsKey(UID))
                { uint x; p.Send(packet); p.VisibleObjects.TryRemove(UID, out x); }
        }

        public void RemovePet()
        {
            PetOwner.Pet = null;
            Map.Remove(this);
            Pet p;
            PetManager.ActivePets.TryRemove(UID, out p);
        }
    }
}

