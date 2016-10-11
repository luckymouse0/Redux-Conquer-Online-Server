/*
 * User: pro4never
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
    /// Handles NPC usage for [380] Guild Conductress
    /// </summary>
    public class NPC_380 : INpc
    {

        public NPC_380(Game_Server.Player _client)
            :base (_client)
    	{
    		ID = 380;	
			Face = 123;    
    	}

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("What can I do for you?");
                    AddOption("Enter the guild area.", 1);
                    AddOption("Just passing by.", 255);
                    break;
                case 1:
                    _client.ChangeMap(1038, 350, 339);
                    break;
            }
            AddFinish();
            Send();
        }
    }
}
