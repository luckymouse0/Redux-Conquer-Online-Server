/*
 * User: mujake
 * Date: 12/31/2013
 * Time: 8 AM
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
    /// Handles NPC usage for [20005] Celestine
    /// </summary>
    public class NPC_20005 : INpc
    {

        public NPC_20005(Game_Server.Player _client)
            : base(_client)
        {
            ID = 20005;
            Face = 9;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("People are pursuing greather achievement during their lives, but none can make it due to the limit of human constitution.");
                    AddOption("What does it mean?", 1);
                    AddOption("I don't belive it.", 255);
                    break;
                case 1:
                    AddText("Mortals are mundane. Only getting rid of it can help them accomplish their aims.");
                    AddText("If you are high level enough, you can get reborn to learn more and stronger skills.");
                    AddOption("How to get reborn?", 2);
                    AddOption("I am satisfied.", 255);
                    break;
                case 2:
                    AddText("It is difficult.First, you should reach an certain level. Second, you need a CelestialStone.");
                    AddOption("How to get CelestialStone?", 3);
                    AddOption("Forget it.", 255);
                    break;
                case 3:
                    AddText("CelestialStone is made of 7 gems: VioletGem, KylinGem, RainbowGem, MoonGem, PhoenixGem, FuryGem, DragonGem, and CleanWater.");
                    AddOption("What is CleanWater?", 4);
                    AddOption("It is difficult.", 255);
                    break;
                case 4:
                    AddText("It is used to get rid of your mundaneness, and then you won't be affected by the environment. By the way, Clean Water comes from celestial rinsing.");
                    AddOption("What are gems used for?", 5);
                    break;
                case 5:
                    AddText("Only seven gems can protect you during the rebirth.");
                    AddOption("I will collect them now.", 6);
                    AddOption("I changed my mind.", 255);
                    break;
                case 6:
                    AddText("It is easy to get the gems. But CleanWater...");
                    AddOption("But what?", 7);
                    break;
                case 7:
                    AddText("The Adventure island is the headstream of CleanWater. But it is occupied by WaterEvil and he uses spell to hide the stream.");
                    AddOption("What can I do?", 8);
                    AddOption("I will give up.", 255);
                    break;
                case 8:
                    AddText("WaterEvilElder will go to get the water every certain time. If you defeat him, you may get the water. But he is very hard to deal with.");
                    AddOption("I see.Thank you.", 255);
                    AddOption("Anything else?", 9);
                    break;
                case 9:
                    if (_client.HasItem(721258) && _client.HasItem(700001) && _client.HasItem(700011) && _client.HasItem(700021) && _client.HasItem(700031) && _client.HasItem(700041) && _client.HasItem(700051) && _client.HasItem(700061))
                    {
                        AddText("Are you sure you want to refine CelestialStone?");
                        AddOption("Sure.", 10);
                        AddOption("I am not sure.", 255);
                    }
                    else
                    {
                        AddText("Come here to refine CelestialStone when you get the 7 Gems and CleanWater.");
                        AddOption("That's ok.", 255);

                    }
                    break;
                case 10: if (_client.HasItem(721258) && _client.HasItem(700001) && _client.HasItem(700011) && _client.HasItem(700021) && _client.HasItem(700031) && _client.HasItem(700041) && _client.HasItem(700051) && _client.HasItem(700061))
                    {
                        AddText("There you go, check it in your inventory.");
                        AddOption("Thank you.", 255);
                        _client.DeleteItem(721258);
                        _client.DeleteItem(700001);
                        _client.DeleteItem(700011);
                        _client.DeleteItem(700021);
                        _client.DeleteItem(700031);
                        _client.DeleteItem(700041);
                        _client.DeleteItem(700051);
                        _client.DeleteItem(700061);
                        _client.CreateItem(721259);
                    }
                    else
                    {
                        AddText("Come here to refine CelestialStone when you get the 7 Gems and CleanWater.");
                        AddOption("That's ok.", 255);

                    }

                    break;
            }
            AddFinish();
            Send();
        }
    }
}