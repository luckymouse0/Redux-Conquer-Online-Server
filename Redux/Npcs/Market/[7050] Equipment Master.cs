using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{

    /// <summary>
    /// Handles NPC usage for [7050] Eqipment Master
    /// </summary>
    public class NPC_7050 : INpc
    {

        public NPC_7050(Game_Server.Player _client)
            : base(_client)
        {
            ID = 7050;
            Face = 9;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {

                case 0:
                    {
                        AddText("Hello, did you noticed that MagicArtisan can only level items up to 120?");
                        AddText("I can upgrade it above 120 up to 130 ,but it costs 1 DragonBall for each upgrade.");
                        AddOption("Yes, I want.", 2);
                        AddOption("No. I like it this way.", 255);
                        break;
                    }
                case 2:
                    {
                        AddText("It costs 1 DragonBall for any item above level 110.Are you really sure you want to do that?! ");
                        AddOption("Yeah, upgrade it! ", 3);
                        AddOption("No. I like it this way.", 255);
                        break;
                    }
                case 3:
                    {
                        AddText("Each upgrade requires 1 DragonBall and there is no turning back! ");
                        AddText("What item would you like me to upgrade?");
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
                        }
                        else if (!_client.HasItem(1088000))
                        {
                            AddText("There must be some mistake. You must pay an DragonBall before you may upgrade it!");
                            AddOption("Nevermind", 255);
                        }
                        else if (equipment.EquipmentLevel <= 110)
                        {
                            AddText("There must be some mistake. Your item is not above level 110!");
                            AddOption("Nevermind", 255);

                        }
                        else if (equipment.GetNextItemLevel() == equipment.StaticID)
                        {
                            AddText("There must be some mistake. Your item can't be upgraded anymore !");
                            AddOption("Nevermind", 255);

                        }

                        else if (_client.Level < equipment.GetDBItemByStaticID(equipment.GetNextItemLevel()).LevelReq)
                        {
                            AddText("There must be some mistake. You are not high level enough to wear the item after upgrade!");
                            AddOption("Nevermind", 255);
                        }

                        else
                        {
                            equipment.ChangeItemID(equipment.GetNextItemLevel());
                            _client.DeleteItem(1088000);
                            _client.Send(ItemInformationPacket.Create(equipment, Enum.ItemInfoAction.Update));
                            equipment.Save();
                        }
                        break;
                    }


            }
            AddFinish();
            Send();

        }
    }
}