using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;
using Redux.Database;
using Redux.Game_Server;
using Redux.Packets.Game;
using Redux.Database.Domain;
namespace Redux.Structures
{
    public class ConquerSkill
    { 
        public ConquerSkill()
        {

        }
        public static ConquerSkill Create(uint _owner, ushort _id, ushort _level, uint _exp = 0)
        {
            var db = new DbSkill()
            {
                ID = _id,
                Level = _level,
                Experience = _exp,
                Owner = _owner,
                PreviousLevel = 0,
            };
            return new ConquerSkill(db);            
        }
       
        public ConquerSkill(DbSkill _skill)
        {
            Database = _skill;
            Info = ServerDatabase.Context.MagicType.GetMagicTypeBySkill(this);
        }
        public ushort ID { get { return Database.ID; } set { Database.ID = value; } }
        public ushort Level
        {
            get { return Database.Level; }
            set
            {
                var oldLevel = Database.Level;
                Database.Level = value;
                var newSkillInfo = ServerDatabase.Context.MagicType.GetMagicTypeBySkill(this);
                if (newSkillInfo != null)
                    Info = newSkillInfo;
                else
                    Database.Level = oldLevel;
            }
        }
        public ushort PreviousLevel { get { return Database.PreviousLevel; } set { Database.PreviousLevel = value; } }
        public uint Experience { get { return Database.Experience; } set { Database.Experience = value; } }
        public DbMagicType Info;
        public DbSkill Database;

        public bool Save()
        {
            bool success = false;
            ServerDatabase.Context.Skills.AddOrUpdate(Database);
                success = true;
            return success;
        }


        public void Send(Entity _owner)
        {
            _owner.Send(ConquerSkillPacket.Create(this));
        }      
    }
}
