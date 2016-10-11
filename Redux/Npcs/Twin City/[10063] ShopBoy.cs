using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{

    public class NPC_10063 : INpc
    {
        /// <summary>
        /// Handles NPC usage for [10063] Shop Boy
        /// </summary>
        public NPC_10063(Game_Server.Player _client)
            : base(_client)
        {
            ID = 10063;
            Face = 7;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("Our shop is famous for dyeing. If you want to have your equipment dyed, please wear them before you enter. You");
                    AddText(" have a wide choice of colors. One meteor will be charged before you try the colors. Do you want a try?");
                    AddOption("Yes, here is a meteor", 1);
                    AddOption("Do you have any good wine?", 2);
                    AddOption("Never mind", 255);
                    break;
                case 1:
                    if (!_client.HasItem(Constants.METEOR_ID))
                    {
                        AddText("You do not have the Meteor I requested.");
                        AddOption("Sorry", 255);
                    }
                    else
                    {
                        _client.DeleteItem(Constants.METEOR_ID);
                        _client.ChangeMap(1008, 24, 24);
                        AddText("You can change your equipment's color as many times as you like while here.");
                        AddText(" When ready to leave just jump through the portal.");
                        AddOption("Understood", 255);
                    }
                    break;
                case 2:
                    AddText("I can sell you DrunkCelestial wine for 1,000 silver if you would like.");
                    AddOption("Yes please", 3);
                    AddOption("No thanks", 255);
                    break;
                case 3:
                    if (_client.Money < 1000)
                    {
                        AddText("You do not have the 1,000 silver required");
                        AddOption("Sorry", 255);
                    }
                    if (_client.Inventory.Count >= 40)
                    {
                        AddText("Please make room in your inventory for the wine.");
                        AddOption("Sorry", 255);
                    }
                    else
                    {
                        _client.Money -= 1000;
                        _client.CreateItem(722185);
                        AddText("Enjoy the wine, it's the finest in all the land!");
                        AddOption("I will!", 255);
                    }
                    break;
            }
            AddFinish();
            Send();

        }
    }
}