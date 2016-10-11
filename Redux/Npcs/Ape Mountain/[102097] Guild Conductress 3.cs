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
    /// Handles NPC usage for [102097] Guild Conductress 3
    /// </summary>
    public class NPC_102097 : INpc
    {

        public NPC_102097(Game_Server.Player _client)
            :base (_client)
    	{
            ID = 102097;	
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
