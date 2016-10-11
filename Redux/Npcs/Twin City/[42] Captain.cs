using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{
    /// <summary>
    /// Handles NPC usage for [42] Captain
    /// </summary>
    public class NPC_42 : INpc
    {

        public NPC_42(Game_Server.Player _client)
            :base (_client)
    	{
    		ID = 42;	
			Face = 37;    
    	}

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    if (_client.HasEffect(Enum.ClientEffect.Black))
                    {
                        AddText("You must stay and think about what you've done!");
                        AddOption("I shall..", 255);

                    }
                    else
                    {
                        AddText("You are no danger to anyone. I can help you leave if you'd like.");
                        AddOption("Yes please", 1);
                        AddOption("Not yet", 255);
                    }
                    break;
                case 1:
                    if (!_client.HasEffect(Enum.ClientEffect.Black))
                    { _client.ChangeMap(1002, 517, 352); }
                    else
                    {
                        AddText("You must stay and think about what you've done!");
                        AddOption("I shall..", 255);
                    }
                    break;
            }
            AddFinish();
            Send();
        }
    }
}
