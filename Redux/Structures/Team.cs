using System.Collections.Concurrent;
using Redux.Game_Server;
using Redux.Packets.Game;

namespace Redux.Structures
{
    public class Team
    {
        public uint TeamUID { get; private set; }
        public TeamMember Leader { get; private set; }
        public ConcurrentDictionary<uint, TeamMember> Members { get; private set; }
        private bool joinForbidden { get; set; }
        public bool MoneyForbidden { get; private set; }
        public bool ItemForbidden { get; private set; }
        public byte Count { get { return (byte) Members.Count; } }
        public bool Joinable { get { return !joinForbidden; } }

        /// <summary>
        /// Creates an instance of a team. (TeamUID is the leader's UID)
        /// </summary>
        /// <param name="leader">Team leader</param>
        public Team(Player leader)
        {
            TeamUID = leader.UID;
            Leader = new TeamMember(leader, leader.UID);
            Members = new ConcurrentDictionary<uint, TeamMember>();
            joinForbidden = false;
            MoneyForbidden = false;
            ItemForbidden = true;
        }

        public bool IsMember(Player member)
        {
            return IsMember(member.UID);
        }
        public bool IsMember(uint memberid)
        {
            if (TeamUID == memberid)
                return true;
            return Members.ContainsKey(memberid);
        }
    }
}
