using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{

    public class NPC_1152 : INpc
    {
        /// <summary>
        /// Handles NPC usage for [1152] Lab Tele and ZONE
        /// </summary>
        public NPC_1152(Game_Server.Player _client)
            : base(_client)
        {
            ID = 1152;
            Face = 1;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("Where are you heading for? I can teleport you into an dark place full of caves and strong monsters for  VirtuePoints .");
                    AddText("Make sure to gather all diamonds dropped by monsters as I will reward you nicely for them !");
                    AddOption("Send me there!", 1);
                    AddOption("I have some diamonds", 5);
                    AddOption("Just passing by.", 255);
                    break;
                case 1://lab1
                    if (_client.VirtuePoints >= 2000)
                    {
                        _client.VirtuePoints -= 2000;
                        _client.ChangeMap(1351, 100, 100);
                    }
                    else
                    {
                        AddText("Sorry, you do not have enough VirtuePoints to enter!");
                        AddOption("I see.", 255);
                    }
                    break;
                case 5:
                    AddText("What diamonds you wish to exchange?");
                    AddOption("15 SunDiamonds", 6);
                    AddOption("12 MoonDiamonds", 7);
                    AddOption("12 StarDiamonds", 8);
                    AddOption("10 CloudDiamonds", 9);
                    AddOption("Just passing by.", 255);
                    break;

                case 6:
                    if (_client.HasItem(721533, 15))
                    {
                        for (var i = 0; i < 15; i++)
                            _client.DeleteItem(721533);
                        _client.CreateItem(720027);

                    }
                    else
                    {
                        AddText("Sorry, you do not have enough diamonds!");
                        AddOption("I see.", 255);

                    }
                    break;
                case 7:
                    if (_client.HasItem(721534, 12))
                    {
                        for (var i = 0; i < 12; i++)
                            _client.DeleteItem(721534);
                        _client.CreateItem(720027);
                        _client.Money += 250000;

                    }
                    else
                    {
                        AddText("Sorry, you do not have enough diamonds!");
                        AddOption("I see.", 255);

                    }
                    break;
                case 8:
                    if (_client.HasItem(721535, 12))
                    {
                        for (var i = 0; i < 12; i++)
                            _client.DeleteItem(721535);
                        _client.CreateItem(720027);
                        _client.Money += 500000;
                    }
                    else
                    {
                        AddText("Sorry, you do not have enough diamonds!");
                        AddOption("I see.", 255);

                    }
                    break;
                case 9:
                    if (_client.HasItem(721536, 10))
                    {
                        for (var i = 0; i < 10; i++)
                            _client.DeleteItem(721536);
                        _client.CreateItem(721541);
                        _client.Money += 1000000;


                    }
                    else
                    {
                        AddText("Sorry, you do not have enough diamonds!");
                        AddOption("I see.", 255);

                    }
                    break;

            }
            AddFinish();
            Send();

        }
    }
}