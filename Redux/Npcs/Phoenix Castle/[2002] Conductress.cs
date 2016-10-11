using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{

    public class NPC_2002 : INpc
    {

        public NPC_2002(Game_Server.Player _client)
            :base (_client)
    	{
    		ID = 2002;	
			Face = 1;    
    	}

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("Where are you heading for? I can teleport you anywhere for 100 silver.");
                    AddOption("Twin City", 1);
                    AddOption("Market", 2);
                    AddOption("Just passing by.", 255);
                    break;
                case 1:
                    if (_client.Money >= 100)
                    {
                        _client.Money -= 100;
                        _client.ChangeMap(1011, 12, 377);
                    }
                    else
                    {
                        AddText("Sorry, you do not have 100 silver.");
                        AddOption("I see.", 255);
                    }
                    break;
                case 2:
                    if (_client.Money >= 100)
                    {
                        _client.Money -= 100;
                        _client.ChangeMap(1036, 212, 196);
                    }
                    else
                    {
                        AddText("Sorry, you do not have 100 silver.");
                        AddOption("I see.", 255);
                    }
                    break;
            }
            AddFinish();
            Send();
        }
    }
}
