using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{

    /// <summary>
    /// Handles NPC usage for [4450] Market Controller
    /// </summary>
    public class NPC_4450 : INpc
    {

        public NPC_4450(Game_Server.Player _client)
            : base(_client)
        {
            ID = 4450;
            Face = 1;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("Do you want to leave the market? I can teleport you for free.");
                    AddOption("Yeah. Thanks", 1);
                    AddOption("No. I shall stay here.", 255);
                    break;
                case 1:
                    _client.ChangeMap((ushort)_client.Character.Map);
                    break;
            }
            AddFinish();
            Send();

        }
    }
}
