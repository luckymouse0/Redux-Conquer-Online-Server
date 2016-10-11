using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{

    public class NPC_10050 : INpc
    {
        /// <summary>
        /// Handles NPC usage for [10050] Twin City Conductress
        /// </summary>
        public NPC_10050(Game_Server.Player _client)
            : base(_client)
        {
            ID = 10050;
            Face = 1;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("Where are you heading for? I can teleport you for a price of 100 silver.");
                    AddOption("Phoenix Castle", 1);
                    AddOption("Desert City", 2);
                    AddOption("Ape Mountain", 3);
                    AddOption("Bird Island.", 4);
                    AddOption("Mine Cave", 5);
                    AddOption("Market", 6);
                    AddOption("Just passing by.", 255);
                    break;
                case 1://Phoenix Castle
                    if (_client.Money >= 100)
                    {
                        _client.Money -= 100;
                        _client.ChangeMap(1002, 957, 557);
                    }
                    else
                    {
                        AddText("Sorry, you do not have 100 silver.");
                        AddOption("I see.", 255);
                    }
                    break;
                case 2://Desert City
                    if (_client.Money >= 100)
                    {
                        _client.Money -= 100;
                        _client.ChangeMap(1002, 64, 469);
                    }
                    else
                    {
                        AddText("Sorry, you do not have 100 silver.");
                        AddOption("I see.", 255);
                    }
                    break;
                case 3://Ape Mountain
                    if (_client.Money >= 100)
                    {
                        _client.Money -= 100;
                        _client.ChangeMap(1002, 555, 957);
                    }
                    else
                    {
                        AddText("Sorry, you do not have 100 silver.");
                        AddOption("I see.", 255);
                    }
                    break;
                case 4://Bird Island
                    if (_client.Money >= 100)
                    {
                        _client.Money -= 100;
                        _client.ChangeMap(1002, 228, 193);
                    }
                    else
                    {
                        AddText("Sorry, you do not have 100 silver.");
                        AddOption("I see.", 255);
                    }
                    break;
                case 5://Mine Cave
                    if (_client.Money >= 100)
                    {
                        _client.Money -= 100;
                        _client.ChangeMap(1002, 53, 399);
                    }
                    else
                    {
                        AddText("Sorry, you do not have 100 silver.");
                        AddOption("I see.", 255);
                    }
                    break;
                case 6://Market
                    if (_client.Money >= 100)
                    {
                        _client.Money -= 100;
                        _client.ChangeMap(1036, 211, 196);
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