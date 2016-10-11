using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{

    public class NPC_1337 : INpc
    {
        /// <summary>
        /// Handles NPC usage for [1337] Welcomer
        /// </summary>
        public NPC_1337(Game_Server.Player _client)
            : base(_client)
        {
            ID = 1337;
            Face = 1;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("Welcome to the Redux public beta! Commands are enabled for all players to test content. ");
                    AddText("Please use the /report command to log bugs or missing features so we can finish the server quickly!");
                    AddOption("Thanks", 255);
                    break;
            }
            AddFinish();
            Send();

        }
    }
}