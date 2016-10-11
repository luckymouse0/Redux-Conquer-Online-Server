using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{

    /// <summary>
    /// Handles NPC usage for [923] LadyLuck
    /// </summary>
    public class NPC_923 : INpc
    {

        public NPC_923(Game_Server.Player _client)
            : base(_client)
        {
            ID = 923;
            Face = 3;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("I always have the worst luck...");
                    AddOption("Sorry to hear that.", 255);
                    break;
            }
            AddFinish();
            Send();

        }
    }
}
