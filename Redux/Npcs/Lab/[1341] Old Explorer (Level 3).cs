
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{
    /// <summary>
    /// Handles NPC usage for [9140] Old Explorer
    /// </summary>
    public class NPC_1341 : INpc
    {

        public NPC_1341(Game_Server.Player _client)
            : base(_client)
        {
            ID = 1341;
            Face = 32;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("Do you wish to enter second level of labyrinth?Give me an SoulToken");
                    AddOption("Yes, send me to the quest map.", 1);
                    AddOption("No Thanks.", 255);
                    break;
                case 1:
                    if (_client.HasItem(721539, 1))
                    {
                        _client.ChangeMap(1354, 321, 123);
                        _client.DeleteItem(721539);
                        AddText("There you go, take care! ");
                        AddOption("Okay.", 255);
                    }
                    else
                    {
                        AddText("You don't have an SoulToken!");
                        AddOption("Nevermind...", 255);

                    }
                    break;
            }
            AddFinish();
            Send();
        }
    }
}