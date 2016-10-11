using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Enum;
using Redux.Database.Domain;
using Redux.Database;
using Redux.Structures;
using Redux.Packets.Game;
using Redux.Managers;
using Redux.Game_Server;

namespace Redux.Structures
{
    public class Guild
    {
        internal struct MemberInfo
            {
                public string Name;
                public GuildRank Rank;
                public byte Level;
                public int TotalDonation;

                public MemberInfo(DbGuildMemberInfo info)
                {
                    Name = info.Name;
                    Rank = info.Rank;
                    Level = (byte)info.Level;
                    TotalDonation = (int)info.Donation;
                }
            }

            private DbGuild _data;
            private IList<MemberInfo> _members;
            private uint[] _allies, _enemies;

            public uint Id { get { return _data.Id; } set { _data.Id = value; } }
            public string Name { get { return _data.Name; } set { _data.Name = value; } }
            public string Announce { get { return _data.Announce; } set { _data.Announce = value; } }
            public uint AnnounceDate { get { return _data.AnnounceDate; } set { _data.AnnounceDate = value; } }
            public uint LeaderId { get { return _data.LeaderId; } set { _data.LeaderId = value; } }
            public string LeaderName { get { return _data.LeaderName; } set { _data.LeaderName = value; } }
            public long Money { get { return _data.Money; } set { _data.Money = value; SaveInfo(); } }
            public bool DeleteFlag { get { return _data.DeleteFlag; } set { _data.DeleteFlag = value; } }
            public uint FealtyGuild { get { return _data.MasterGuildId; } set { _data.MasterGuildId = value; } }
            public int Amount { get { return _data.Amount; } set { _data.Amount = value; SaveInfo(); } }
            public uint Enemy0 { get { return _data.Enemy0; } set { _data.Enemy0 = value; } }
            public uint Enemy1 { get { return _data.Enemy1; } set { _data.Enemy1 = value; } }
            public uint Enemy2 { get { return _data.Enemy2; } set { _data.Enemy2 = value; } }
            public uint Enemy3 { get { return _data.Enemy3; } set { _data.Enemy3 = value; } }
            public uint Enemy4 { get { return _data.Enemy4; } set { _data.Enemy4 = value; } }
            public uint Ally0 { get { return _data.Ally0; } set { _data.Ally0 = value; } }
            public uint Ally1 { get { return _data.Ally1; } set { _data.Ally1 = value; } }
            public uint Ally2 { get { return _data.Ally2; } set { _data.Ally2 = value; } }
            public uint Ally3 { get { return _data.Ally3; } set { _data.Ally3 = value; } }
            public uint Ally4 { get { return _data.Ally4; } set { _data.Ally4 = value; } }

            public int DeputyLeaders { get; set; }

            public bool Create(DbGuild res)
            {
                if (_data != null) return false;

                _data = res;

                if (_data.DeleteFlag) return false;

                _members = new List<MemberInfo>();
                foreach (var info in ServerDatabase.Context.Guilds.GetMemberInfo((int)Id))
                {
                    if (info.Rank == GuildRank.DeputyLeader) DeputyLeaders++;
                    _members.Add(new MemberInfo(info));
                }

                _allies = new uint[5];
                _enemies = new uint[5];

                _allies[0] = Ally0;
                _allies[1] = Ally1;
                _allies[2] = Ally2;
                _allies[3] = Ally3;
                _allies[4] = Ally4;

                _enemies[0] = Enemy0;
                _enemies[1] = Enemy1;
                _enemies[2] = Enemy2;
                _enemies[3] = Enemy3;
                _enemies[4] = Enemy4;

                return true;
            }

            public bool Create(Structures.CreateGuildInfo info, bool save)
            {
                //if (_data != null) return false;

                _members = new List<MemberInfo>();
                _allies = new uint[5];
                _enemies = new uint[5];

                _data = save ? new DbGuild { Id = 0 } : ServerDatabase.Context.Guilds.GetById(info.GuildId);

                _data.Id = info.GuildId;
                _data.Name = info.Name;
                _data.Announce = string.Empty;
                _data.LeaderId = info.LeaderId;
                _data.LeaderName = info.LeaderName;
                _data.Money = info.Money;
                
                _data.Amount = 0; //info.DeleteFlag ? 0 : 1;

                if (save)
                {
                    ServerDatabase.Context.Guilds.AddOrUpdate(_data);
                    return true;
                }

                return true;
            }

            public void SaveInfo()
            {
                ServerDatabase.Context.Guilds.Update(_data);
            }

