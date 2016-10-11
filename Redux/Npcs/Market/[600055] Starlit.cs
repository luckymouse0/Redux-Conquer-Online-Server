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
    /// Handles NPC usage for [600055]  Starlit
    /// </summary>
    public class NPC_600055 : INpc
    {

        public NPC_600055(Game_Server.Player _client)
            : base(_client)
        {
            ID = 10062;
            Face = 24;
        }
        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("I can help you to divorce your spouse. You just will have to agree to a divorce. I will also need a MeteorTear. If you have this item and you really want to get divorced, we will do it soon.");
                    AddOption("Yes, I want a divorce.", 1);
                    AddOption("Just passing by.", 255);
                    break;
                case 1:
                    if (_client.Spouse  == Constants.DEFAULT_MATE)
                    {
                        AddText("You really sure you want to divorce your spouse now? You will not regret later?");
                        AddOption("No I'll not. Let's do this.", 2);
                        AddOption("I prefer to remain married.", 255);
                    }
                    else
                    {
                        AddText("Sorry, I can not divorce you if you are not married.");
                        AddOption("Okay.", 255);
                    }
                    break;
                case 2:
                    AddText("Are you ready? Be sure you're with MeteorTear, I really need it.");
                    AddOption("Yeah. I am ready.", 3);
                    AddOption("Let me think it over.", 255);
                    break;
                case 3:
                    if (_client.Spouse == Constants.DEFAULT_MATE && _client.HasItem(Constants.METEOR_TEAR_ID))
                    {
                        var dbSpouse = Database.ServerDatabase.Context.Characters.GetByName(_client.Spouse);
                        if(dbSpouse != null)
                        {
                            dbSpouse.Spouse = Constants.DEFAULT_MATE;
                            Database.ServerDatabase.Context.Characters.AddOrUpdate(dbSpouse);

                            var meteorTear = _client.GetItemByID(Constants.METEOR_TEAR_ID);
                            _client.RemoveItem(meteorTear);
                            meteorTear.DbItem.Owner = dbSpouse.UID;
                            meteorTear.Save();

                            var spouse = Managers.PlayerManager.GetUser(_client.Spouse);
                            if (spouse != null)
                            {
                                spouse.AddItem(meteorTear);
                                spouse.Send(ItemInformationPacket.Create(meteorTear));

                                //sanity check incase they log out while it's being saved above or something weird
                                spouse.Spouse = Constants.DEFAULT_MATE;
                                spouse.Send((StringsPacket.Create(spouse.UID, StringAction.Mate, Constants.DEFAULT_MATE)));
                            }

                            Managers.PlayerManager.SendToServer(new TalkPacket(ChatType.GM, "Unfortunately! " + _client.Name + " and " + _client.Spouse + " got divorced."));

                            _client.Spouse = Constants.DEFAULT_MATE;
                            _client.Save();
                            AddText("Done, you are now divorced. You are free to find another passion around. Be happy my friend.");
                            AddOption("Thank you.", 255);
                        }
                    }
                    else
                    {
                        AddText("Sorry, I can not divorce you if you don't have a MeteorTear.");
                        AddOption("Okay.", 255);
                    }
                    break;
            }
            AddFinish();
            Send();
        }
    }
}
