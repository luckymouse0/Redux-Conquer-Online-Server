using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{

    /// <summary>
    /// Handles NPC usage for [2071] CPAdmin
    /// </summary>
    public class NPC_2071 : INpc
    {

        public NPC_2071(Game_Server.Player _client)
            : base(_client)
        {
            ID = 2071;
            Face = 7;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            /*
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("I can help you turn DragonBalls into CP.");
                    AddOption("DragonBall (4.000 CP)", 1);
                    AddOption("DBScroll (40.000 CP)", 2);
                    AddOption("No thank you.", 255);
                    break;
                case 1:
                    if (_client.HasItem(1088000))
                    {
                        _client.DeleteItem(1088000);
                        _client.CP += 4000;
                        AddText("It is finished. Enjoy your Conquer Points.");
                        AddOption("Thanks.", 255);
                    }
                    else
                    {
                        AddText("I will not be so easily fooled. You do not have a DragonBall to trade!");
                        AddOption("Sorry.", 255);
                    }
                    break;
                case 2:
                    if (_client.HasItem(720028))
                    {
                        _client.DeleteItem(720028);
                        _client.CP += 40000;
                        AddText("It is finished. Enjoy your Conquer Points.");
                        AddOption("Thanks.", 255);
                    }
                    else
                    {
                        AddText("I will not be so easily fooled. You do not have a DBScroll to trade!");
                        AddOption("Sorry.", 255);
                    }
                    break;
            }
            AddFinish();
            Send();
             */

        }
    }
}
