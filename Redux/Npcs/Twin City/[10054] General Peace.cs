using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{

    public class NPC_10054 : INpc
    {
        /// <summary>
        /// Handles NPC usage for [10054] General Peace
        /// </summary>
        public NPC_10054(Game_Server.Player _client)
            : base(_client)
        {
            ID = 10054;
            Face = 7;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("This is the way to the Desert City. Although you are excellent, it is dangerous to go ahead.");
                    AddOption("I want to go.", 1);
                    AddOption("I think I will stay here", 255);
                    break;
                case 1:
                    _client.ChangeMap(1000, 971, 666);
                    break;
            }
            AddFinish();
            Send();
        }
    }
}