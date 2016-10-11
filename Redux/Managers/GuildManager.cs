using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Database;
using Redux.Enum;
using Redux.Structures;
using Redux.Packets;
using Redux.Database.Domain;

namespace Redux.Managers
{
    class GuildManager
    {
        private static readonly IDictionary<uint, Structures.Guild> _guilds;

        static GuildManager()
        {
            _guilds = new Dictionary<uint, Structures.Guild>();
        }

        public static bool Initialize()
        {
            var results = ServerDatabase.Context.Guilds.GetAll();

            foreach (var res in results)
            {
                var guild = new Structures.Guild();
                if (guild.Create(res))
                {
                    _guilds.Add(guild.Id, guild);
                }
            }

            var winnerID = ServerDatabase.Context.Events.GetWinner();
            if (winnerID != null)
                GuildWar.CurrentWinner = GuildManager.GetGuild(winnerID.WinnerID);

            return true;
        }

        public static Structures.Guild GetGuild(uint id)
        {
            if (id == 0 || !_guilds.ContainsKey(id)) return null;
            return _guilds[id];
        }

        public static Structures.Guild GetGuildByName(string name)
        {
            return _guilds.Values.FirstOrDefault(g => g.Name == name);
        }

        public static IEnumerable<Structures.Guild> Select(Func<Structures.Guild, bool> predicate)
        {
            return _guilds.Values.Where(predicate);
        }

        /// <summary>
        /// Creates a new <see cref="Structures.Guild"/> from the <paramref name="info"/> provided.
        /// </summary>
        /// <returns>The id of the new guild</returns>
        public static uint CreateGuild(Structures.CreateGuildInfo info)
        {
            var dbGuild = new DbGuild
            {
                Name = info.Name,
                Announce = string.Empty,
                LeaderId = info.LeaderId,
                LeaderName = info.LeaderName,
                Money = info.Money
               
            };

            try
            {
                ServerDatabase.Context.Guilds.Add(dbGuild);
            }
            catch (Exception)
            {
                return 0;
            }
            info.GuildId = dbGuild.Id;

            if (GetGuild(info.GuildId) != null)
            {
                return info.GuildId;
            }

            var guild = new Structures.Guild();
            if (guild.Create(info, true))
            {
                _guilds.Add(guild.Id, guild);

                var leader = PlayerManager.GetUser(info.LeaderId);
                if (leader != null)
                {
                    leader.GuildAttribute.Rank = GuildRank.GuildLeader;
                }
            }

            return dbGuild.Id;
        }

        public static bool DestroyGuild(uint guildId, uint targetId)
        {
            if (guildId == 0) return false;

            foreach (var g in _guilds.Values)
            {
                if (g != null && g.FealtyGuild == guildId)
                {
                    if (!DestroyGuild(g.Id, targetId))
                    {
                        return false;
                    }
                }
            }

            var masterGuild = _guilds[guildId];
            masterGuild.DeleteFlag = true;
            masterGuild.SaveInfo();

            var guild = GetGuild(guildId);
            if (guild != null)
            {
                PlayerManager.SendToServer(new Packets.Game.TalkPacket(ChatType.GM, string.Format("Guild {0} has been terminated.", guild.Name)));
                PlayerManager.SendToServer(Packets.Game.GuildPackets.Create(GuildAction.DestroySyndicate, guildId, 0));

                Structures.Guild targetGuild = null;
                if (targetId != 0) targetGuild = GetGuild(targetId);

                foreach (var user in PlayerManager.Select(p => p != null && p.GuildAttribute.GuildId == guildId))
                {
                    if (targetId == 0)
                    {
                        user.GuildAttribute.LeaveGuild(false, false);
                    }
                    else
                    {
                        user.GuildAttribute.SetIdRank(targetId, GuildRank.Member, false);
                        
                    }
                }

                _guilds.Remove(guildId);
                if(guildId != 0)
                    Database.ServerDatabase.Context.GuildAttributes.DeleteAttr(guildId);
            }

            return true;
        }
    }
}
