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

namespace Redux.Npcs
{
    /// <summary>
    /// Handles NPC usage for [43] CaptainLi
    /// Written by Aceking 9-24-13
    /// </summary>
    public class NPC_43 : INpc
    {

        public NPC_43(Game_Server.Player _client)
            :base (_client)
    	{
    		ID = 43;	
			Face = 37;    
    	}

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("What can I do for you?");
                    AddOption("Visit the jail.", 1);
                    AddOption("Just passing by.", 255);
                    break;
                case 1:
                    AddText("Give me 1000 silver, I will teleport you there");
                    AddOption("Here are 1000 silver", 2);
                    AddOption("If so, I will stay here.", 255);
                    break;
                case 2:
                    if (_client.Money >= 1000)
                    { _client.ChangeMap(6000, 29, 72); _client.Money -= 1000; }
                    else
                    {
                        AddText("Sorry, you do not have enough.");
                        AddOption("I see.", 255);
                    }
                    break;
            }
            AddFinish();
            Send();
        }
    }
}
