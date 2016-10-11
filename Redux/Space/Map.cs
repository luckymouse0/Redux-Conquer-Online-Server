using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Redux.Utility;
using Redux.Database;
using Redux.Database.Domain;
using Redux.Game_Server;
using Redux.Managers;
using Redux.Enum;
namespace Redux.Space
{
    #region Map Class
    public class Map
    {
        #region Variables
        public bool IsActive = true;
        private List<DbDropRule> _mineRules;
        public List<DbDropRule> MineRules { get { return _mineRules; } }
        public uint ID { get; private set; }
        public uint DynamicID { get; private set; }
        public DbMap MapInfo { get; private set; }
        public ConcurrentDictionary<uint, ILocatableObject> Objects { get; private set; }
        public List<SpawnManager> Spawns { get; private set; }
        public ThreadSafeCounter ItemCounter, MobCounter, SobCounter, PetCounter;
        #endregion
        #region Constructor
        /// <summary>
        /// Initialize a new Map
        /// </summary>
        /// <param name="dynamicID">Key to identify map</param>
        /// <param name="id">Dmap value of map. Used for all movement checks</param>
        public Map(uint _dynamicID, uint _id)
        {
            Objects = new ConcurrentDictionary<uint, ILocatableObject>();
            ItemCounter = new ThreadSafeCounter(100000, 150000);
            SobCounter = new ThreadSafeCounter(150000, 200000);
            MobCounter = new ThreadSafeCounter(400000, 500000);
            PetCounter = new ThreadSafeCounter(500000, 600000);
            MapInfo = ServerDatabase.Context.Maps.GetById(_id);
            ID = _id;
            DynamicID = _dynamicID;
            var dNpcs = ServerDatabase.Context.Npcs.GetNpcsByMap((ushort)_dynamicID);
            if (dNpcs.Count > 0)
                foreach (var dNpc in dNpcs)
                    Insert(new Npc(dNpc, this));

            var dSpawns = ServerDatabase.Context.Spawns.GetSpawnsByMap((ushort)_dynamicID);
            Spawns = new List<SpawnManager>();
            if(dSpawns.Count > 0)
                foreach (var dSpawn in dSpawns)                
                    Spawns.Add(new SpawnManager(dSpawn, this));

            foreach(var dSob in ServerDatabase.Context.SOB.GetSOBByMap((ushort)_dynamicID))            
                Insert(new SOB(dSob));

            if (MapInfo.Type.HasFlag(MapTypeFlags.MineEnable))            
                _mineRules = ServerDatabase.Context.DropRules.GetRulesByMonsterType(ID).ToList();
                    
            Console.WriteLine("Map ID {0} loaded {1} npcs and {2} spawns", _dynamicID, dNpcs.Count, dSpawns.Count);
        }
        #endregion

        #region Functions
        public bool IsTGMap { get { return ID == 1039; } }
        public bool IsNoScrollEnabled { get { return MapInfo.Type.HasFlag(Enum.MapTypeFlags.TeleportDisable); } }
        public bool IsFlyEnabled { get { return !MapInfo.Type.HasFlag(Enum.MapTypeFlags.FlyDisable); } }
        public bool IsPKEnabled { get { return !MapInfo.Type.HasFlag(Enum.MapTypeFlags.PkDisable); } }
        public bool IsNeverWound { get { return MapInfo.Type.HasFlag(Enum.MapTypeFlags.NeverWound); } }
        public bool IsFreePK { get { return MapInfo.Type.HasFlag(Enum.MapTypeFlags.PkField) || MapInfo.Type.HasFlag(Enum.MapTypeFlags.FreePk); } }
        public bool IsGuildMap { get { return MapInfo.Type.HasFlag(Enum.MapTypeFlags.GuildMap); } }
        public IEnumerable<ILocatableObject> QueryScreen(ILocatableObject objCenter)
        {
            var area = GetEntityScreenArea(objCenter);
            var query = from x in Objects.Values
                        where area.AreaContains(x.Location)
                        select x;
            return query;
        }

