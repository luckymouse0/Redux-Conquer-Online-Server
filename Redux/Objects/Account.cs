using System;
using Redux.Enum;
using Redux.Database;
using Redux.Database.Domain;

namespace Redux.Login_Server
{
    public class Account
    {
        private readonly DbAccount _data;
        public Account(DbAccount data)
        {
            _data = data;
        }

        public uint UID { get { return _data.UID; } }
        public string Username { get { return _data.Username; } set { _data.Username = value; } }
        public string Password { get { return _data.Password; } set { _data.Password = value; } }
        public string EMail { get { return _data.EMail; } set { _data.EMail = value; } }
        public string Question { get { return _data.Question; } set { _data.Question = value; } }
        public string Answer { get { return _data.Answer; } set { _data.Answer = value; } }
        public PlayerPermission Permission { get { return _data.Permission; } set { _data.Permission = value; } }
        public uint Token { get { return _data.Token; } set { _data.Token = value; } }
        public uint Timestamp { get { return _data.Timestamp; } set { _data.Timestamp = value; } }

        /// <summary>
        /// Allows an auth client to login to the game server.
        /// </summary>
        /// <param name="update">Determines whether to immediately update the row in the database.</param>
        public void AllowLogin(bool update = true)
        {
            Timestamp = Common.SecondsServerOnline;
            if (update) SaveInfo();
        }

        /// <summary>
        /// Saves or updates this instance.
        /// </summary>
        public void SaveInfo()
        {
            try
            {
                ServerDatabase.Context.Accounts.AddOrUpdate(_data);
            }
            catch (Exception P) { Console.WriteLine(P); }
        }
    }
}
