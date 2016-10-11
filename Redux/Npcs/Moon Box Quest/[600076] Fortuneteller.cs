using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{
    /// <summary>
    /// Handles NPC usage for [60076] FortuneTeller
    /// Written by Pro4Never
    /// </summary>
    public class NPC_600076 : INpc
    {

        public NPC_600076(Game_Server.Player _client)
            : base(_client)
        {
            ID = 600076;
            Face = 20;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("I can tell your future. Would you like me to tell you what awaits you?");
                    AddOption("Tell me", 1);
                    AddOption("I prefer the mystery.", 255);

                    break;
                case 1:
                    //TODO: Teleport to MB quest
                    break;
            }
            AddFinish();
            Send();
        }
    }
}