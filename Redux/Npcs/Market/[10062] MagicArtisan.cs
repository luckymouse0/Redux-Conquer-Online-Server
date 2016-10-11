using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;
using Redux.Enum;

namespace Redux.Npcs
{

    /// <summary>
    /// Handles NPC usage for [10062]  Magic Artisan 
    /// </summary>
    public class NPC_10062 : INpc
    {

        public NPC_10062(Game_Server.Player _client)
            : base(_client)
        {
            ID = 10062;
            Face = 6;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    {
                        AddText("Hello! I can always succeed in upgrading item quality. But I`ll ask for a few more dragon balls than Artisan Wind. I can also upgrade item levels. How can I help you?");
                        AddOption("Upgrade Quality", 1);
                        AddOption("Upgrade Level", 2);
                        AddOption("No thanks", 255);
                        break;
                    }

                case 1:
                    {
                        AddText("What item would you like me to upgrade the quality of?");
                        AddOption("Helmet/Earrings/TaoCap ", 11);
                        AddOption("Necklace/Bag ", 12);
                        AddOption("Ring/Bracelet ", 16);
                        AddOption("Weapon", 14);
                        AddOption("Shield ", 15);
                        AddOption("Armor ", 13);
                        AddOption("Boots ", 18);
                        AddOption("I changed my mind. ", 255);
                        break;
                    }

                case 2:
                    {
                        AddText("What item would you like me to upgrade the level of?");
                        AddOption("Helmet/Earrings/TaoCap ", 31);
                        AddOption("Necklace/Bag ", 32);
                        AddOption("Ring/Bracelet ", 36);
                        AddOption("Weapon", 34);
                        AddOption("Shield ", 35);
                        AddOption("Armor ", 33);
                        AddOption("Boots ", 38);
                        AddOption("I changed my mind. ", 255);
                        break;
                    }

                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 18:
                    {
                        var equipment = _client.Equipment.GetItemBySlot((Enum.ItemLocation)(_linkback % 10));
                        if (equipment == null)
                        {
                            AddText("There must be some mistake., You must be wearing an item before you may upgrade it!");
                            AddOption("Nevermind", 255);
                        }
                        else if (equipment.StaticID == equipment.GetNextItemQuality())
                        {
                            AddText("There must be some mistake. This item cannot be upgraded further!");
                            AddOption("Nevermind", 255);
                        }
                        else
                        {
                            var dbCost = 1 + 100 / equipment.ChanceToUpgradeQuality();
                            AddText("It will take " + dbCost + " DragonBalls to upgrade the quality of this item.");
                            AddOption("Upgrade it", (byte)(_linkback % 10 + 20));
                            AddOption("Never mind", 255);
                        }
                        break;
                    }

                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                case 28:
                    {
                        var equipment = _client.Equipment.GetItemBySlot((Enum.ItemLocation)(_linkback % 10));
                        if (equipment == null)
                        {
                            AddText("There must be some mistake., You must be wearing an item before you may upgrade it!");
                            AddOption("Nevermind", 255);
                        }
                        else if (equipment.StaticID == equipment.GetNextItemQuality())
                        {
                            AddText("There must be some mistake. This item cannot be upgraded!");
                            AddOption("Nevermind", 255);
                        }
                        else
                        {
                            var dbCost = 1 + 100 / equipment.ChanceToUpgradeQuality();
                            if (!_client.HasItem(Constants.DRAGONBALL_ID, dbCost))
                            {
                                AddText("You do not have the required " + dbCost + " DragonBalls to upgrade quality!");
                                AddOption("Sorry", 255);
                            }
                            else
                            {
                                for (var i = 0; i < dbCost; i++)
                                    _client.DeleteItem(Constants.DRAGONBALL_ID);
                                equipment.ChangeItemID(equipment.GetNextItemQuality());

                                if (Common.PercentSuccess(Constants.SOCKET_RATE * 2))
                                {
                                    if (equipment.Gem1 == 0)
                                    {
                                        equipment.Gem1 = 255;
                                        Managers.PlayerManager.SendToServer(new TalkPacket(ChatType.GM, "As a very lucky player, " + _client.Name + " has added the first socket to his/her " + equipment.BaseItem.Name));
                                    }
                                    else if (equipment.Gem2 == 0)
                                    {
                                        equipment.Gem2 = 255;
                                        Managers.PlayerManager.SendToServer(new TalkPacket(ChatType.GM, "As a very lucky player, " + _client.Name + " has added the second socket to his/her " + equipment.BaseItem.Name));
                                    }
                                } equipment.Save();
                                _client.Send(ItemInformationPacket.Create(equipment, ItemInfoAction.Update));
                                _client.Recalculate();
                                AddText("The quality has been upgraded! Enjoy your new item.");
                                AddOption("Thanks!", 255);
                            }
                        }
                        break;
                    }

                case 31:
                case 32:
                case 33:
                case 34:
                case 35:
                case 36:
                case 38:
                    {
                        var equipment = _client.Equipment.GetItemBySlot((Enum.ItemLocation)(_linkback % 10));
                        if (equipment == null)
                        {
                            AddText("There must be some mistake., You must be wearing an item before you may upgrade it!");
                            AddOption("Nevermind", 255);
                        }
                        else if (equipment.StaticID == equipment.GetNextItemLevel() || equipment.BaseItem.LevelReq >=120)
                        {
                            AddText("There must be some mistake. I cannot upgrade this item further.");
                            AddOption("Nevermind", 255);
                        }
                        else
                        {
                            var metCost = 1 + 100 / equipment.ChanceToUpgradeLevel();
                            if (metCost > 40)
                                metCost = 40;
                            AddText("It will take " + metCost + " Meteors to upgrade the level of this item.");
                            AddOption("Upgrade it", (byte)(_linkback % 10 + 40));
                            AddOption("Never mind", 255);
                        }
                        break;
                    }

                case 41:
                case 42:
                case 43:
                case 44:
                case 45:
                case 46:
                case 48:
                    {
                        var equipment = _client.Equipment.GetItemBySlot((Enum.ItemLocation)(_linkback % 10));
                        if (equipment == null)
                        {
                            AddText("There must be some mistake., You must be wearing an item before you may upgrade it!");
                            AddOption("Nevermind", 255);
                        }
                        else if (equipment.StaticID == equipment.GetNextItemLevel() || equipment.BaseItem.LevelReq >= 120)
                        {
                            AddText("There must be some mistake. I cannot upgrade this item further.");
                            AddOption("Nevermind", 255);
                        }
                        else if (_client.Level < equipment.GetDBItemByStaticID(equipment.GetNextItemLevel()).LevelReq)
                        {
                            AddText("You are too low level to wear the upgraded item!");
                            AddOption("Oops", 255);
                        }
                        else
                        {
                            var metCost = 1 + 100 / equipment.ChanceToUpgradeLevel();
                            if (metCost > 40)
                                metCost = 40;
                            if (!_client.HasItem(Constants.METEOR_ID, (int)metCost))
                            {
                                AddText("You do not have the required " + metCost + " Meteors to upgrade this item's level!");
                                AddOption("Sorry", 255);
                            }
                            else
                            {
                                for (var i = 0; i < metCost; i++)
                                    _client.DeleteItem(Constants.METEOR_ID);
                                equipment.ChangeItemID(equipment.GetNextItemLevel());
                                if (Common.PercentSuccess(Constants.SOCKET_RATE))
                                {
                                    if (equipment.Gem1 == 0)
                                    {
                                        equipment.Gem1 = 255;
                                        Managers.PlayerManager.SendToServer(new TalkPacket(ChatType.GM, "As a very lucky player, " + _client.Name + " has added the first socket to his/her " + equipment.BaseItem.Name));
                                    }
                                    else if (equipment.Gem2 == 0)
                                    {
                                        equipment.Gem2 = 255;
                                        Managers.PlayerManager.SendToServer(new TalkPacket(ChatType.GM, "As a very lucky player, " + _client.Name + " has added the second socket to his/her " + equipment.BaseItem.Name));
                                    }
                                }
                                equipment.Save();
                                _client.Send(ItemInformationPacket.Create(equipment, ItemInfoAction.Update));
                                _client.Recalculate();
                                AddText("The level has been upgraded! Enjoy your new item.");
                                AddOption("Thanks!", 255);
                            }
                        }
                        break;
                    }
            }
            AddFinish();
            Send();

        }
    }
}
