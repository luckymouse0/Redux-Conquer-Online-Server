using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{

    /// <summary>
    /// </summary>
    public class NPC_3953 : INpc
    {
        private byte baseGemRequest;
        private uint BASE_GEM_ID;
        private uint cost;


        public NPC_3953(Game_Server.Player _client)
            : base(_client)
        {
            ID = 3953;
            Face = 54;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:

                    AddText("Hello, do you need to exchange those  gems for one of higher quality and spare some space in warehouses?");
                    AddText("For each 15 gems of normal or refined quality I will give you one of higher quality?");
                    AddOption("Compose gems .", 1);
                    AddOption("I see.", 255);
                    break;
                case 1:
                    AddText("What kind of gems do you wish to compose?");
                    AddText("The fee for composing is 10.000 gold for normal gems into refined and 100.000 for refined into super gems ?");
                    AddOption("Compose normal to refined gems .", 2);
                    AddOption("Compose refined to super gems .", 3);

                    AddOption("I see.", 255);
                    break;
                case 2:
                    BASE_GEM_ID = 700001;
                    cost = 10000;
                    goto case 4;
                case 3:
                    BASE_GEM_ID = 700002;
                    cost = 100000;
                    goto case 4;

                case 4:
                    AddText("What type of gem would you like?");
                    AddOption("Phoenix Gem", 20);
                    AddOption("Dragon Gem", 21);
                    AddOption("Fury Gem", 22);
                    AddOption("Rainbow Gem", 23);
                    AddOption("More Options", 5);
                    AddOption("I'm not sure...", 255);
                    break;
                case 5:
                    AddText("What type of gem would you like?");
                    AddOption("Kylin Gem", 24);
                    AddOption("Violet Gem", 25);
                    AddOption("Moon Gem", 26);
                    AddOption("Previous Options", 4);
                    AddOption("I'm not sure...", 255);
                    break;
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                    baseGemRequest = (byte)(_linkback % 10);
                    _linkback = 6;
                    goto case 6;

                case 6:
                    if (_client.Money < cost)
                    {
                        AddText("You don't have the required amount of  money!");
                        AddOption("Ok! ", 255);

                    }
                    else if (_client.HasItem((uint)(BASE_GEM_ID + baseGemRequest * 10), 15))//Check if they have at least 15
                    {
                        for (var i = 0; i < 15; i++)
                            _client.DeleteItem((uint)(BASE_GEM_ID + baseGemRequest * 10));
                        _client.Money -= cost;
                        _client.CreateItem((uint)(BASE_GEM_ID + baseGemRequest * 10 + 1));
                        AddText("There you go.Check it out in your inventory!");
                        AddOption("Thanks!", 255);
                        _client.Save();

                    }
                    else
                    {
                        AddText("You don't have the required amount of gems or not enough money!");
                        AddOption("Ok! ", 255);


                    }
                    break;
            }
            AddFinish();
            Send();

        }
    }
}