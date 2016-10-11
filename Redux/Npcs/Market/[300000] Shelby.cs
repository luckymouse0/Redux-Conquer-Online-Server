/*
 * User: mujake
 * Date: 12/30/2013
 * Time: 1 PM
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
    /// Handles NPC usage for [300000] Shelby
    /// </summary>
    public class NPC_300000 : INpc
    {

        public NPC_300000(Game_Server.Player _client)
            : base(_client)
        {
            ID = 300000;
            Face = 54;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    if (_client.Character.Level >= 70)
                    {
                        AddText("We hope all can help each other. If you power level the newbies, we may reward you with Experience, Meteors or Dragonballs.");
                        AddText("Are you interested?");
                        AddOption("Tell me more details.", 1);
                        AddOption("Check my virtue points.", 5);
                        AddOption("Claim prize.", 6);
                        AddOption("Just passing by.", 255);
                    }
                    else
                    {
                        AddText("You must be level 70 at least or more in order to get virtue points and be able to exchange for am prize.");
                        AddOption("I see.", 255);

                    }
                    break;
                case 1:
                    AddText("If you are above level 70 and try to power level the newbies (at least 20 levels lower than you), you may gain virtue points.");
                    AddOption("What are the virtue points?", 2);
                    break;
                case 2:
                    AddText("The more newbies you power level, the more virtue points you gain. I shall give you a good reward for a certain virtue points.");
                    AddOption("How can I gain virtue points?", 3);
                    AddOption("What prize can I expect?", 4);

                    break;
                case 3:
                    AddText("Once the newbies are one level up, the team captain can gain virtue points accordingly.");
                    AddOption("I see.", 255);

                    break;
                case 4:
                    AddText("I shall reward you exp. of equivalent to an ExpBall for 15,000 virtue points , a meteor for 5,000 virtue points and an Dragonball for 50,000 virtue points.");
                    AddOption("I see.", 255);

                    break;
                case 5:
                    AddText("Your current virtue points are " + _client.VirtuePoints + "  , please try to gain more.");
                    AddOption("I see.", 255);

                    break;
                case 6:
                    if (_client.Inventory.Count == 40)
                    {
                        //Lazy sanity check to stop people accidentally wasting VP. They can still packet exploit but they'd just be wasting VP and not getting items.
                        AddText("Please make room in your inventory before trying to claim a prize!");
                        AddOption("Sorry", 255);
                    }
                    else
                    {
                        AddText("What prize do you prefer?");
                        AddOption("Meteor", 7);
                        AddOption("Experience.", 8);
                        AddOption("Dragonball", 9);
                        AddOption("Let me think it over.", 255);
                    }
                    break;
                case 7:
                    if (_client.VirtuePoints >= 5000)
                    {
                        _client.VirtuePoints -= 5000;
                        _client.CreateItem(1088001);
                        AddText("There you go.Check out in your inventory!");
                        AddOption("Thanks!", 255);

                    }
                    else
                    {
                        AddText("Sorry, you do not have the required virtue points.");
                        AddOption("I see.", 255);


                    }
                    break;
                case 8:
                    if (_client.VirtuePoints >= 15000 && _client.Character.Level <= 129)
                    {
                        _client.VirtuePoints -= 15000;
                        _client.GainExpBall(1200);
                        AddText("There you go!");
                        AddOption("Thanks!", 255);

                    }
                    else if (_client.VirtuePoints >= 15000 && _client.Character.Level >= 130)
                    {
                        AddText("Only characters below level 130 can exchange the virtue points for exp.");
                        AddText("You may find Simon (Twin city 393,235) and spend 2,000 virtue points to hunt the treasure in the Labyrinth.)");
                        AddOption("Wow.So cool.", 255);


                    }
                    else
                    {
                        AddText("Sorry, you do not have the required virtue points.");
                        AddOption("I see.", 255);

                    }
                    break;

                case 9:                    
                    if (_client.VirtuePoints >= 50000)
                    {
                        _client.VirtuePoints -= 50000;
                        _client.CreateItem(1088000);
                        AddText("There you go.Check out in your inventory!");
                        AddOption("Thanks!", 255);
                    }
                    else
                    {
                        AddText("Sorry, you do not have the required virtue points.");
                        AddOption("I see.", 255);


                    }
                    break;


            }
            AddFinish();
            Send();

        }
    }
}