            public bool IsMasterGuild { get { return FealtyGuild == 0; } }

            public uint MasterGuildId { get { return FealtyGuild != 0 ? FealtyGuild : Id; } }

            /// <summary>
            /// Recursively gets the parent guild.
            /// </summary>
            public Guild MasterGuild
            {
                get
                {
                    if (FealtyGuild != 0)
                    {
                        var masterGuild = GuildManager.GetGuild(FealtyGuild);
                        if (masterGuild != null)
                        {
                            return masterGuild.MasterGuild;
                        }
                    }

                    return this;
                }
            }

            public void SendMemberList(Player client)
            {
                StringsPacket packet = new StringsPacket();

                packet.Strings = new Packets.NetStringPacker();
                packet.Type = StringAction.MemberList;
                packet.UID = client.UID;

                if (client.GuildId <= 0)
                    return;
                String GL = "";
                List<String> OnMembers = new List<String>();
                List<String> OffMembers = new List<String>();
                List<String> OnDeputies = new List<String>();
                List<String> OffDeputies = new List<String>();

                var memberlist = Database.ServerDatabase.Context.GuildAttributes.GetMembers(client.GuildId);

                foreach (var entry in memberlist)
                {
                    var member = Database.ServerDatabase.Context.Characters.GetByUID(entry.Id);

                    if (entry.Rank == GuildRank.GuildLeader)
                    {
                        GL = member.Name + Convert.ToChar(32) + member.Level + Convert.ToChar(32) + client.IsOnline(member.Name);
                    }
                    else if (entry.Rank == GuildRank.DeputyLeader)
                    {
                        if (client.IsOnline(member.Name) == 1)
                        {
                            OnDeputies.Add(member.Name + Convert.ToChar(32) + member.Level + Convert.ToChar(32) + "1");
                        }
                        else
                        {
                            OffDeputies.Add(member.Name + Convert.ToChar(32) + member.Level + Convert.ToChar(32) + "0");
                        }
                    }
                    else
                    {
                        if (client.IsOnline(member.Name) == 1)
                        {
                            OnMembers.Add(member.Name + Convert.ToChar(32) + member.Level + Convert.ToChar(32) + "1");
                        }
                        else
                            OffMembers.Add(member.Name + Convert.ToChar(32) + member.Level + Convert.ToChar(32) + "0");
                    }

                }

                if (GL.EndsWith("1"))
                    packet.Strings.AddString(GL);

                foreach (var dep in OnDeputies)
                {
                    packet.Strings.AddString(dep);
                }

                foreach (var mem in OnMembers)
                {
                    packet.Strings.AddString(mem);
                }

                if (GL.EndsWith("0"))
                    packet.Strings.AddString(GL);

                foreach (var dep in OffDeputies)
                {
                    packet.Strings.AddString(dep);
                }

                foreach (var mem in OffMembers)
                {
                    packet.Strings.AddString(mem);
                }
                    
                client.Send(packet);
            }
            public string StringInfo
            {
                get { return string.Format(Name);}//("{0} {1} {2} {3}", Name, LeaderName, Level, Amount); }
            }

            public void BroadcastLocalGuildMsg(byte[] buffer, Player excludeSender = null)
            {
                if (buffer == null || buffer.Length == 0) return;

                foreach (var user in PlayerManager.Select(u => u != null && u.GuildId == Id && !(excludeSender != null && excludeSender.UID == u.UID)))
                {
                    //user.Send(buffer.UnsafeClone());
                    user.Send(new TalkPacket(ChatType.SynAnnounce, user.Guild.Announce));
                }
            }
            public List<Player> Members()
            {
                //? return new List<Player>(UserManager.Select(u => u != null && u.GuildId == Id));
                List<Player> users = new List<Player>();
                foreach (var user in PlayerManager.Select(u => u != null && u.GuildId == Id))
                    users.Add(user);

                return users;
            }
            public void BroadcastGuildMsg(byte[] buffer, Player excludeSender = null)
            {
                var master = MasterGuild;
                if (master == null) return;
                master.BroadcastLocalGuildMsg(buffer, excludeSender);

                var masterId = master.Id;
                if (masterId == 0) return;
                foreach (var guild in GuildManager.Select(g => g != null && g.FealtyGuild == masterId))
                {
                    foreach (var subGuild in GuildManager.Select(g => g != null && g.FealtyGuild == guild.Id))
                    {
                        subGuild.BroadcastLocalGuildMsg(buffer, excludeSender);
                    }

                    guild.BroadcastLocalGuildMsg(buffer);
                }

                //if (buffer == null || buffer.Length == 0) return;

                //for (var i = 0; i < Amount; i++)
                //{
                //    var info = _members[i];
                //    var member = UserManager.GetUser(info.Name);
                //    if (member != null && member.GuildId == Id
                //        && !(excludeSender != null && excludeSender.Id == member.Id))
                //    {
                //        member.Send(buffer.UnsafeClone());
                //    }
                //}
            }

