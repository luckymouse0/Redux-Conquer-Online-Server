/*
 * User: pro4never
 * Date: 10/26/2014
 * Time: 9:18 PM
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
    /// Handles NPC usage for [126] Assistant
    /// </summary>
    public class NPC_127 : INpc
    {

        public NPC_127(Game_Server.Player _client)
            :base (_client)
    	{
            ID = 127;	
			Face = 9;    
    	}

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("Hello, I am the assistant of the mine union.");
                    AddText("If you want to enter the mine cave, I can send you.");
                    AddOption("Yes Please", 1);
                    AddOption("Just passing by.", 255);
                    break;
                case 1:
                    if (_client.Level >= 70)
                        _client.ChangeMap(1027, 138, 104);
                    else
                    {
                        AddText("Sorry, you must be at least level 70 before you can enter this mine.");
                        AddOption("Sorry", 255);
                    }
                    break;
            }
            AddFinish();
            Send();
        }
    }
}
