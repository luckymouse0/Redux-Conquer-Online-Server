/*
 * User: cookc
 * Date: 9/21/2013
 * Time: 8:14 PM
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;
using Redux.Game_Server;
using Redux.Managers;
namespace Redux.Npcs
{
    /// <summary>
    /// Handles NPC usage for [6701] LeftGate
    /// Written by Aceking 9-24-13
    /// </summary>
    public class NPC_6701 : INpc
    {

        public NPC_6701(Game_Server.Player _client)
            :base (_client)
    	{
            ID = 6701;	
			Face = 0;    
    	}

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    if (GuildWar.CurrentWinner == _client.Guild && GuildWar.CurrentWinner != null && (MapManager.PullMapByID(1038).Search<SOB>(6701)).Life > 1)
                    {
                        AddText("Your guild is currently Victorious. I can let you inside if you wish!");
                        AddOption("Let me in", 1);
                        AddOption("Open/Close the Gate", 2);
                        AddOption("No Thanks.", 255);
                    }
                    else if (GuildWar.CurrentWinner == _client.Guild && GuildWar.CurrentWinner != null && (MapManager.PullMapByID(1038).Search<SOB>(6701)).Life == 1)
                    {
                        AddText("Your guild is currently Victorious. I can repair the gate if you wish.");
                        AddOption("Repair", 3);
                        AddOption("No Thanks.", 255);
                    }
                    break;
                case 1:

                    if (GuildWar.CurrentWinner == _client.Guild && GuildWar.CurrentWinner != null)
                    {
                        _client.ChangeMap(1038, 162, 199);
                    }
                    break;
                case 2:
                    if (GuildWar.CurrentWinner == _client.Guild && GuildWar.CurrentWinner != null && _client.GuildRank == Enum.GuildRank.GuildLeader)
                        GuildWar.ToggleLeftGate();
                    else
                    {
                        AddText("Sorry, you are not able to perform this action.");
                        AddOption("Thanks", 255);
                    }
                    break;
                case 3:
                    if (GuildWar.CurrentWinner == _client.Guild && GuildWar.CurrentWinner != null && _client.GuildRank == Enum.GuildRank.GuildLeader)
                        GuildWar.RepairLeftGate();
                    else
                    {
                        AddText("Sorry, you are not able to perform this action.");
                        AddOption("Thanks", 255);
                    }
                    break;
            }
            AddFinish();
            Send();
        }
    }
}