            public void BroadcastGuildMsg(string msg, Player excludeSender = null, string emotion = "", uint color = 0xffffffu, ChatType type = ChatType.Syndicate)
            {
                BroadcastGuildMsg(new TalkPacket("SYSTEM", "ALLUSERS", msg, emotion, color, type), excludeSender);
            }

            public void BroadcastMessage(string message)
            {
                foreach (var member in Members())
                {
                    member.Send(new TalkPacket(ChatType.Syndicate, message));
                }
            }
            public void SendInfoToClient(Player client)
            {
                if (client == null) return;

                SendEnemyList(client);
                SendAllyList(client);

                var guildId = Id;
                client.Send(GuildPackets.Create(GuildAction.SetSyndicate, guildId, FealtyGuild));

                // TODO?
            }

            public bool IsAllied(uint targetId, bool nest = true)
            {
                if (targetId == 0) return false;

                var target = GuildManager.GetGuild(targetId);
                if (target == null) return false;
                targetId = target.MasterGuildId;

                if (nest && FealtyGuild != 0)
                {
                    var guild = GuildManager.GetGuild(FealtyGuild);
                    if (guild != null) return guild.IsAllied(targetId);
                }

                for (var i = 0; i < 5; i++)
                {
                    if (GetAlly(i) == targetId)
                        return true;
                }

                return false;
            }

            public bool IsEnemied(uint targetId, bool nest = true)
            {
                if (targetId == 0) return false;

                var target = GuildManager.GetGuild(targetId);
                if (target == null) return false;
                targetId = target.MasterGuildId;

                if (nest && FealtyGuild != 0)
                {
                    var guild = GuildManager.GetGuild(FealtyGuild);
                    if (guild != null) return guild.IsEnemied(targetId);
                }

                for (var i = 0; i < 5; i++)
                {
                    if (GetEnemy(i) == targetId)
                        return true;
                }

                return false;
            }

            public bool IsNeutral(uint targetId)
            {
                if (!IsAllied(targetId) && !IsEnemied(targetId))
                    return true;

                return false;
            }

            public string GetRankString(GuildRank rank)
            {
                const string FORMAT = "{0:d} {1} {2} {3} {4}";
                switch (rank)
                {
                    case GuildRank.GuildLeader: return string.Format(FORMAT, rank, 1, 1, 0, 0);
                    case GuildRank.DeputyLeader: return string.Format(FORMAT, rank, DeputyLeaders, 5, 0, 0);
                    default: return string.Format(FORMAT, rank, 0, 0, 0, 0);
                }
            }

            public bool AddMemberList(string name, GuildRank rank, byte level)
            {
                _members.Add(new MemberInfo { Name = name, Rank = rank, Level = level });
                return true;
            }

            public bool RemoveMemberList(string name)
            {
                for (var i = 0; i < _members.Count; i++)
                {
                    if (_members[i].Name == name)
                    {
                        _members.RemoveAt(i);
                        return true;
                    }
                }

                return false;
            }

            public bool EditMemberList(string name, GuildRank rank)
            {
                for (var i = 0; i < _members.Count; i++)
                {
                    if (_members[i].Name == name)
                    {
                        var member = _members[i];

                        if (member.Rank != GuildRank.DeputyLeader && rank == GuildRank.DeputyLeader) DeputyLeaders++;
                        else if (member.Rank == GuildRank.DeputyLeader && rank != GuildRank.DeputyLeader) DeputyLeaders--;

                        member.Rank = rank;
                        _members[i] = member;
                        return true;
                    }
                }

                return false;
            }

            public bool EditMemberList(string name, int totalDonation)
            {
                for (var i = 0; i < _members.Count; i++)
                {
                    if (_members[i].Name == name)
                    {
                        var member = _members[i];
                        member.TotalDonation = totalDonation;
                        _members[i] = member;
                        return true;
                    }
                }

                return false;
            }

            public uint GetAlly(int index)
            {
                if (index < 0 || index >= 5) return 0;
                return _allies[index];
            }

