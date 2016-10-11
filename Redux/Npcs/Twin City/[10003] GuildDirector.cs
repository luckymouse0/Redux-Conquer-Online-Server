/*
 * User: cookc
 * Date: 9/21/2013
 * Time: 8:08 PM
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;
using Redux.Managers;
using Redux.Game_Server;
using Redux.Space;

namespace Redux.Npcs
{
    /// <summary>
    /// Handles NPC usage for [10003] GuildDirector
    /// Written by Aceking 10-06-13
    /// </summary>
    public class NPC_10003 : INpc
    {

        public NPC_10003(Game_Server.Player _client)
            :base (_client)
    	{
    		ID = 10003;	
			Face = 7;    
    	}

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    if (_client.GuildId == 0)
                    {
                        AddText("Greetings! I am the guild director, in charge of administrating and managing");
                        AddText(" guilds. What business do you have with me?");
                        AddOption("Create a guild", 1);
                        AddOption("Inquire about a guild.", 3);
                    }
                    else if (_client.GuildRank == Enum.GuildRank.GuildLeader)
                    {
                        AddText("Greetings! I am the guild director, in charge of administrating and managing");
                        AddText(" guilds. What business do you have with me?");
                        AddOption("Add Enemy", 14);
                        AddOption("Add Ally", 15);
                        AddOption("Remove Enemy", 16);
                        AddOption("Remove Ally", 17);
                        AddOption("Promote Member", 18);
                        AddOption("Demote Member", 19);
                        AddOption("Disband Guild", 20);
                        AddOption("More Options", 21);
                       // AddOption("Inquire about a guild.", 3);
                    }
                    else
                    {
                        AddText("Greetings! I am the guild director, in charge of administrating and managing");
                        AddText(" guilds. What business do you have with me?");
                        AddOption("Inquire about a guild.", 3);
                    }
                    break;

                case 1:
                    if (_client.Level >= 90)
                    {
                        AddText("Please enter the name for your guild");
                        AddInput("Name", 6);
                    }
                    else
                    {
                        AddText("Sorry, you cannot create a guild before you reach level 90. Please train");
                        AddText(" harder.");
                        AddOption("I see.", 255);
                    }
                    break;

                case 2:
                    {
                        AddText("Disbanding your guild is a decision not to be taken lightly. When armies fall");
                        AddText(" apart, a period of chaos ensues?");
                        AddOption("I decide to disband.", 4);
                        AddOption("I changed my mind.", 255);
                    }
                    break;

                case 3:

                   AddText("Which guild would you like to inquire about?");
                    AddOption("Let me think it over", 255);
                    break;

                case 4:

                    _client.GuildAttribute.DisbandGuild();
                    break;

                case 6:

                    if (_client.Money < 1000000)
                    {
                        AddText("I'm sorry, players must have at least 1 million gold to create a guild");
                        AddOption("I'll come back", 255);
                    }

                    else
                    {
                        bool GuildCheck = true;
                        var GuildList = Database.ServerDatabase.Context.Guilds.GetAll();
                        foreach (var guild in GuildList)
                            if (guild.Name.ToLower() == _client.NpcInputBox.ToLower())
                            { GuildCheck = false; break; }
                        
                        if (GuildCheck == true)
                        {
                            if (_client.GuildAttribute.CreateGuild(_client.NpcInputBox, 90, 1000000, 500000))
                            {
                                AddText("I have set up your guild successfully!");
                                AddOption("Thanks!", 255);
                            }
                        }
                        else
                        {
                            AddText("There was an error setting up your guild. The name may be taken or you may not pass the requirements");
                            AddOption("I'll try again", 255);
                        }
                    }
                    break;

                case 14://Add Enemy
                    AddText("Please enter the name for the guild you wish to enemy");
                    AddInput("Enemy Name", 7);

                    break;

                case 7://Confirm Enemy
                    String guildname = _client.NpcInputBox;
                    
                    var targetguild = GuildManager.GetGuildByName(guildname);
                    if (targetguild == null)
                    {
                        AddText("This guild name does not exist.");
                        AddOption("Thanks.", 255);
                        break;
                    }

                    _client.GuildAttribute.AddEnemy(guildname);

                    AddText("And the war begins...");
                    AddOption("We will win!", 255);
                    break;

                case 15://Add Ally
                    AddText("Please enter the name for the guild you wish to ally");
                    AddInput("Ally Name", 9);
                    break;

                case 9://Confirm Ally
                    String allyguild = _client.NpcInputBox;
                    
                    var guildtorequest = GuildManager.GetGuildByName(allyguild);
                    if (guildtorequest == null)
                    {
                        AddText("This guild name does not exist.");
                        AddOption("Thanks.", 255);
                        break;
                    }

                    _client.GuildAttribute.AddAlly(allyguild);

                    
                    break;
                case 16://Remove Enemy
                    
                    AddText("Choose the enemy you wish to make peace with...");
                    AddInput("Make peace with...", 8);
                    break;

                case 8://Confirm remove enemy
                    var enemyguild = GuildManager.GetGuildByName(_client.NpcInputBox);
                    
                    var success = _client.GuildAttribute.RemoveEnemy(_client.NpcInputBox);
                    if (success)
                    {
                        foreach (var member in _client.Guild.Members())
                        {
                            member.Send(GuildPackets.Create(Enum.GuildAction.ClearEnemy, enemyguild.Id, enemyguild.MasterGuildId));
                            member.Guild.SendInfoToClient(member);
                        }
                        AddText("And the land knows peace once again.");
                        AddOption("Thank you.", 255);
                    }
                    else
                    {
                        AddText("You are not enemied with this guild.");
                        AddOption("My mistake.", 255);
                    }
                    break;

                case 17://Remove Ally
                    AddText("Choose the Ally you wish to remove from your Allies");
                    AddInput("Remove...", 10);
                    break;

                case 10://Confirm Remove Ally
                    String allyname = _client.NpcInputBox;
                    
                    var alliedguild = GuildManager.GetGuildByName(allyname);
                    if (alliedguild == null)
                    {
                        AddText("This guild name does not exist.");
                        AddOption("Thanks.", 255);
                        break;
                    }

                    _client.GuildAttribute.RemoveAlly(allyname);

                    AddText("And so it is...");
                    AddOption("Thanks.", 255);
                    break;
                case 18://Promote
                    AddText("What is the name of the player you wish to promote?");
                    AddInput("Their name is...", 11);
                    break;

                case 11://Confirm Promote
                    String name = _client.NpcInputBox;

                    if (_client.GuildRank != Enum.GuildRank.GuildLeader)
                    {
                        AddText("You are not the Guild Leader.");
                        AddOption("Thanks.", 255);
                        break;
                    }

                    if (_client.GuildAttribute.PromoteMember(name, Enum.GuildRank.DeputyLeader) == true)
                    {
                        AddText("They have been promoted.");
                        AddOption("Thank you.", 255);
                    }
                    else
                    {
                        AddText("They could not be promoted...");
                        AddOption("Thanks anyway", 255);
                    }

                    break;
                case 19://Demote
                    AddText("What is the name of the player you wish to demote?");
                    AddInput("Their name is...", 12);
                    break;

                case 12://Confirm Demote
                    string demotename = _client.NpcInputBox;

                    if (_client.GuildRank != Enum.GuildRank.GuildLeader)
                    {
                        AddText("You are not the Guild Leader.");
                        AddOption("Thanks.", 255);
                        break;
                    }

                    if (_client.GuildAttribute.DemoteMember(demotename, Enum.GuildRank.Member) == true)
                    {
                        AddText("They have been demoted.");
                        AddOption("Thank you.", 255);
                    }
                    else
                    {
                        AddText("They could not be demoted...");
                        AddOption("Thanks anyway", 255);
                    }
                    break;
                case 20://Disband
                    AddText("Are you sure you wish to disband?");
                    AddOption("I am sure.", 13);
                    break;

                case 13://Confirm Disband
                    if (_client.GuildRank != Enum.GuildRank.GuildLeader)
                    {
                        AddText("You are not the Guild Leader");
                        AddOption("Oops.", 13);
                    }
                    else
                        _client.GuildAttribute.DisbandGuild();
                    break;
                case 21://More Options
                    //TODO    AddOption("Create branch", 22);
                    //TODO    AddOption("Destroy branch", 23);
                   
                    AddOption("Back...", 0);


                   break;
                
            }
            AddFinish();
            Send();
        }
    }
}
