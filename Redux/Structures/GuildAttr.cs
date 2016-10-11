using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Enum;
using Redux.Managers;
using Redux.Database;
using Redux.Database.Domain;
using Redux.Structures;
using Redux.Game_Server;
using Redux.Packets.Game;

namespace Redux.Structures
{
    public struct CreateGuildInfo
        {
            public uint GuildId;
            public string Name;
            public uint LeaderId;
            public string LeaderName;
            public int RequiredLevel;
            public int RequiredMetempsychosis;
            public int RequiredProfession;
            public byte Level;
            public long Money;
            public bool DeleteFlag;
            public uint Amount;
        }

        public struct GuildAttrInfoStruct
        {
            public uint Id;
            public uint GuildId;
            public GuildRank Rank;
            public int SilverDonation;
            public int PkDonation;
            public int CPDonation;
            public int ArsenalDonation;
            public int GuideDonation;
            public uint JoinDate;
        }

        public class GuildAttr
        {
            private readonly Player _client;
            private DbGuildAttr _data;

            public uint Id { get { return _data != null ? _data.Id : 0; } set { if (_data != null) _data.Id = value; } }
            public uint GuildId { get { return _data != null ? _data.GuildId : 0; } set { if (_data != null) _data.GuildId = value; } }
            public GuildRank Rank { get { return _data != null ? _data.Rank : 0; } set { if (_data != null) _data.Rank = value; } }
            public int SilverDonation { get { return _data != null ? _data.SilverDonation : 0; } set { if (_data != null) _data.SilverDonation = value; } }
            
            public GuildAttr(Player client)
            {
                _client = client;
                _data = ServerDatabase.Context.GuildAttributes.GetGuildId(_client.UID);
            }

            public bool Create()
            {
                if (_data != null) return false;
                if (_client == null) return false;

                _data = ServerDatabase.Context.GuildAttributes.GetById(_client.UID);
                if (_data == null) return true;

                if (GuildManager.GetGuild(GuildId) == null)
                {
                    _data = null;
                    return true;
                }

                return true;
            }

            public bool SendInfoToClient()
            {
                var info = GetInfo();

                var guild = GuildManager.GetGuild(GuildId);
                if (guild != null) guild = guild.MasterGuild;
                
                _client.Send(GuildAttrInfoPacket.Create(info, guild));

                if (guild != null)
                {
                    var msgAnnounce = Redux.Packets.Game.GuildPackets.Create(GuildAction.SetAnnounce, guild.AnnounceDate, 0);
                    msgAnnounce.Strings.AddString(guild.Announce);
                    _client.Send(msgAnnounce);

                    guild.SendAllyList(_client);
                    guild.SendEnemyList(_client);
                }

                return true;
            }

            public bool SendDonationInfoToClient()
            {
                var info = GetInfo();

                _client.Send(GuildDonationPacket.Create(info));

                return true;
            }

            public bool SendPromotionInfoToClient()
            {
                if (GuildId == 0) return false;

                var guild = GuildManager.GetGuild(GuildId);
                if (guild == null) return false;

                var msg = GuildPackets.Create(GuildAction.PromoteInfo, 0, 0);
                msg.Strings.AddString(guild.GetRankString(GuildRank.GuildLeader));
                msg.Strings.AddString(guild.GetRankString(GuildRank.DeputyLeader));
                //msg.Strings.AddString("980 0 1 0 650");
                //msg.Strings.AddString("880 0 1 0 320");
                //msg.Strings.AddString("840 0 1 0 270");
                //msg.Strings.AddString("680 0 1 0 100");
                //msg.Strings.AddString("602 0 0 0 0");
                _client.Send(msg);

                return true;
            }

            public void SaveInfo()
            {
                if (_data == null) return;
                ServerDatabase.Context.GuildAttributes.AddOrUpdate(_data);
            }

            public GuildAttrInfoStruct GetInfo()
            {
                var info = new GuildAttrInfoStruct { Id = _client.UID };

                if (_data != null)
                {
                    info.GuildId = _data.GuildId;
                    info.Rank = _data.Rank;
                    info.SilverDonation = _data.SilverDonation;
                    info.JoinDate = _data.JoinDate;
                }

                return info;
            }

