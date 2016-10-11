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
    /// Handles NPC usage for [10002] Barber
    /// </summary>
    public class NPC_10002 : INpc
    {

        public NPC_10002(Game_Server.Player _client)
            : base(_client)
        {
            ID = 10002;
            Face = 20;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("Hi, I can offer you a variety of hairstyles. Would you like to make a payment of 500 silvers to change your hairstyle?");
                    AddOption("Yes, Nostalgic styles.", 1);
                    AddOption("Just passing by.", 255);
                    break;
                case 1:
                    AddText("Please choose a desired HairStyle below:");
                    AddOption("~Nostalgic 1~", 11);
                    AddOption("~Nostalgic 2~", 12);
                    AddOption("~Nostalgic 3~", 13);
                    AddOption("~Nostalgic 4~", 14);
                    AddOption("~Nostalgic 5~", 15);
                    AddOption("~Nostalgic 6~", 16);
                    AddOption("~Nostalgic 7~", 17);
                    AddOption("Not interested.", 255);
                    break;
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                    {
                        if (_client.Money >= 500)
                        {
                            _client.Money -= 500;
                            _client.Hair = (ushort)((Math.Floor((decimal)(_client.Hair / 100)) * 100) + _linkback);
                            _client.Send(new UpdatePacket(_client.UID, UpdateType.Hair, _client.Hair));
                            AddText("There you go, your 'Nostalgic' hairstyle as requested.");
                            AddOption("Thank you.", 255);
                        }
                        else
                        {
                            AddText("Please come back when you have 500 Silvers.");
                            AddOption("I see.", 255);
                        }
                    }
                    break;
            }
            AddFinish();
            Send();
        }
    }
}