            public void SetAlly(int index, uint targetId, bool update = true)
            {
                _allies[index] = targetId;

                if (update)
                {
                    Ally0 = _allies[0];
                    Ally1 = _allies[1];
                    Ally2 = _allies[2];
                    Ally3 = _allies[3];
                    Ally4 = _allies[4];
                    SaveInfo();
                }

                SynchroInfo();

                if (targetId != 0)
                {
                    BroadcastGuildMsg(GuildPackets.Create(GuildAction.SetAlly, targetId, 0));
                }
            }

            public uint GetEnemy(int index)
            {
                if (index < 0 || index >= 5) return 0;
                return _enemies[index];
            }

            public void SetEnemy(int index, uint targetId, bool update = true)
            {
                _enemies[index] = targetId;

                if (update)
                {
                    Enemy0 = _enemies[0];
                    Enemy1 = _enemies[1];
                    Enemy2 = _enemies[2];
                    Enemy3 = _enemies[3];
                    Enemy4 = _enemies[4];
                    SaveInfo();
                }

                SynchroInfo();

                if (targetId != 0)
                {
                    BroadcastGuildMsg(GuildPackets.Create(GuildAction.SetEnemy, targetId, 0));
                }
                
            }

            

           

            public void SetAnnounce(string announce, bool update = true)
            {
                if (string.IsNullOrEmpty(announce)) return;

                Announce = announce;
                AnnounceDate = (uint)Common.Clock;

                //var msg = GuildPackets.Create(GuildAction.SetAnnounce, AnnounceDate, 0);
                //msg.Strings.AddString(announce);
                BroadcastGuildMsg(announce);

                if (update)
                {
                    SaveInfo();
                }
            }

            public void SynchroInfo()
            {
                var master = MasterGuild;
                if (master == null) return;

                foreach (var user in PlayerManager.Select(p => p != null && p.GuildId == master.Id))
                {
                    user.GuildAttribute.SendInfoToClient();
                }

                // TODO: branches
            }

            public bool ApplyKickoutMember(Player leader, Player target)
            {
                var leaderGuildId = leader.GuildId;
                var targetGuildId = target.GuildId;
                if (leaderGuildId != targetGuildId)
                    return false;

                var rank = leader.GuildRank;

                if (rank != GuildRank.GuildLeader)
                    return false;

                var guild = leader.Guild;
                if (guild == null) return false;

                return true;
            }

            public void SendMemberList(Player client, int startIndex)
            {
                if (startIndex < 0 || startIndex >= _members.Count) return;

                GuildMemberPacket msg;
                if (GuildMemberPacket.Create(out msg))
                {
                    var count = 0;
                    for (var i = startIndex; i < Amount && count < 12; i++)
                    {
                        var member = _members[i];
                        var info = new GuildMemberInfo
                        {
                            Name = member.Name,
                            Rank = member.Rank,
                           Donation = member.TotalDonation
                        };
                        msg.AddInfo(info);
                        count++;
                    }

                    client.Send(msg);
                }
            }

            public void SendAllyList(Player client)
            {
                for (var i = 0; i < 5; i++)
                {
                    var targetGuild = GuildManager.GetGuild(_allies[i]);
                    if (targetGuild == null) continue;

                    targetGuild = targetGuild.MasterGuild;
                    if (targetGuild != null)
                    {
                        client.Send(StringsPacket.Create(targetGuild.Id, StringAction.SetAlly, targetGuild.StringInfo));
                    }
                }
            }

            public void SendEnemyList(Player client)
            {
                for (var i = 0; i < 5; i++)
                {
                    var targetGuild = GuildManager.GetGuild(_enemies[i]);
                    if (targetGuild == null) continue;

                    targetGuild = targetGuild.MasterGuild;
                    if (targetGuild != null)
                    {
                        client.Send(StringsPacket.Create(targetGuild.Id, StringAction.SetEnemy, targetGuild.StringInfo));
                    }
                }
            }

            /// <summary>
            /// Determines whether a member with <paramref name="rank1"/> is able to kick a member with <paramref name="rank2"/>.
            /// </summary>
            /// <param name="rank1">Rank of the kicker</param>
            /// <param name="rank2">Rank of the member being kicked</param>
            /// <returns>Returns whether <paramref name="rank1"/> can kick <paramref name="rank2"/></returns>
            public static bool CanKickout(GuildRank rank1, GuildRank rank2)
            {
                if (rank1 == GuildRank.GuildLeader) return true;

                return false;
            }
        }
    }

