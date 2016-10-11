using System;
using Redux.Database.Domain;
using Redux.Enum;
using Redux.Managers;
using Redux.Game_Server;

namespace Redux.Game_Server
{
    public class TeamMember
    {
        private Player _client { get; set; }
        private uint TeamID { get; set; }

        public string Name { get { return _client.Name; } }
        public uint PlayerID { get { return _client.UID; } }
        public uint Mesh { get { return _client.Lookface; } }
        public uint Life { get { return _client.Life; } }
        public uint MaximumLife { get { return _client.MaximumLife; } }

        public TeamMember(Player member, uint teamid)
        {
            _client = member;
            TeamID = teamid;
        }

        public void Send(byte[] data)
        {
            if (_client != null)
                _client.Send(data);
        }
    }
}
