using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{

    /// <summary>
    /// Handles NPC usage for [5004] MillionareLee
    /// </summary>
    public class NPC_5004 : INpc
    {

        public NPC_5004(Game_Server.Player _client)
            : base(_client)
        {
            ID = 5004;
            Face = 102;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("If you have more rare items then you know what to do with, I can help you pack them!");
                    AddOption("Pack Meteors", 1);
                    AddOption("Pack DragonBalls", 2);
                    AddOption("I don't have that problem.", 255);
                    break;
                case 1:
                    if (!_client.HasItem(Constants.METEOR_ID, 10))
                    {
                        AddText("Fool! You need 10 Meteors if you want me to help you pack them.");
                        AddOption("I'm sorry!", 255);
                    }
                    else
                    {
                        for (var i = 0; i < 10; i++)
                            _client.DeleteItem(Constants.METEOR_ID);
                        _client.CreateItem(Constants.METEOR_SCROLL_ID);
                        AddText("It is done. Enjoy your MeteorScroll.");
                        AddOption("Wow, thanks!", 255);
                    }
                    break;
                case 2:
                    if (!_client.HasItem(Constants.DRAGONBALL_ID, 10))
                    {
                        AddText("Fool! You need 10 DragonBalls if you want me to help you pack them.");
                        AddOption("I'm sorry!", 255);
                    }
                    else
                    {
                        for (var i = 0; i < 10; i++)
                            _client.DeleteItem(Constants.DRAGONBALL_ID);
                        _client.CreateItem(Constants.DB_SCROLL_ID);
                        AddText("It is done. Enjoy your DBScroll.");
                        AddOption("Wow, thanks!", 255);
                    }
                    break;              
            }
            AddFinish();
            Send();

        }
    }
}
