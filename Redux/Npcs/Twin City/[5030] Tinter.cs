using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;
using Redux.Enum;
using Redux.Structures;

namespace Redux.Npcs
{

    public class NPC_5030 : INpc
    {
        /// <summary>
        /// Handles NPC usage for [5030] Tinter
        /// </summary>
        public NPC_5030(Game_Server.Player _client)
            : base(_client)
        {
            ID = 5030;
            Face = 7;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("I am the master of all colors. What equipment would you like to dye today?");
                    AddOption("Helmet", 1);
                    AddOption("Armor", 2);
                    AddOption("Shield", 3);
                    AddOption("Nevermind", 255);
                    break;
                case 1:
                    {
                        ConquerItem equipment;
                        if (_client.TryGetEquipmentByLocation(ItemLocation.Helmet, out equipment) && equipment.EquipmentSort == 1)
                        {
                            AddText("What color would you like?");
                            AddOption("White", 13);
                            AddOption("Blue", 14);
                            AddOption("Red", 15);
                            AddOption("Purple", 16);
                            AddOption("Yellow", 17);
                            AddOption("Nevermind", 255);
                        }
                        else
                        {
                            AddText("You are not wearing a helmet. I don't do hair!");
                            AddOption("Sorry", 255);
                        }
                    }
                    break;
                case 2:
                    {
                        ConquerItem equipment;
                        if (_client.TryGetEquipmentByLocation(ItemLocation.Armor, out equipment) && equipment.EquipmentSort == 3)
                        {
                            AddText("What color would you like?");
                            AddOption("White", 33);
                            AddOption("Blue", 34);
                            AddOption("Red", 35);
                            AddOption("Purple", 36);
                            AddOption("Yellow", 37);
                            AddOption("Nevermind", 255);
                        }
                        else
                        {
                            AddText("You are not wearing armor. I don't do tattoos!");
                            AddOption("Sorry", 255);
                        }
                        break;
                    }
                case 3:
                    {
                        ConquerItem equipment;
                        if (_client.TryGetEquipmentByLocation(ItemLocation.WeaponL, out equipment) && equipment.EquipmentType == 900)
                        {
                            AddText("What color would you like?");
                            AddOption("White", 43);
                            AddOption("Blue", 44);
                            AddOption("Red", 45);
                            AddOption("Purple", 46);
                            AddOption("Yellow", 47);
                            AddOption("Nevermind", 255);
                        }
                        else
                        {
                            AddText("You are not wearing a Shield.");
                            AddOption("Sorry", 255);
                        }
                        break;
                    }

                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                    {
                        ConquerItem equipment;
                        if (_client.TryGetEquipmentByLocation(ItemLocation.Helmet, out equipment) && equipment.EquipmentSort == 1)
                        {
                            equipment.Color = (byte)(_linkback % 10);
                            equipment.Save();
                            _client.Send(ItemInformationPacket.Create(equipment, ItemInfoAction.Update));
                            _client.SendToScreen(SpawnEntityPacket.Create(_client));
                        }
                        else
                        {
                            AddText("You are not wearing a helmet. I don't do hair!");
                            AddOption("Sorry", 255);
                        }
                        break;
                    }

                case 33:
                case 34:
                case 35:
                case 36:
                case 37:
                    {
                        ConquerItem equipment;
                        if (_client.TryGetEquipmentByLocation(ItemLocation.Armor, out equipment) && equipment.EquipmentSort == 3)
                        {
                            equipment.Color = (byte)(_linkback % 10);
                            equipment.Save();
                            _client.Send(ItemInformationPacket.Create(equipment, ItemInfoAction.Update));
                            _client.SendToScreen(SpawnEntityPacket.Create(_client));
                        }
                        else
                        {
                            AddText("You are not wearing armor. I don't do tattoos!");
                            AddOption("Sorry", 255);
                        }
                        break;
                    }

                case 43:
                case 44:
                case 45:
                case 46:
                case 47:
                    {
                        ConquerItem equipment;
                        if (_client.TryGetEquipmentByLocation(ItemLocation.WeaponL, out equipment) && equipment.EquipmentType == 900)
                        {
                            equipment.Color = (byte)(_linkback % 10);
                            equipment.Save();
                            _client.Send(ItemInformationPacket.Create(equipment, ItemInfoAction.Update));
                            _client.SendToScreen(SpawnEntityPacket.Create(_client));
                        }
                        else
                        {
                            AddText("You are not wearing a Shield.");
                            AddOption("Sorry", 255);
                        }
                        break;
                    }
            }
            AddFinish();
            Send();

        }
    }
}