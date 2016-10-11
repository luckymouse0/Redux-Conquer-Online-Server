using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{

    /// <summary>
    /// Handles NPC usage for [390] Love Stone
    /// </summary>
    public class NPC_390 : INpc
    {

        public NPC_390(Game_Server.Player _client)
            : base(_client)
        {
            ID = 390;
            Face = 6;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("Sometimes, you may feel lonely. Yeah, you do need someone to be with you. I can understand that. Now answer me, have you ever ");
                    AddText("fallen in love with someone?");
                    AddOption("Maybe, what should I do?", 1);
                    AddOption("Never.", 255);
                    break;
                case 1:
                    if (_client.Spouse != Constants.DEFAULT_MATE)
                    {
                        AddText("It seems you've already found your true love. Be sure to cherish them daily");
                        AddOption("I will.", 255);
                    }
                    else
                    {
                        AddText("Marriage is not a decision to be taken lightly and means spending your life with your lover. Are you positive you've found your mate?");
                        AddOption("Positive!", 2);
                        AddOption("Maybe not...", 255);
                    }
                    break;
                case 2:
                    if (_client.Spouse == Constants.DEFAULT_MATE)
                    {
                        AddText("By sending this flower to your lover you are giving yourself to them. They may chose to accept or reject your offer.");
                        AddOption("Thank you.", 255);
                        var p = Packets.Game.GeneralActionPacket.Create(_client.UID, Enum.DataAction.OpenCustom, 1067);
                        //p.Data2Low = _client.X;
                        //p.Data2High = _client.Y;
                        _client.Send(p);
                    }
                    break;
            }
            AddFinish();
            Send();

        }
    }
}
