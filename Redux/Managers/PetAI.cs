using Redux.Game_Server;
using Redux.Packets.Game;
using Redux.Space;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Redux.Managers
{
    public class PetAI
    {
        private Entity owner;
        private Monster Pet;
        private Player PetOwner;

        public Timer MotorSystem;

        private int ThinkSpeed;
        private int MoveSpeed;
        private int AtkSpeed;

        private int ViewRange;
        private int MoveRange;
        private int AtkRange;

        private DateTime LastAttack;
        private int LastMoveTick;
        private int LastAwakeTick;

        private Entity Target;
        public PetAI(Entity owner, Monster Entity, int ThinkSpeed, int MoveSpeed, int AtkSpeed, int ViewRange, int MoveRange, int AtkRange)
        {
            this.owner = owner;
            this.Pet = Entity;
            this.Pet.SendToScreen(GeneralActionPacket.Create(Pet.UID, Enum.DataAction.SpawnEffect, 0), true);
            this.ThinkSpeed = ThinkSpeed;
            this.MoveSpeed = MoveSpeed;
            this.AtkSpeed = AtkSpeed + 1000;
            this.ViewRange = ViewRange;
            if (this.ViewRange > 15)
                this.ViewRange = 15;
            this.MoveRange = MoveRange;
            this.AtkRange = AtkRange;

            this.LastAttack = DateTime.Now;
            this.LastMoveTick = Environment.TickCount;
            this.LastAwakeTick = Environment.TickCount;
            this.MotorSystem = new System.Timers.Timer(10000);
            this.MotorSystem.Elapsed += new ElapsedEventHandler(MotorSystem_TakingDecision);
            this.MotorSystem.Interval = 500;
            this.MotorSystem.Enabled = true;
        }

        ~PetAI()
        {
            Pet = null;
            PetOwner = null;
            if (MotorSystem != null)
                MotorSystem.Stop();
            MotorSystem = null;
        }


        public void StopSystem()
        {
            Pet = null;
            PetOwner = null;
            if (MotorSystem != null)
                MotorSystem.Stop();
            MotorSystem = null;
        }
        public void Reset()
        {
            MotorSystem.Stop();
            MotorSystem = null;

            LastAttack = DateTime.Now;
            LastMoveTick = Environment.TickCount;

            Target = null;

            MotorSystem = new Timer();
            MotorSystem.Interval += ThinkSpeed;
            MotorSystem.Elapsed += new ElapsedEventHandler(MotorSystem_TakingDecision);
            if (Pet.Alive)
                MotorSystem.Start();
        }

        public Boolean IsInVisualField(Entity Target)
        {
            if (Target == null)
                return false;

            if (Pet.Map != Target.Map)
                return false;

            if (Calculations.GetDistance(Target.X, Target.Y, Pet.X, Pet.Y) <= ViewRange)
                return true;
            return false;
        }

        public Boolean IsInVisualField(Int16 MapId, UInt16 X, UInt16 Y)
        {
            if (Pet.MapID != MapId)
                return false;

            if (Calculations.GetDistance(X, Y, Pet.Location.X, Pet.Location.Y) <= ViewRange)
                return true;
            return false;
        }

        public Boolean IsInAttackField(Entity Target)
        {
            if (Target == null)
                return false;

            if (Pet.MapID != Target.MapID)
                return false;

            if (Calculations.GetDistance(Target.X, Target.Y, Pet.Location.X, Pet.Location.Y) <= AtkRange)
                return true;
            return false;
        }

        public Boolean IsInAttackField(Int16 MapId, UInt16 X, UInt16 Y)
        {
            if (Pet.MapID != MapId)
                return false;

            if (Calculations.GetDistance(X, Y, Pet.Location.X, Pet.Location.Y) <= AtkRange)
                return true;
            return false;
        }

        private void Follow()
        {
            if (!IsInVisualField(PetOwner))
            {
                Pet.Map = PetOwner.Map;
                var dir = Calculations.GetDirection(Pet.Location, PetOwner.Location);
                Point tryMove = new Point(Common.Random.Next(PetOwner.X - 15, PetOwner.X + 15), Common.Random.Next(PetOwner.Y - 15, PetOwner.Y + 15));
                if(!Common.MapService.Valid((ushort)Pet.Map.ID, (ushort)tryMove.X, (ushort)tryMove.Y))
                {
                    Pet.X = PetOwner.X;
                    Pet.Y = PetOwner.Y;
                }
                else { 
                    Pet.X = (ushort)tryMove.X;
                    Pet.Y = (ushort)tryMove.Y;
                }
                Pet.Direction = PetOwner.Direction;
                Pet.UpdateSurroundings(true);
                var packet = GeneralActionPacket.Create(Pet.UID, Enum.DataAction.Jump, PetOwner.X, PetOwner.Y);
                Pet.SendToScreen(packet, true);
            }
        }

        private void Attack()
        {
            if (this.Target != null && !IsInVisualField(this.Target))
            {
                this.Target = null;
            }
            if (this.Target == null)
                this.Target = PetOwner.CombatManager.target;
            if (this.Target != null)
            {
                if (DateTime.Now.Subtract(LastAttack).Seconds > 0)
                {
                    LastAttack = DateTime.Now;
                    Pet.CombatEngine.ProcessInteractionPacket(InteractPacket.Create(Pet.UID, Target.UID, Target.X, Target.Y, Enum.InteractAction.Attack, 0));
                }
                if (this.Target.Alive == false)
                {
                    Database.Domain.DbMagicType skill = Database.ServerDatabase.Context.MagicType.GetById(Pet.BaseMonster.SkillType);
                    uint dmg = Pet.CalculateSkillDamage(this.Target, skill, false);
                    var expGain = PetOwner.CalculateExperienceGain(this.Target, dmg);
                    ((Player)PetOwner).GainExperience(expGain);
                    this.Target = null;
                    Console.WriteLine("EXP gained " + expGain);
                }
            }
        }
        private void MotorSystem_TakingDecision(Object Sender, ElapsedEventArgs Args)
        {
            PetOwner = PlayerManager.GetUser(owner.UID);
            if (PetOwner != null)
            {
                Follow();
                Attack();
            }
        }
    }
}