            public bool SetIdRank(uint guildId, GuildRank rank, bool synchro = true)
            {
                _data.GuildId = guildId;
                _data.Rank = rank;
                SaveInfo();

                if (synchro) SynchroInfo();
                return true;
            }

            public bool SetRank(GuildRank rank, bool leader = false)
            {
                if (!(rank != GuildRank.GuildLeader && _data.Rank != GuildRank.GuildLeader || leader))
                    return false;

                var guild = GuildManager.GetGuild(GuildId);
                if (guild == null) return false;

                _data.Rank = rank;
                SaveInfo();

                if (!guild.EditMemberList(_client.Name, rank))
                    return false;

                SynchroInfo();
                return true;
            }

            public bool CreateGuild(string name, byte level, uint money, int moneyLeave)
            {
                if (_data != null) return false;

                if (_client.Level < level)
                {
                    
                    _client.SendSysMessage("You have not reached level " + level);
                    return false;
                }

                if (_client.Money < money)
                {
                    _client.SendSysMessage("You don't have " + money + " silvers!");
                    return false;
                }

                var info = new CreateGuildInfo
                {
                    GuildId = 0,
                    Name = name,
                    LeaderId = _client.UID,
                    LeaderName = _client.Name,
                    RequiredLevel = 1,
                    RequiredMetempsychosis = 0,
                    RequiredProfession = 0,
                    Level = 1,
                    Money = moneyLeave
                };

                var guildId = GuildManager.CreateGuild(info);
                if (guildId == 0)
                {
                    _client.SendSysMessage("You are forbidden to use this guild name.");
                    return false;
                }

                JoinGuild(guildId, GuildRank.GuildLeader);
                if (_data == null) return false;

                PlayerManager.SendToServer(new TalkPacket(ChatType.GM, string.Format("Congratulations! {0} has set up {1} successfully.", _client.Name, name)));

                _client.Money -= money;
                return true;
            }

            public bool DisbandGuild()
            {
                var guildId = GuildId;
                if (guildId == 0) return false;

                if (Rank != GuildRank.GuildLeader)
                {
                    _client.SendSysMessage("You have not been authorized!");
                    return false;
                }

                uint masterGuildId = 0;
                var guild = GuildManager.GetGuild(guildId);
                if (guild != null) masterGuildId = guild.FealtyGuild;

                GuildManager.DestroyGuild(guildId, masterGuildId);
                return true;
            }

            public bool JoinGuild(uint id, GuildRank rank = GuildRank.Member, int donation = 0)
            {
                if (_data != null) return false;

                _data = new DbGuildAttr
                {
                    Id = _client.UID,
                    GuildId = id,
                    Rank = rank,
                    JoinDate = (uint)Common.Clock
                };
                ServerDatabase.Context.GuildAttributes.AddOrUpdate(_data);

                var guild = GuildManager.GetGuild(GuildId);
                if (guild != null)
                {
                    guild.Amount++;
                    guild.SaveInfo();
                }

                AddMemberList(id, _client.Name, Rank, _client.Level);

                SynchroInfo(true);
                if (guild != null)
                {
                    guild.SendInfoToClient(_client);
                }

                return true;
            }

            public bool AddMemberList(uint id, string name, GuildRank rank, byte level)
            {
                var guild = GuildManager.GetGuild(id);
                if (guild == null) return false;

                return guild.AddMemberList(name, rank, level);
            }

            public void SynchroInfo(bool announce = false)
            {
                var info = GetInfo();
                var guild = GuildManager.GetGuild(GuildId);

                _client.Send(GuildAttrInfoPacket.Create(info, guild));
                //_client.Send(GuildDonationPacket.Create(info));

                if (guild != null)
                {
                    guild.SendAllyList(_client);
                    guild.SendEnemyList(_client);
                }

                _client.SendToScreen(Packets.Game.SpawnEntityPacket.Create(_client), true);


                if (announce)
                {
                    string ann = string.Empty;
                    if (guild != null)
                    {
                        var master = guild.MasterGuild;
                        ann = master.Announce;
                    }
                    _client.SendMessage(ann);
                    
                }
            }

