using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Enum;
using Redux.Managers;
using Redux.Database;
using Redux.Database.Domain;
using Redux.Structures;
using Redux.Game_Server;
using Redux.Packets.Game;

namespace Redux.Managers
{
    public class TradeSequence
        {
            public Player Owner;//person to initially request trade
            public Player Target;//person to receive the request
            public bool WindowOpen = false;
            public bool OwnerConfirmed = false;
            public bool TargetConfirmed = false;
            public bool Accepted = false;
            private uint OwnerMoney = 0;
            private uint TargetMoney = 0;
            private uint OwnerCP = 0;
            private uint TargetCP = 0;
            private List<Structures.ConquerItem> OwnerItems = new List<Structures.ConquerItem>();
            private List<Structures.ConquerItem> TargetItems = new List<Structures.ConquerItem>();

            
            public void ProcessTradePacket(Player sender, TradePacket packet)
            {
                switch (packet.Subtype)
                {
                    #region Trade Request
                    case TradeType.Request:

                        if (Target == null)//No request has already been sent!
                        {
                            if (PlayerManager.Players.ContainsKey(packet.Target))
                                Target = PlayerManager.GetUser(packet.Target);
                            else//Target is not a client
                            {
                                Owner.SendMessage("Invalid target for trade!", ChatType.System);
                                Owner.Trade = null;
                                return;
                            }
                            if (Target.Trade == null)//Target is not trading! Lets send request
                            {
                                packet.Target = Owner.UID;
                                Target.Send(packet);
                                Target.Trade = this;
                            }
                            else//Target is trading, Shut down this request!
                            {
                                Owner.SendMessage(Target.Name + " is already trading, please try later", ChatType.System);
                                Owner.Trade = null;
                                return;
                            }
                        }
                        else
                        {
                            packet.Subtype = TradeType.ShowTable;
                            packet.Target = Target.UID;
                            Owner.Send(packet);
                            packet.Target = Owner.UID;
                            Target.Send(packet);
                        }
                        break;
                    #endregion
                    #region Trade Timeout
                    case TradeType.TimeOut:
                        Owner.Trade = null;
                        if (Target != null)
                        { Target.Trade = null; }
                        break;
                    #endregion
                    #region AddItem
                    case TradeType.AddItem:
                        {
                            var _item = sender.GetItemByUID(packet.Target);
                            if (_item != null && _item.IsTradeable)
                            {                               
                                if (sender == Owner)
                                {
                                    if (Target.Inventory.Count + OwnerItems.Count < 40)
                                    {
                                        OwnerItems.Add(_item);
                                        Target.Send(ItemInformationPacket.Create(_item, ItemInfoAction.Trade));
                                    }
                                    else//full inv on Target
                                    {
                                        Owner.SendMessage(Target.Name + " cannot hold more items", ChatType.System);
                                    }
                                }
                                else if (sender == Target)
                                {
                                    if (Owner.Inventory.Count + TargetItems.Count < 40)
                                    {
                                        TargetItems.Add(_item);
                                        Owner.Send(ItemInformationPacket.Create(_item, ItemInfoAction.Trade));
                                    }
                                    else//full inv on Target
                                    {
                                        Owner.SendMessage(Target.Name + " cannot hold more items", ChatType.System);
                                    }
                                }
                            }
                            break;
                        }
                    #endregion
                    #region Accept
                    case TradeType.Accept:
                        {
                            if (!Accepted)
                            {
                                if (sender == Owner)
                                {
                                    packet.Target = Owner.UID;
                                    Target.Send(packet);
                                }
                                else if (sender == Target)
                                {
                                    packet.Target = Target.UID;
                                    Owner.Send(packet);
                                }
                                Accepted = true;
                            }

                            else
                            {
                                bool success = true;
                                foreach (Structures.ConquerItem _item in OwnerItems)
                                    if (!Owner.Inventory.ContainsKey(_item.UniqueID))
                                    {
                                        success = false;
                                        break;
                                    }
                                foreach (Structures.ConquerItem _item in TargetItems)
                                    if (!Target.Inventory.ContainsKey(_item.UniqueID))
                                    {
                                        success = false;
                                        break;
                                    }
                                if (Owner.Money < OwnerMoney)
                                    success = false;
                                if (Target.Money < TargetMoney)
                                    success = false;
                                if (Owner.CP < OwnerCP)
                                    success = false;
                                if (Target.CP < TargetCP)
                                    success = false;
                                
                                packet.Subtype = TradeType.HideTable;
                                Owner.Send(packet);
                                Target.Send(packet);

                                if (success)
                                {
                                    foreach (Structures.ConquerItem _item in TargetItems)
                                    {
                                        Owner.AddItem(_item);
                                        Owner.Send(ItemInformationPacket.Create(_item, ItemInfoAction.AddItem));
                                        _item.SetOwner(Owner);

                                        Target.Send(new ItemActionPacket
                                        {
                                            ActionType = ItemAction.RemoveInventory,//might be wrong
                                            UID = _item.UniqueID,
                                        });
                                        //Target.Send(ItemActionPacket.Create(_item.UniqueID, _item.StaticID, ItemAction.RemoveInventory));
                                        if (!Target.RemoveItem(_item))
                                            Console.WriteLine("Error with removing player item. " + Target.Name);

                                        


                                    }
                                    foreach (Structures.ConquerItem _item in OwnerItems)
                                    {
                                        Target.AddItem(_item);
                                        Target.Send(ItemInformationPacket.Create(_item, ItemInfoAction.AddItem));
                                        _item.SetOwner(Target);
                                        Owner.Send(new ItemActionPacket
                                        {
                                            ActionType = ItemAction.RemoveInventory,//might be wrong
                                            UID = _item.UniqueID,
                                        });
                                        //Owner.Send(ItemActionPacket.Create(_item.UniqueID, _item.StaticID, ItemAction.RemoveInventory));
                                        if (!Owner.RemoveItem(_item))
                                            Console.WriteLine("Error with removing player item. " + Owner.Name);
                                    }
                                    Owner.Money -= OwnerMoney;
                                    Target.Money += OwnerMoney;

                                    Target.Money -= TargetMoney;
                                    Owner.Money += TargetMoney;

                                    Target.CP -= TargetCP;
                                    Owner.CP += TargetCP;

                                    Owner.CP -= OwnerCP;
                                    Target.CP += OwnerCP;

                                    Target.SendMessage("Trade Successful", ChatType.System);
                                    Owner.SendMessage("Trade Successful", ChatType.System);
                                }
                                else
                                {
                                    Target.SendMessage("Trade Failed", ChatType.System);
                                    Owner.SendMessage("Trade Failed", ChatType.System);
                                }

                                Owner.Trade = null;
                                Target.Trade = null;
                            }

                            break;
                        }
                    #endregion
                    #region AddCp
                    case TradeType.SetConquerPoints:
                        {
                            packet.Subtype = TradeType.ShowConquerPoints;
                            if (sender.CP >= packet.Target)
                            {
                                if (Owner == sender)
                                {
                                    OwnerCP = packet.Target;
                                    Target.Send(packet);
                                }
                                else if (Target == sender)
                                {
                                    TargetCP = packet.Target;
                                    Owner.Send(packet);
                                }
                            }
                            else
                                Console.WriteLine("not enough cp");
                            break;
                        }
                    #endregion
                    #region AddMoney
                    case TradeType.SetMoney:
                        {
                            packet.Subtype = TradeType.ShowMoney;
                            if (sender.Money >= packet.Target)
                            {
                                if (Owner == sender)
                                {
                                    OwnerMoney = packet.Target;
                                    Target.Send(packet);
                                }
                                else if (Target == sender)
                                {
                                    TargetMoney = packet.Target;
                                    Owner.Send(packet);
                                }
                            }
                            else
                                Console.WriteLine("not enough gold");
                            break;
                        }
                    #endregion
                    #region Close Trade
                    case TradeType.Close:
                        Owner.Trade = null;
                        if (Target != null)
                        {
                            packet.Subtype = TradeType.HideTable;
                            packet.Target = Target.UID;
                            Owner.Send(packet);
                            packet.Target = Owner.UID;
                            Target.Send(packet);
                            Target.Trade = null;
                        }
                        break;
                    #endregion
                    default:
                        Console.WriteLine("Unhandled Trade Type: " + packet.Subtype);
                        break;
                }

            }
        }
        
    }

