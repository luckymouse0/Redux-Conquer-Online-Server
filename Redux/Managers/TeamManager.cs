using System.Collections.Generic;
using Redux.Game_Server;

namespace Redux.Managers
{
    public class TeamManager
    {     
        public Player Leader { get; private set; }
        public List<Player> Members { get; private set; }
        public bool IsJoinEnabled { get; set; }
        public bool IsMoneyEnabled { get; set; }
        public bool IsItemEnabled { get; set; }
        public int GetMemberCount { get { return Members.Count; } }

        public TeamManager(Player leader)
        {
            Leader = leader;
            Members = new List<Player>();
            IsJoinEnabled = true;
            IsMoneyEnabled = true;
            IsItemEnabled = false;
        }

        public void SendToTeam(byte[] packet, Player exclude = null)
        {
            if (Leader != exclude)
                Leader.Send(packet);
            foreach (var player in Members)
                if (player != exclude)
                    player.Send(packet);
        }

    }
}
