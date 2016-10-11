using System;
using Redux.Database.Domain;
using Redux.Enum;
using Redux.Managers;
using Redux.Game_Server;

namespace Redux.Game_Server
{
    public class Associate
    {
        public uint UID { get; private set;}
        public string Name { get; private set; }
        public bool IsOnline { get { return PlayerManager.Players.ContainsKey(this.UID); } }
        public AssociateType Type { get; private set; }
        public string Message { get; set; }
        private Player _client { get; set; }

        public Associate(DbAssociate dbAssoc)
        {
            UID = dbAssoc.AssociateID;
            Name = dbAssoc.Name;
            Type = (AssociateType)dbAssoc.Type;
            UpdateClient(true);
        }

        private void UpdateClient(bool force)
        {
            if (!IsOnline)
                return;
            if (force)
                _client = PlayerManager.Players[UID];
            else if (_client != null)
                return;
            else
                _client = PlayerManager.Players[UID];

        }

        public void Send(byte[] data)
        {
            UpdateClient(false);
            if (_client != null)
                _client.Send(data);
        }

    }
}
