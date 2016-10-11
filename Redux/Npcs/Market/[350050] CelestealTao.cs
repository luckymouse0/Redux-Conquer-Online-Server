using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{

    /// <summary>
    /// Handles NPC usage for [350050] CelestealTao
    /// </summary>
    public class NPC_350050 : INpc
    {
        private uint stats;
        bool hasAgreed = false;

        public NPC_350050(Game_Server.Player _client)
            : base(_client)
        {
            ID = 350050;
            Face = 54;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    AddText("The DragonBall is really a precious item.You can use it to reallot your attribute points after you have been reborned.");
                    AddText("After reborning at 70 or above you can reallot your attribute points freely.");
                    AddText(" Well, if you have a DragonBall, I can help you do that.");
                    AddOption("Please help me.", 1);
                    AddOption("Definitely.", 255);
                    break;
                case 1:
                    if (_client.RebornCount < 1)
                    {

                        AddText("Please come back when you have been reborned at least once in your lifetime?");
                        AddOption("I see.", 255);
                        break;
                    }
                    if (_client.Level < 70)
                    {

                        AddText("You must be level 70 or above to be able to reallot attribute points!");
                        AddOption("Thanks. I see.", 255);
                        break;
                    }
                    if (!_client.HasItem(1088000))
                    {
                        AddText("Sorry, but you must pay one DragonBall come back when you have one!");
                        AddOption("I see!", 255);
                        break;

                    }
                    else if (hasAgreed)
                    {
                        stats = (uint)(_client.Character.Strength + _client.Character.Vitality + _client.Character.Agility + _client.Character.Spirit + _client.ExtraStats);
                        _client.DeleteItem(1088000);

                        _client.Strength = 0;
                        _client.Vitality = 8;
                        _client.Agility = 2;
                        _client.Spirit = 0;
                        _client.ExtraStats = (ushort)(stats - 10);
                        _client.Recalculate(true);
                        _client.Save();

                        AddText("There you go, allot them as you wish!");
                        AddOption("I see!", 255);
                    }
                    else
                    {
                        hasAgreed = true;
                        AddText("Are you sure you want to reallot your attribute points?");
                        AddOption("Yes.", (byte)_linkback);
                        AddOption("No thanks.", 255);
                    }
                    break;


            }
            AddFinish();
            Send();

        }
    }
}