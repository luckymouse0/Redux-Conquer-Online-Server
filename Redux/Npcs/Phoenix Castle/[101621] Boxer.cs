/*
 * User: cookc
 * Date: 9/21/2013
 * Time: 8:08 PM
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{
    /// <summary>
    /// Handles NPC usage for [101621] Boxer
    /// Written by pro4never 24/10/2014
    /// </summary>
    public class NPC_101621 : INpc
    {

        public NPC_101621(Game_Server.Player _client)
            : base(_client)
        {
            ID = 101621;
            Face = 20;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("If you are level 20 or above, you may level up at the training grounds. it does");
                    AddText("not consume any durability, life and mana. Do you want me to teleport you");
                    AddText("there? It is free for my guild members, and 1000 silver for others.");
                    AddOption("Please teleport me there.", 1);
                    AddOption("Just passing by.", 255);
                    break;
                case 1:
                    if (_client.Money >= 1000)
                    { _client.ChangeMap(1039, 217, 215); _client.Money -= 1000; }
                    else
                    {
                        AddText("Sorry, you do not have enough.");
                        AddOption("I see.", 255);
                    }
                    break;
            }
            AddFinish();
            Send();
        }
    }
}
