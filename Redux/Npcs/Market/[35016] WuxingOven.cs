using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;
using Redux.Enum;


namespace Redux.Npcs
{

    public class NPC_35016 : INpc
    {

        public NPC_35016(Game_Server.Player _client)
            : base(_client)
        {
            ID = 35016;
            Face = 67;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("A good set of equipment can help you a lot in future battles.Is there something I can do for you?");
                    AddOption("Enchant HP to my gears.", 1);
                    AddOption("Composing Upgrade.", 2);
                    AddOption("Nothing right now, thanks.", 255);

                    break;
                case 1:
                    _client.Send(GeneralActionPacket.Create(_client.UID, Enum.DataAction.OpenCustom, 1091));
                    break;
                case 2:
                    AddText("There are two ways of composing from +1 to +9 with +n stones or +n items and from +9 to +12 with DragonBalls.");
                    AddText("Whitch one would you want?");
                    AddOption("Compose +1 to +9.", 3);
                    AddOption("Compose +9 to +12.", 4);
                    AddOption("How does composing  +9 to +12 works?", 5);
                    AddOption("Thanks.", 255);

                    break;
                case 3:
                    _client.Send(GeneralActionPacket.Create(_client.UID, Enum.DataAction.OpenWindow, 1));
                    break;
                case 4:
                    AddText("What a glorious day! I belive it's the best weather to refine equipment in. Do you need my help with anything?");
                    AddOption("Compose +9 Equipment to +10.", 6);
                    AddOption("Compose +10 Equipment to +11.", 7);
                    AddOption("Compose +11 Equipment to +12.", 8);
                    AddOption("Nothing right now, thanks.", 255);
                    break;
                case 5:
                    AddText("If you have any +9 items, you may use DragonBalls to refine it and upgrade its bonus level up to +12.");
                    AddOption("Are there any limits to equipment refining?", 9);
                    AddOption("How many DragonBalls would I need?", 17);
                    break;
                case 9:
                    AddText("When your level reaches 130, I can help you upgrade the bonus level of your equiment and weapons.");
                    AddText("However, there are limitations.Only right-Handed weapons can be refined. Whereas, there are no limits to equipment refining.");
                    AddOption("Thanks. I never knew.", 255);
                    break;
                case 17:
                    AddText("12 DragonBalls are required for refining +9 items to +10, ");
                    AddText("25 DragonBalls are required for refining +10 items to +11, ");
                    AddText("4 DBScrolls are required for refining +11 items to +12,");
                    AddOption("Got it, thanks.", 255);
                    break;

                #region +9 to +10

                case 6:
                    AddText("Choose the equipment you want to upgrade from +9 to +10 from the list below. ");
                    AddOption("Helmet/Earrings/TaoCap ", 11);
                    AddOption("Ring/Bracelet ", 16);
                    AddOption("Necklace/Bag ", 12);
                    AddOption("Boots ", 18);
                    AddOption("Armor ", 13);
                    AddOption("Shield ", 15);
                    AddOption("Weapon ", 14);
                    AddOption("I changed my mind. ", 255);

                    break;
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
                            AddText("There must be some mistake.... You must be wearing the items in order to refine it!");
                        else if (equipment.Plus != 9)
                            AddText("It seems that your items is not +9, in this case i can not help you.");
                        else if (!_client.HasItem(1088000, 12))
                            AddText("There must be some mistake. You do not have the 12 DragonBalls required to refine this item!");
                        else
                        {
                            for (var i = 0; i < 12; i++)
                                _client.DeleteItem(1088000);
                            equipment.Plus++;
                            _client.Send(ItemInformationPacket.Create(equipment));
                            AddText("It is done! Please enjoy your new equipment.");
                            Managers.PlayerManager.SendToServer(new TalkPacket(ChatType.GM, _client.Name + " upgraded his " + equipment.BaseItem.Name + " to +10 . Congratulations!"));
                            _client.Save();

                        }
                        AddOption("Nevermind", 255);
                        break;
                    }
                #endregion

                #region +10 to +11
                case 7:
                    AddText("Choose the equipment you want to upgrade from +10 to +11 from the list below. ");
                    AddOption("Helmet/Earrings/TaoCap ", 21);
                    AddOption("Ring/Bracelet ", 26);
                    AddOption("Necklace/Bag ", 22);
                    AddOption("Boots ", 28);
                    AddOption("Armor ", 23);
                    AddOption("Shield ", 25);
                    AddOption("Weapon ", 24);
                    AddOption("I changed my mind. ", 255);

                    break;
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                case 28:
                    {
                        var equipment = _client.Equipment.GetItemBySlot((Enum.ItemLocation)(_linkback % 20));
                        if (equipment == null)
                            AddText("There must be some mistake.... You must be wearing the items in order to refine it!");
                        else if (equipment.Plus != 10)
                            AddText("It seems that your items is not +10, in this case I can not help you.");
                        else if (!_client.HasItem(1088000, 25))
                            AddText("There must be some mistake. You do not have the 25 DragonBalls required to refine this item!");
                        else
                        {
                            for (var i = 0; i < 25; i++)
                                _client.DeleteItem(1088000);
                            equipment.Plus++;
                            _client.Send(ItemInformationPacket.Create(equipment));
                            AddText("It is done! Please enjoy your new equipment.");
                            Managers.PlayerManager.SendToServer(new TalkPacket(ChatType.GM, _client.Name + " upgraded his " + equipment.BaseItem.Name + " to +11 . Congratulations!"));
                            _client.Save();

                        }
                        AddOption("Nevermind", 255);
                        break;
                    }
                #endregion

                #region +11 to +12


                case 8:
                    AddText("Choose the equipment you want to upgrade from +11 to +12 from the list below. ");
                    AddOption("Helmet/Earrings/TaoCap ", 31);
                    AddOption("Ring/Bracelet ", 36);
                    AddOption("Necklace/Bag ", 32);
                    AddOption("Boots ", 38);
                    AddOption("Armor ", 33);
                    AddOption("Shield ", 35);
                    AddOption("Weapon ", 34);
                    AddOption("I changed my mind. ", 255);

                    break;
                case 31:
                case 32:
                case 33:
                case 34:
                case 35:
                case 36:
                case 38:
                    {
                        var equipment = _client.Equipment.GetItemBySlot((Enum.ItemLocation)(_linkback % 30));
                        if (equipment == null)
                            AddText("There must be some mistake.... You must be wearing the items in order to refine it!");
                        else if (equipment.Plus != 11)
                            AddText("It seems that your items is not +11, in this case I can not help you.");
                        else if (!_client.HasItem(720028, 4))
                            AddText("There must be some mistake. You do not have the 4 DBScrolls required to refine this item!");
                        else
                        {
                            for (var i = 0; i < 4; i++)
                                _client.DeleteItem(720028);
                            equipment.Plus++;
                            _client.Send(ItemInformationPacket.Create(equipment));
                            AddText("It is done! Please enjoy your new equipment.");
                            Managers.PlayerManager.SendToServer(new TalkPacket(ChatType.GM, _client.Name + " upgraded his " + equipment.BaseItem.Name + " to +12 . Congratulations!"));
                            _client.Save();

                        }
                        AddOption("Nevermind", 255);
                        break;
                    }
                #endregion

            }
            AddFinish();
            Send();
        }
    }
}