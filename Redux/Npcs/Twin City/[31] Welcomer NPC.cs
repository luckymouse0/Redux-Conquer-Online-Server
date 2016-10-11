using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{

    public class NPC_31 : INpc
    {

        public NPC_31(Game_Server.Player _client)
            : base(_client)
        {
    		ID = 31;	
			Face = 5;    
    	}
    	
        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
        	Responses = new List<NpcDialogPacket>();
        	AddAvatar();
        	switch(_linkback)
        	{
                case 0:
                    AddText("Welcome to Triumph CO! Please enjoy your stay");
                    AddOption("Thanks", 255);
                    break;
        		default:
        			break;
        	}
            AddFinish();
            Send();
        }
    }
}
