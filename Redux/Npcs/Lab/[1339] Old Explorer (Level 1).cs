using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{
    /// <summary>
    /// Handles NPC usage for [1339] Old Explorer
    /// </summary>
    public class NPC_1339 : INpc
    {

        public NPC_1339(Game_Server.Player _client)
            : base(_client)
        {
            ID = 1339;
            Face = 32;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("Do you wish to enter second level of labyrinth? Give me an SkyToken");
                    AddOption("Yes, send me to the quest map.", 1);
                    AddOption("No Thanks.", 255);
                    break;
                case 1:
                    if (_client.HasItem(721537, 1))
                    {
                        _client.ChangeMap(1352, 104, 108);
                        _client.DeleteItem(721537);
                        AddText("There you go, take care! ");
                        AddOption("Okay.", 255);
                    }
                    else
                    {
                        AddText("You don't have an SkyToken!");
                        AddOption("Nevermind...", 255);

                    }
                    break;
            }
            AddFinish();
            Send();
        }
    }
}