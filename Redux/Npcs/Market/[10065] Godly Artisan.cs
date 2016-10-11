using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{

    /// <summary>
    /// Handles NPC usage for [10065]  Godly Artisan 
    /// NOT FINISHED!
    /// </summary>
    public class NPC_10065 : INpc
    {

        public NPC_10065(Game_Server.Player _client)
            : base(_client)
        {
            ID = 10065;
            Face = 54;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Hello , did you knew that an socketed equipment is better than one without sockets?");
                        AddText("It costs DragonBalls or ToughDrills/StarDrills depending on the item.");
                        AddOption("Socket my weapon", 1);
                        AddOption("Socket my gears", 2);
                        AddOption("No. I like it this way.", 255);
                        break;
                    }
                case 1:
                    {
                        var equipment = _client.Equipment.GetItemBySlot(Enum.ItemLocation.WeaponR);
                        if (equipment == null)
                            AddText("There must be some mistake. You must be wearing a weapon before I can help you socket it!");
                        else if (equipment.Gem2 > 0)
                            AddText("There must be some mistake. Your weapon already has the maximum number of sockets!");
                        else
                        {
                            var cost = (uint)(equipment.Gem1 == 0 ? 1 : 5);
                            AddText("It will cost " + cost + " DragonBall(s) to socket this weapon. Are you positive you wish to continue?");
                            AddOption("I'm Sure!", 10);
                        }
                        AddOption("Nevermind", 255);
                        break;
                    }
                case 2:
                    {
                        AddText("It costs 12 DragonBalls for the first socket and 7 StarDrills for second.If you consider yourself lucky try ToughDrill! ");
                        AddOption("Make the first socket ", 3);
                        AddOption("Make the second socket ", 4);
                        AddOption("No. I like it this way.", 255);
                        break;
                    }
                case 3:
                    {
                        AddText("The first socket requires 12 DragonBalls and there is no turning back! ");
                        AddText("What item would you like me to socket?");
                        AddOption("Helmet/Earrings/TaoCap ", 11);
                        AddOption("Necklace/Bag ", 12);
                        AddOption("Ring/Bracelet ", 16);
                        AddOption("Shield ", 15);
                        AddOption("Armor ", 13);
                        AddOption("Boots ", 18);
                        AddOption("I changed my mind. ", 255);
                        break;
                    }
                case 4:
                    {
                        AddText("Opening the second socket is a delicate matter. I can guarentee an upgrade by using 7 StarDrills ");
                        AddText("or you can try your luck using a ToughDrill.");
                        AddOption("Use ToughDrill", 5);
                        AddOption("Use 7 StarDrills", 6);
                        AddOption("No. I like it this way.", 255);
                        break;
                    }
                case 5:
                    {
                        AddText("There is no guarentee this upgrade will be a success. Each attempt will require a ToughDrill. ");
                        AddText("What item would you like to try to socket?");
                        AddOption("Helmet/Earrings/TaoCap ", 21);
                        AddOption("Necklace/Bag ", 22);
                        AddOption("Ring/Bracelet ", 26);
                        AddOption("Shield ", 25);
                        AddOption("Armor ", 23);
                        AddOption("Boots ", 28);
                        AddOption("I changed my mind. ", 255);
                        break;
                    }
                case 6:
                    {
                        AddText("This upgrade is guarenteed to work but requires 7 StarDrills. ");
                        AddText("What item would you like to try to socket?");
                        AddOption("Helmet/Earrings/TaoCap ", 31);
                        AddOption("Necklace/Bag ", 32);
                        AddOption("Ring/Bracelet ", 36);
                        AddOption("Shield ", 35);
                        AddOption("Armor ", 33);
                        AddOption("Boots ", 38);
                        AddOption("I changed my mind. ", 255);
                        break;
                    }
                case 10:
                    {
                        var equipment = _client.Equipment.GetItemBySlot(Enum.ItemLocation.WeaponR);
                        if (equipment == null)
                        {
                            AddText("There must be some mistake. You must be wearing a weapon before I can help you socket it!");
                            AddOption("Nevermind", 255);
                        }
                        else if (equipment.Gem2 > 0)
                        {
                            AddText("There must be some mistake. Your weapon already has the maximum number of sockets!");
                            AddOption("Nevermind", 255);
                        }
                        else
                        {
                            var cost = equipment.Gem1 == 0 ? 1 : 5;
                            if (_client.HasItem(Constants.DRAGONBALL_ID, cost))
                            {
                                if (cost == 1)
                                    equipment.Gem1 = 255;
                                else
                                    equipment.Gem2 = 255;

                                for (var i = 0; i < cost; i++)
                                    _client.DeleteItem(Constants.DRAGONBALL_ID);
                                equipment.Save();
                                _client.Send(ItemInformationPacket.Create(equipment));
                                AddText("It is done! Please enjoy your new equipment.");
                                AddOption("Thanks!", 255);
                            }
                            else
                            {
                                AddText("There must be some mistake. Your do not have the " + cost + " DragonBall(s) required");
                                AddOption("Nevermind", 255);
                            }

                        }
                        break;
                    }

                case 11:
                case 12:
                case 13:
                case 15:
                case 16:
                case 18:
                    {
                        var equipment = _client.Equipment.GetItemBySlot((Enum.ItemLocation)(_linkback % 10));
                        if (equipment == null)
                        {
                            AddText("There must be some mistake., You must be wearing an item before you may socket it!");
                            AddOption("Nevermind", 255);
                        }
                        else if (equipment.Gem1 != 0)
                        {
                            AddText("There must be some mistake. This item already has a socket!");
                            AddOption("Nevermind", 255);
                        }
                        else if (!_client.HasItem(Constants.DRAGONBALL_ID, 12))
                        {
                            AddText("There must be some mistake. You do not have the 12 DragonBalls required to socket this item!");
                            AddOption("Nevermind", 255);
                        }
                        else
                        {
                            for (var i = 0; i < 12; i++)
                                _client.DeleteItem(Constants.DRAGONBALL_ID);
                            equipment.Gem1 = 255;
                            equipment.Save();
                            _client.Send(ItemInformationPacket.Create(equipment));
                            AddText("It is done! Please enjoy your new equipment.");
                            AddOption("Thanks!", 255);
                        }

                        break;
                    }

                case 21:
                case 22:
                case 23:
                case 25:
                case 26:
                case 28:
                    {
                        var equipment = _client.Equipment.GetItemBySlot((Enum.ItemLocation)(_linkback % 10));
                        if (equipment == null)
                        {
                            AddText("There must be some mistake., You must be wearing an item before you may socket it!");
                            AddOption("Nevermind", 255);
                        }
                        else if (equipment.Gem1 == 0)
                        {
                            AddText("There must be some mistake. You must open the first socket before you may attempt the second!");
                            AddOption("Nevermind", 255);
                        }
                        else if (equipment.Gem2 != 0)
                        {
                            AddText("There must be some mistake. This item already has two sockets!");
                            AddOption("Nevermind", 255);
                        }
                        else if (!_client.HasItem(1200005))
                        {
                            AddText("There must be some mistake. You do not have a ToughDrill!");
                            AddOption("Nevermind", 255);
                        }
                        else
                        {
                            _client.DeleteItem(1200005);
                            if (Common.PercentSuccess(12))
                            {
                                equipment.Gem2 = 255;
                                equipment.Save();
                                _client.Send(ItemInformationPacket.Create(equipment));
                                AddText("It is done! Please enjoy your new equipment.");
                                AddOption("Thanks!", 255);
                            }
                            else
                            {
                                _client.CreateItem(1200006);
                                AddText("I was not able to open the second socket for you. I will give you a StarDrill for future use.");
                                AddOption("Damn...", 255);
                            }
                        }
                        break;
                    }

                case 31:
                case 32:
                case 33:
                case 35:
                case 36:
                case 38:
                    {
                        var equipment = _client.Equipment.GetItemBySlot((Enum.ItemLocation)(_linkback % 10));
                        if (equipment == null)
                        {
                            AddText("There must be some mistake., You must be wearing an item before you may socket it!");
                            AddOption("Nevermind", 255);
                        }
                        else if (equipment.Gem1 == 0)
                        {
                            AddText("There must be some mistake. You must open the first socket before you may attempt the second!");
                            AddOption("Nevermind", 255);
                        }
                        else if (equipment.Gem2 != 0)
                        {
                            AddText("There must be some mistake. This item already has two sockets!");
                            AddOption("Nevermind", 255);
                        }
                        else if (!_client.HasItem(1200006, 7))
                        {
                            AddText("There must be some mistake. You do not have the required 7 StarDrills!");
                            AddOption("Nevermind", 255);
                        }
                        else
                        {
                            for (var i = 0; i < 7; i++)
                                _client.DeleteItem(1200006);
                            equipment.Gem2 = 255;
                            equipment.Save();
                            _client.Send(ItemInformationPacket.Create(equipment));
                            AddText("It is done! Please enjoy your new equipment.");
                            AddOption("Thanks!", 255);

                        }
                        break;
                    }
            }
            AddFinish();
            Send();

        }
    }
}
