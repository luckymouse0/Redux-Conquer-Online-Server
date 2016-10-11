using System;
using Redux.Game_Server;
using Redux.Structures;
using Redux.Managers;
using Redux.Packets.Game;

namespace Redux.Items
{
    /// <summary>
    /// Handles item usage for [721020] MoonBox
    /// </summary>
    public class Item_721020 : IItem
    {
        public override void Run(Player _client, ConquerItem _item)
        {
            if (_client.Inventory.Count > 37)
                return;

            _client.DeleteItem(_item);

            for (var i = 0; i < 3; i++)
            {
                int randomNumber = Common.Random.Next(10000);

                if (randomNumber == 0)
                {
                    switch (Common.Random.Next(34))
                    {
                        case 0: _client.CreateItem((uint)(181305)); break;
                        case 1: _client.CreateItem((uint)(181315)); break;
                        case 2: _client.CreateItem((uint)(181325)); break;
                        case 3: _client.CreateItem((uint)(181335)); break;
                        case 4: _client.CreateItem((uint)(181345)); break;
                        case 5: _client.CreateItem((uint)(181355)); break;
                        case 6: _client.CreateItem((uint)(181365)); break;
                        case 7: _client.CreateItem((uint)(181375)); break;
                        case 8: _client.CreateItem((uint)(181385)); break;
                        case 9: _client.CreateItem((uint)(181405)); break;
                        case 10: _client.CreateItem((uint)(181415)); break;
                        case 11: _client.CreateItem((uint)(181425)); break;
                        case 12: _client.CreateItem((uint)(181505)); break;
                        case 13: _client.CreateItem((uint)(181515)); break;
                        case 14: _client.CreateItem((uint)(181525)); break;
                        case 15: _client.CreateItem((uint)(181605)); break;
                        case 16: _client.CreateItem((uint)(181615)); break;
                        case 17: _client.CreateItem((uint)(181625)); break;
                        case 18: _client.CreateItem((uint)(181705)); break;
                        case 19: _client.CreateItem((uint)(181715)); break;
                        case 20: _client.CreateItem((uint)(181725)); break;
                        case 21: _client.CreateItem((uint)(181805)); break;
                        case 22: _client.CreateItem((uint)(181815)); break;
                        case 23: _client.CreateItem((uint)(181825)); break;
                        case 24: _client.CreateItem((uint)(181905)); break;
                        case 25: _client.CreateItem((uint)(181915)); break;
                        case 26: _client.CreateItem((uint)(181925)); break;
                        case 27: _client.CreateItem((uint)(182305)); break;
                        case 28: _client.CreateItem((uint)(182315)); break;
                        case 29: _client.CreateItem((uint)(182325)); break;
                        case 30: _client.CreateItem((uint)(182385)); break;
                        case 31: _client.CreateItem((uint)(182365)); break;
                        case 32: _client.CreateItem((uint)(182345)); break;
                        case 33: _client.CreateItem((uint)(182335)); break;
                    }
                }
                else if((randomNumber > 0)&&(randomNumber <= 5000))
                {
                    _client.CreateItem((uint)(Constants.METEOR_ID));
                }
                else if((randomNumber > 5000)&&(randomNumber <= 7500))
                {
                    _client.CreateItem((uint)(Constants.DRAGONBALL_ID));
                }
                else
                {
                    //Codigo Gears
                    int randomLevel = Common.Random.Next(125);                    
                    int randomQuality = Common.Random.Next(100);
                    uint id;

                    if (randomQuality == 0)
                    {
                        id = DropManager.GenerateDropID((byte)randomLevel, (ushort)9);  //SUPER
                    }
                    else if((randomQuality > 0)&&(randomQuality <= 5))
                    {
                        id = DropManager.GenerateDropID((byte)randomLevel, (ushort)8);  //ELITE
                    }
                    else if((randomQuality > 5)&&(randomQuality <= 35))
                    {
                        id = DropManager.GenerateDropID((byte)randomLevel, (ushort)7);  //UNIQUE
                    }
                    else
                    {
                        id = DropManager.GenerateDropID((byte)randomLevel, (ushort)6);  //REFINED
                    }

                    var itemInfo = Database.ServerDatabase.Context.ItemInformation.GetById(id);
                    var coItem = new ConquerItem((uint)Common.ItemGenerator.Counter, itemInfo);

                    if ((coItem.EquipmentSort == 1 || coItem.EquipmentSort == 3 || coItem.EquipmentSort == 4) && coItem.BaseItem.TypeDesc != "Earring")
                            coItem.Color = (byte)Common.Random.Next(3, 7);

                    if (coItem.IsWeapon)     //Socket Weapons
                    {
                        if (Common.PercentSuccess(7))
                        {
                            coItem.Gem1 = 255;

                            if (Common.PercentSuccess(7))
                            {
                                coItem.Gem2 = 255;
                            }
                        }
                    }
                    else    //Socket other Equipment
                    {
                        if (Common.PercentSuccess(0.01))
                        {
                            coItem.Gem1 = 255;

                            if (Common.PercentSuccess(0.01))
                            {
                                coItem.Gem2 = 255;
                            }
                        }
                    }

                    coItem.SetOwner(_client);
                    if (_client.AddItem(coItem))
                        _client.Send(ItemInformationPacket.Create(coItem));
                }
            }
        }
    }
}