            public bool LeaveGuild(bool deleteRecord = true, bool synchro = true)
            {
                if (_data == null) return false;

                var oldGuildId = GuildId;

                if (deleteRecord) ServerDatabase.Context.GuildAttributes.Remove(_data);
                _data = null;

                if (synchro)
                {
                    var guild = GuildManager.GetGuild(oldGuildId);
                    if (guild != null && guild.Amount > 0)
                    {
                        guild.Amount--;
                        guild.RemoveMemberList(_client.Name);
                    }

                    SynchroInfo();
                }

                return true;
            }

            public bool KickoutMember(string target)
            {
                if (string.IsNullOrEmpty(target)) return false;

                var guildId = GuildId;

                var user = Database.ServerDatabase.Context.Characters.GetByName(target);
                ServerDatabase.Context.GuildAttributes.DeleteGuildAttr(user.UID, guildId, GuildRank.GuildLeader);
                
                var oldGuildId = GuildId;
                var guild = GuildManager.GetGuild(oldGuildId);
                if (guild != null && guild.Amount > 0)
                {
                    guild.Amount--;
                    guild.RemoveMemberList(target);
                }

                return true;
            }
            public bool BoostDonateMoney(uint money)
            {
                if (_data == null) return false;
                if (money <= 0) return false;

                var guild = GuildManager.GetGuild(GuildId);
                if (guild == null) return false;

                guild = guild.MasterGuild;
                if (guild == null) return false;

                guild.Money += money;
                AddSilverDonation(money);
                guild.EditMemberList(_client.Name, SilverDonation);

                SynchroInfo();
                return true;
            }
            public bool DonateMoney(uint money)
            {
                if (_data == null) return false;
                if (money <= 0) return false;

                var guild = GuildManager.GetGuild(GuildId);
                if (guild == null) return false;

                guild = guild.MasterGuild;
                if (guild == null) return false;

                if (_client.Money < money)
                {
                    _client.SendSysMessage("You do not have enough money.");
                    return false;
                }

                guild.Money += money;
                _client.Money -= money;
                AddSilverDonation(money);
                guild.EditMemberList(_client.Name, SilverDonation);

                SynchroInfo();
                return true;
            }

      

            public bool AddSilverDonation(uint money)
            {
                SilverDonation += (int)money;// / 10000;
                return true;
            }