        public IEnumerable<T> QueryScreen<T>(ILocatableObject objCenter)
        {
            var area = GetEntityScreenArea(objCenter);
            var query = from x in Objects.Values
                        where area.AreaContains(x.Location) && x is T
                        select (T)x;
            return query;
        }
        public IEnumerable<T> QueryBox<T>(ILocatableObject _objCenter, int _size)
        {
            var area = new Rectangle(new Point(_objCenter.Location.X - _size, _objCenter.Location.Y - _size), _size * 2, _size * 2);
            var query = from x in Objects.Values
                        where area.AreaContains(x.Location) && x is T
                        select (T)x;
            return query;
        }
        public IEnumerable<Entity> QueryBox(int _x, int _y, int _width, int _height)
        {
            var area = new Rectangle(_x, _y, _width, _height);
            var query = from x in Objects.Values
                        where area.AreaContains(x.Location) && x is Entity                        
                        select x as Entity;
            return query;
        }

        public IEnumerable<Game_Server.Player> QueryPlayers(ILocatableObject objCenter)
        {
            var area = GetEntityScreenArea(objCenter);
            var query = from x in Objects.Values
                        where area.AreaContains(x.Location) && x is Game_Server.Player
                        select x as Game_Server.Player;
            return query;
        }

        public Rectangle GetEntityScreenArea(ILocatableObject objCenter)
        {
            return new Rectangle(new Point(objCenter.Location.X - 18, objCenter.Location.Y - 18), 36, 36);
        }

        public bool Insert(ILocatableObject obj)
        {
            if (obj == null)
                return false;
            if (Objects.ContainsKey(obj.UID))
                return false;
            obj.Map = this;
            Objects.TryAdd(obj.UID, obj);
            return true;
        }
        public bool Remove(ILocatableObject obj, bool updatelocal = true)
        {
            if (updatelocal)
            {
                if (obj is Entity)
                {
                    var entity = obj as Entity;
                    entity.SendToScreen(new Packets.Game.GeneralActionPacket() { UID = entity.UID, Action = Enum.DataAction.RemoveEntity });
                    foreach (var id in entity.VisibleObjects.Keys)
                        Managers.MapManager.DespawnByUID(entity, id);
                }
            }
            return Objects.TryRemove(obj.UID, out obj);            

        }
        public ILocatableObject Search(uint uid)
        {
            lock (Objects)
            {
                var query = from x in Objects.Values
                            where x.UID == uid
                            select x;
                return query.FirstOrDefault();
            }
        }
        public T Search<T>(uint id)
            where T : ILocatableObject
        {
            if (id != 0)
                foreach (var target in Objects)
                    if (target.Value is T)
                        if (target.Key == id)
                            return (T)target.Value;
            return default(T);
        }

        public IEnumerable<KeyValuePair<uint, ILocatableObject>> Select(Func<KeyValuePair<uint, ILocatableObject>, bool> predicate)
        {
            return Objects.Where(predicate);
        }

        public bool IsValidMonsterLocation(Point location)
        {
            return Common.MapService.Valid((ushort)ID, (ushort)location.X, (ushort)location.Y) && !Common.MapService.HasFlag((ushort)ID, (ushort)location.X, (ushort)location.Y, TinyMap.TileFlag.Monster);
        }
        public bool IsValidItemLocation(Point location)
        {
            return Common.MapService.Valid((ushort)ID, (ushort)location.X, (ushort)location.Y) && !Common.MapService.HasFlag((ushort)ID, (ushort)location.X, (ushort)location.Y, TinyMap.TileFlag.Item);
        }
        public bool IsValidPlayerLocation(Point location)
        {
            return Common.MapService.Valid((ushort)ID, (ushort)location.X, (ushort)location.Y);
        }
        #endregion
    }
    #endregion
}
