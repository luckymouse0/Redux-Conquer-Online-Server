using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{

    public class NPC_50020 : INpc
    {
        /// <summary>
        /// Handles NPC usage for [50020] Milly
        /// </summary>
        public NPC_50020(Game_Server.Player _client)
            : base(_client)
        {
            ID = 50020;
            Face = 4;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    if (_client.HasItem(721000))
                    {
                        AddText("My sister is in Bird Island. She fell in love with Joe there. Since Joe left her with no news, she stays there to wait for him.");
                        AddOption("I see.", 255);
                    }
                    else
                    {
                        AddText("I cannot get near you now. You have got me going crazy. Wherever you go, whatever you do, I will be right here waiting for you.");
                        AddOption("Why are you so sad?", 1);
                        AddOption("I got to go.", 255);
                    }
                    break;
                case 1:
                    AddText("My sister taught me that song, Her lover, Joe, is a brave and kind man. All said they make a good couple, but he left her.");
                    AddOption("What a pity.", 2);
                    AddOption("Another old love story.", 255);
                    break;
                case 2:
                    AddText("My sister believes that Joe still loves her. She stays in Bird Island waiting for him. Can you take my letter to her?");
                    AddOption("OK. I shall visit her.", 3);
                    AddOption("Just passing by.", 255);
                    break;
                case 3:
                    if (_client.Inventory.Count < 40)
                        _client.CreateItem(721000);
                    else
                    {
                        AddText("Your inventory is full, please leave a slot to take this letter.");
                        AddOption("Ok.", 255);
                        break;
                    }
                    break;
            }
            AddFinish();
            Send();

        }
    }
}