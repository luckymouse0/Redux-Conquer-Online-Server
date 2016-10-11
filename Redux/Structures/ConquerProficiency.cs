using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Database;
using Redux.Game_Server;
using Redux.Packets.Game;
using Redux.Database.Domain;

namespace Redux.Structures
{
    public class ConquerProficiency
    {
        public static ConquerProficiency Create(uint _owner, ushort _id, ushort _level, uint _exp = 0)
        {
            var db = new DbProficiency()
            {
                Owner = _owner,
                ID = _id,
                Level = _level,
                Experience = _exp,
                PreviousLevel = 0
            };
            return new ConquerProficiency(db);
        }
        public ConquerProficiency(DbProficiency _prof)
        {
            database = _prof;
        }
        public ushort ID { get { return database.ID; } set { database.ID = value; } }
        public ushort Level { get { return database.Level; } set { database.Level = value; } }
        public ushort PreviousLevel { get { return database.PreviousLevel; } set { database.PreviousLevel = value; } }
        public uint Experience { get { return database.Experience; } set { database.Experience = value; } }
        private DbProficiency database;
        public bool Save()
        {
            bool success = false;
            try
            {
                ServerDatabase.Context.Proficiencies.AddOrUpdate(database);
                success = true;
            }
            catch (Exception p) { Console.WriteLine(p); }
            return success;
        }
        public void Send(Entity _owner)
        {
            _owner.Send(WeaponProficiencyPacket.Create(this));
        }

    }
}
