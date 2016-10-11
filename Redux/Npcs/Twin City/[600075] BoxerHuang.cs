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
    /// Handles NPC usage for [600075] BoxerHuang
    /// Written by Aceking 9-24-13
    /// </summary>
    public class NPC_600075 : INpc
    {

        public NPC_600075(Game_Server.Player _client)
            :base (_client)
    	{
    		ID = 600075;	
			Face = 20;    
    	}

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("Although nothing will be consumed here, you cannot level up as fast as");
                    AddText(" killing the monsters. Shall I teleport you out?");
                    AddOption("Yeah, please.", 1);
                    AddOption("No, thanks.", 255);
                    break;
                case 1:                    
                    _client.ChangeMap((ushort)_client.Character.Map);
                    break;
            }
            AddFinish();
            Send();
        }
    }
}
