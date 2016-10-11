using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{

    public class NPC_601 : INpc
    {
        /// <summary>
        /// Handles NPC usage for [601]OfflineTGAdmin
        /// </summary>
        public NPC_601(Game_Server.Player _client)
            : base(_client)
        {
            ID = 601;
            Face = 4;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    _client.Send(OfflineInfoPacket.Create(_client));
                    break;
            }
            AddFinish();
            Send();

        }
    }
}