            public bool AddAlly(string name)
            {
                if (string.IsNullOrEmpty(name)) return false;

                var guildId = GuildId;
                if (guildId == 0) return false;

                var guild = GuildManager.GetGuild(guildId);
                if (guild == null) return false;

                if (Rank != GuildRank.GuildLeader)
                    return false;

                var targetGuild = GuildManager.GetGuildByName(name);
                if (targetGuild == null || targetGuild.Id == guildId)
                    return false;
                targetGuild = targetGuild.MasterGuild;

                if (guild.IsEnemied(targetGuild.Id))
                {
                    _client.SendSysMessage("You must make peace with Guild " + name + " before you ally.");
                    return false;
                }

                var targetLeader = PlayerManager.GetUser(targetGuild.LeaderId);
                if (targetLeader == null)
                {
                    _client.SendSysMessage("Target guild`s leader is offline!");
                    return false;
                }

                if (!targetLeader.FetchApply(ApplyType.AllyGuild, _client.UID))
                {
                    _client.SetApply(ApplyType.AllyGuild, targetLeader.UID);
                    var msgApply = GuildPackets.Create(GuildAction.Unknown16, guildId, 0);
                    msgApply.Strings.AddString(guild.Name);
                    targetLeader.Send(msgApply);
                    return true;
                }

                var index1 = -1;
                for (var i = 0; i < 5; i++)
                {
                    var targetId = guild.GetAlly(i);
                    if (targetId == 0 || GuildManager.GetGuild(targetId) == null || targetId == targetGuild.Id)
                        index1 = i;
                }

                if (index1 == -1)
                {
                    _client.SendSysMessage("Allies List is full!");
                    return false;
                }

                var index2 = -1;
                for (var i = 0; i < 5; i++)
                {
                    var targetId = targetGuild.GetAlly(i);
                    if (targetId == 0 || GuildManager.GetGuild(targetId) == null || targetId == guild.Id)
                        index2 = i;
                }

                if (index2 == -1)
                {
                    _client.SendSysMessage("The target's Ally List is full!");
                    return false;
                }

                guild.SetAlly(index1, targetGuild.Id);
                targetGuild.SetAlly(index2, guild.Id);

                guild.BroadcastGuildMsg(GuildPackets.Create(GuildAction.SetAlly, targetGuild.Id, 0));
                targetGuild.BroadcastGuildMsg(GuildPackets.Create(GuildAction.SetAlly, guild.Id, 0));

                // update allies page
                //guild.SynchroInfo();
                //targetGuild.SynchroInfo();

                // broadcast guild message
                guild.BroadcastMessage(string.Format("Guild Leader {0} has added Guild {1} to the allies list!", guild.LeaderName, targetGuild.Name));
                targetGuild.BroadcastMessage(string.Format("Guild Leader {0} has added Guild {1} to the allies list!", targetGuild.LeaderName, guild.Name));

                return true;
            }

            public bool AddEnemy(string name)
            {
                if (string.IsNullOrEmpty(name)) return false;

                var guildId = GuildId;
                if (guildId == 0) return false;

                var guild = GuildManager.GetGuild(guildId);
                if (guild == null) return false;

                if (Rank != GuildRank.GuildLeader)
                    return false;

                var targetGuild = GuildManager.GetGuildByName(name);
                if (targetGuild == null || targetGuild.Id == guildId)
                    return false;
                targetGuild = targetGuild.MasterGuild;

                if (guild.IsAllied(targetGuild.Id))
                {
                    _client.SendSysMessage("You must unally Guild " + name + " before you enemy.");
                    return false;
                }

                // check if target guild is enemy already
                for (var i = 0; i < 5; i++)
                {
                    var targetId = guild.GetEnemy(i);
                    if (targetId != 0 && targetId == targetGuild.Id)
                        return false;
                }

                for (var i = 0; i < 5; i++)
                {
                    var targetId = guild.GetEnemy(i);
                    if (targetId == 0 || GuildManager.GetGuild(targetId) == null)
                    {
                        guild.SetEnemy(i, targetGuild.Id);

                        guild.BroadcastMessage(string.Format("Guild Leader {0} has added Guild {1} to the enemy list!", guild.LeaderName, targetGuild.Name));
                        targetGuild.BroadcastMessage(string.Format("Guild {1}`s Guild Leader {0} has added our Guild to the enemy list!", guild.LeaderName, guild.Name));
                        break;
                    }
                }

                return true;
            }

            public bool RemoveEnemy(string name)
            {
                if (string.IsNullOrEmpty(name)) return false;

                var guildId = GuildId;
                if (guildId == 0) return false;

                var guild = GuildManager.GetGuild(guildId);
                if (guild == null) return false;

                if (Rank != GuildRank.GuildLeader)
                    return false;

                var targetGuild = GuildManager.GetGuildByName(name);
                if (targetGuild == null) return false;
                targetGuild = targetGuild.MasterGuild;

                for (var i = 0; i < 5; i++)
                {
                    var targetId = guild.GetEnemy(i);
                    if (targetId == targetGuild.Id)
                    {
                        guild.SetEnemy(i, 0);
                        guild.BroadcastGuildMsg(GuildPackets.Create(GuildAction.ClearEnemy, targetId, 0));

                        foreach (var member in guild.Members())
                        {

                        }
                        guild.BroadcastMessage(string.Format("Guild Leader {0} has removed Guild {1} from the enemy list!", guild.LeaderName, targetGuild.Name));
                        targetGuild.BroadcastMessage(string.Format("Guild {1}`s Guild Leader {0} has removed our Guild from the enemy list!", guild.LeaderName, guild.Name));
                        break;
                    }
                }

                return true;
            }

