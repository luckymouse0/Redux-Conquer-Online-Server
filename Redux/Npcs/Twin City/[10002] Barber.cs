using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{
    /// <summary>
    /// Handles NPC usage for [10002] Barber
    /// </summary>
    public class NPC_10002 : INpc
    {

        public NPC_10002(Game_Server.Player _client)
            :base (_client)
    	{
    		ID = 10002;	
			Face = 111;    
    	}

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("I can help you find a new hairstyle just for you! Only 1,00 silver to re-invent yourself.");
                    AddOption("Cut my Hair", 1);
                    AddOption("Just passing by.", 255);
                    break;
                case 1:
                    if (_client.Money >= 1000)
                    {
                        _client.Money -= 1000;
                        _client.Hair = (ushort)((_client.HairColour * 100) + Common.Random.Next(30, 51));
                        AddText("Perfection! I hope you enjoy your new haircut. Come back any time if you wish to change it again.");
                    }
                    else
                        AddText("I'm sorry, you do not have the 1000 silver required for my services.");                    
                    AddOption("Thanks", 255);
                    break;
            }
            AddFinish();
            Send();
        }
    }
}
