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
    /// Handles NPC usage for [6010] Guild War Warden
    /// Written by Aceking 9-24-13
    /// </summary>
    public class NPC_6010 : INpc
    {

        public NPC_6010(Game_Server.Player _client)
            :base (_client)
    	{
            ID = 6010;	
			Face = 67;    
    	}

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    if ((DateTime.Now.Minute == 0 || DateTime.Now.Minute == 30) && GuildWar.Running == true)
                    {
                        AddText("It is time for Pardon! I can let you out if ou wish");
                        AddText("");
                        AddOption("Let me out!", 1);
                        AddOption("No Thanks.", 255);
                    }
                    else if (GuildWar.Running == false)
                    {
                        AddText("It is time for Pardon! I can let you out if ou wish");
                        AddText("");
                        AddOption("Let me out!", 1);
                        AddOption("No Thanks.", 255);
                    }
                    else
                    {
                        AddText("It is not time for Pardon! There is a pardon every 30 minutes while Guild War is running.");
                        AddText("");
                        AddOption("I will wait", 255);
                    }

                    break;
                case 1:
                    _client.ChangeMap(1002, 430, 380);
                    break;
            }
            AddFinish();
            Send();
        }
    }
}
