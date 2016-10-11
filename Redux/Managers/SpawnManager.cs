using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;
using Redux.Space;
using Redux.Database;
using Redux.Game_Server;
using Redux.Database.Domain;

namespace Redux.Managers
{
    /// <summary>
    /// Manages the actions and updates to a single monster spawn
    /// </summary>
    public class SpawnManager
    {
        public bool IsActive = false;
        //Each spawn consists of one monster type only
        public DbMonstertype BaseType { get; private set; }
        public List<DbDropRule> DropRules { get; private set; }
        public Point TopLeft, BottomRight;
        public int Frequency;
        public int AmountPer, AmountMax;
        private long lastSpawn;
        public Queue<Monster> DeadMembers { get; private set; }
        public ConcurrentDictionary<uint, Monster> DyingMembers { get; private set; }
        public ConcurrentDictionary<uint, Monster> AliveMembers { get; private set; }
        public Entity PetOwner { get; set; }
        public Map Map { get; private set; }

        public SpawnManager(Entity player, DbMagicType skill)
        {
            PetOwner = player;
            var magictype = ServerDatabase.Context.MagicType.GetById((uint)skill.ID + skill.Level);
            BaseType = ServerDatabase.Context.Monstertype.GetById((uint)magictype.Power);
            DropRules = ServerDatabase.Context.DropRules.GetRulesByMonsterType((uint)magictype.Power).ToList();
            TopLeft = new Point(player.X, player.Y);
            BottomRight = new Point(player.X, player.Y);
            Map = player.Map;
            DeadMembers = new Queue<Monster>();
            DyingMembers = new ConcurrentDictionary<uint, Monster>();
            AliveMembers = new ConcurrentDictionary<uint, Monster>();
            DeadMembers.Enqueue(new Monster(BaseType, this, player));
        }

        public SpawnManager(DbSpawn _baseSpawn, Map _map)
        {
            BaseType = ServerDatabase.Context.Monstertype.GetById(_baseSpawn.MonsterType);
            DropRules = ServerDatabase.Context.DropRules.GetRulesByMonsterType(_baseSpawn.MonsterType).ToList();
            TopLeft = new Point(_baseSpawn.X1, _baseSpawn.Y1);
            BottomRight = new Point(_baseSpawn.X2, _baseSpawn.Y2);
            Frequency = _baseSpawn.Frequency;
            AmountPer = _baseSpawn.AmountPer;
            AmountMax = _baseSpawn.AmountMax;
            DeadMembers = new Queue<Monster>();
            DyingMembers = new ConcurrentDictionary<uint, Monster>();
            AliveMembers = new ConcurrentDictionary<uint, Monster>();
            Map = _map;
            for (int i = 0; i < AmountMax; i++)
                DeadMembers.Enqueue(new Monster(BaseType, this));
        }

        public void SpawnManager_Timer()
        {
            if (Common.Clock - lastSpawn > Common.MS_PER_SECOND * Frequency && AliveMembers.Count < AmountMax)
            {
                var toSpawn = Math.Min(AmountPer, DeadMembers.Count);
                if (toSpawn > 0)
                {
                    lastSpawn = Common.Clock;
                    for (int i = 0; i < toSpawn; i++)
                    {
                        var mob = DeadMembers.Dequeue();
                        mob.Respawn();
                    }
                }
            }
            if (DyingMembers.Count > 0)
                foreach (var m in DyingMembers)
                    if (Common.Clock - m.Value.DiedAt > Common.MS_PER_SECOND * 3)
                        m.Value.Faded();

            if (AliveMembers.Count > 0 && IsActive)
                foreach (var member in AliveMembers.Values)
                    if (member.Alive && member.IsActive)
                        member.Monster_Timer();


        }
    }
}
