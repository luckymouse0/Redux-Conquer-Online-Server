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
    /// Handles NPC usage for [42] Warden
    /// </summary>
    public class NPC_42 : INpc
    {

        public NPC_42(Game_Server.Player _client)
            :base (_client)
    	{
    		ID = 42;	
			Face = 54;    
    	}

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("You cannot leave here unless you have paid for your crimes.");
                    AddOption("Ok! Let me get out from here!", 1);
                    AddOption("Any chance of a discount?", 2);
                    AddOption("I would rather rot in hell!", 255);
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