            public bool RemoveAlly(string name)
            {
                if (string.IsNullOrEmpty(name)) return false;

                var guildId = GuildId;
                if (guildId == 0) return false;

                var guild = GuildManager.GetGuild(guildId);
                if (guild == null) return false;

                if (Rank != GuildRank.GuildLeader)
                    return false;

                var targetGuild = GuildManager.GetGuildByName(name);
                if (targetGuild == null) return false;
                targetGuild = targetGuild.MasterGuild;

                for (var i = 0; i < 5; i++)
                {
                    var targetId = guild.GetAlly(i);
                    if (targetId == targetGuild.Id)
                    {
                        guild.SetAlly(i, 0);
                        guild.BroadcastGuildMsg(GuildPackets.Create(GuildAction.ClearAlly, targetId, 0));

                        foreach (var member in guild.Members())
                        {

                        }
                        guild.BroadcastMessage(string.Format("Guild Leader {0} has removed Guild {1} from the allied list!", guild.LeaderName, targetGuild.Name));
                        targetGuild.BroadcastMessage(string.Format("Guild {1}`s Guild Leader {0} has removed our Guild from the allied list!", guild.LeaderName, guild.Name));
                        
                    }

                    var targetguildId = targetGuild.GetAlly(i);
                    if (targetguildId == guild.Id)
                    {
                        targetGuild.SetAlly(i, 0);
                        targetGuild.BroadcastGuildMsg(GuildPackets.Create(GuildAction.ClearAlly, guild.Id, 0));

                    }
                }

                return true;
            }

            public bool PromoteMember(string name, GuildRank rank)
            {
                if (string.IsNullOrEmpty(name)) return false;

                var guildId = GuildId;
                if (guildId == 0) return false;

                var guild = GuildManager.GetGuild(guildId);
                if (guild == null) return false;

                if (_client.GuildRank != GuildRank.GuildLeader)
                    return false;

                if (guild.DeputyLeaders >= 5)
                {
                    _client.SendSysMessage("All the posts have been occupied!");
                    return false;
                }

                var target = PlayerManager.GetUser(name);
                if (target == null)
                {
                    _client.SendSysMessage("The player is offline!");
                    return false;
                }

                if (_client.UID == target.UID)
                    return false;

                var targetGuildId = target.GuildId;
                if (targetGuildId == 0 || targetGuildId != guildId)
                    return false;

                if (!target.GuildAttribute.SetRank(rank))
                    return false;

                guild.BroadcastMessage(string.Format("{0} has appointed {1} as Deputy Leader.", _client.Name, name, rank));
                guild.BroadcastGuildMsg(GuildPackets.Create(GuildAction.Unknown39, (ushort)rank, 0));

                return true;
            }

            public bool DemoteMember(string name, GuildRank rank = GuildRank.Member)
            {
                if (string.IsNullOrEmpty(name)) return false;

                var guildId = _client.GuildId;
                if (guildId == 0) return false;

                var guild = GuildManager.GetGuild(guildId);
                if (guild == null) return false;

                if (_client.GuildRank != GuildRank.GuildLeader)
                    return false;

                var target = PlayerManager.GetUser(name);
                if (target == null)
                {
                    _client.SendSysMessage("The player is offline!");
                    return false;
                }

                if (_client.UID == target.UID)
                    return false;

                var targetGuildId = target.GuildId;
                if (targetGuildId == 0 || targetGuildId != guildId)
                    return false;

                if (!target.GuildAttribute.SetRank(GuildRank.Member))
                    return false;

                guild.BroadcastMessage(string.Format("{0} was discharged from the Deputy Leader position.", name, GuildRank.DeputyLeader));
                guild.BroadcastGuildMsg(GuildPackets.Create(GuildAction.Unknown41, 990, 0));

                return true;
            }
        }
    }

