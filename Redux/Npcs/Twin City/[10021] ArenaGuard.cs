using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{
    /// <summary>
    /// Handles NPC usage for [10021] Boxer
    /// Written by pro4never 11/16/2014
    /// </summary>
    public class NPC_10021 : INpc
    {

        public NPC_10021(Game_Server.Player _client)
            :base (_client)
    	{
            ID = 10021;	
			Face = 7;    
    	}

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("The arena is open. Welcome to challenge other people. The admission fee is only 50 silver. ");
                    AddText("If you PK in the arena, you will not gain or lose any experience or items equipped, and will get revived at the place you die. ");
                    AddText("The Kungfu circle is very dangerous, I suggest you PK in area.");
                    AddOption("Enter the arena.", 1);
                    AddOption("Just passing by.", 255);
                    break;
                case 1:
                    if (_client.Money >= 50)
                    { _client.ChangeMap(1005, 51, 71); _client.Money -= 50; }
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
