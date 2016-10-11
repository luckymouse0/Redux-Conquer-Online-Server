using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;
using Redux.Database;
using Redux.Database.Domain;

namespace Redux.Structures
{
    public class Task
    {
        public Task(DbTask task)
        {
            _dbTask = task;
        }
        public Task(uint owner, TaskType type, DateTime expires)
        {
            _dbTask = new DbTask();
            _dbTask.Owner = owner;
            _dbTask.Type = type;
            _dbTask.Expires = expires;
            _dbTask.Count = 0;
            Save();
        }

        public int Condition { get { return _dbTask.Condition; } set { _dbTask.Condition = value; Save(); } }
        public int Count { get { return _dbTask.Count; } set { _dbTask.Count = value; Save(); } }
        public DateTime Expires { get { return _dbTask.Expires; } set { _dbTask.Expires = value; Save(); } }
        public TaskType Type { get { return _dbTask.Type; } }
        private DbTask _dbTask;

        private void Save()
        {
            ServerDatabase.Context.Tasks.AddOrUpdate(_dbTask);
        }
    }
}
