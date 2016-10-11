using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;
using Redux.Enum;


namespace Redux.Npcs
{

    public class NPC_1234 : INpc
    {
        private int cost;
        bool hasAgreed = false;

        public NPC_1234(Game_Server.Player _client)
            : base(_client)
        {
            ID = 1234;
            Face = 67;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("A good set of equipment can help you a lot in future battles.I can help you by upgrading blessed attribute.");
                    AddText("Keep in mind that each upgrade costs TortoiseGems witch are rare and expensive.");
                    AddOption("Upgrade blessed attribute.", 1);
                    AddOption("Nothing right now, thanks.", 255);

                    break;
                case 1:
                    {
                        AddText("I see, upgrading an items blessed attribute will help you receive less damage. ");
                        AddText("What item would you like me upgrade?");
                        AddOption("Helmet/Earrings/TaoCap ", 11);
                        AddOption("Necklace/Bag ", 12);
                        AddOption("Ring/Bracelet ", 16);
                        AddOption("Right Weapon ", 14);
                        AddOption("Shield/Left Weapon ", 15);
                        AddOption("Armor ", 13);
                        AddOption("Boots ", 18);
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
                            AddText("There must be some mistake. You must be wearing an item before you may upgrade it!");
                            AddOption("Nevermind", 255);
                            break;
                        }
                        cost = equipment.Bless;

                        if (equipment.Bless == 0)
                        {
                            AddText("Your item must be blessed in order to upgrade this attribute!");
                            AddOption("Nevermind", 255);
                            break;
                        }

                        if (equipment.Bless == 7)
                        {
                            AddText("Your item has maximum bless attribute!");
                            AddOption("Nevermind", 255);
                            break;
                        }

                        if (hasAgreed && !_client.HasItem(700073, cost))
                        {
                            AddText("To upgrade this item you need to pay " + cost + " Super TortoiseGems witch you don't have!");
                            AddOption("I see.", 255);
                        }
                        else if (hasAgreed && _client.HasItem(700073, cost))
                        {

                            equipment.Bless += 2;
                            for (var i = 0; i < cost; i++)
                                _client.DeleteItem(700073);
                            _client.Send(ItemInformationPacket.Create(equipment, Enum.ItemInfoAction.Update));
                            equipment.Save();
                            Managers.PlayerManager.SendToServer(new TalkPacket(ChatType.System, "Congratulations you have upgraded you items blessed attribute!"));

                        }
                        else
                        {
                            hasAgreed = true;
                            AddText("To upgrade this item you need to pay " + cost + " Super TortoiseGems! Are you sure?");
                            AddOption("Yes.", (byte)_linkback);
                            AddOption("No thanks.", 255);

                        }
                        break;

                    }

            }
            AddFinish();
            Send();
        }
    }
}