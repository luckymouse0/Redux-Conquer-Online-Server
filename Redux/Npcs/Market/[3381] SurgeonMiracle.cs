using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;
using Redux.Enum;


namespace Redux.Npcs
{

    /// <summary>
    /// Handles NPC usage for [3381] Surgeon Miracle
    /// </summary>
    public class NPC_3381 : INpc
    {
        public NPC_3381(Game_Server.Player _client)
            : base(_client)
        {
            ID = 3381;
            Face = 54;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:

                    AddText("Hello, are you satisfield with your stature? I can change your body size for 1 DragonBall.");
                    AddText("Would you like to change it?");
                    AddOption("Yeah.", 1);
                    AddOption("No.", 255);
                    break;
                case 1:
                    AddText("Are you sure you want to do this?");
                    AddOption("Yes.", 2);
                    AddOption("I changed my mind.", 255);
                    break;

                case 2:
                    if (_client.HasItem(1088000))
                    {
                        _client.DeleteItem(1088000);
                        if (_client.Lookface % 2 == 0)
                            _client.Lookface--;
                        else
                            _client.Lookface++;
                        _client.Character.Lookface = _client.Lookface;
                        _client.Save();

                        AddText("There you go. I hope you are satisfied now!");
                        AddOption("Thanks!", 255);
                        _client.Save();
                    }
                    else
                    {
                        AddText("You don't have 1 DragonBall.");
                        AddOption("Nevermind ", 255);
                    }
                    break;
            }
            AddFinish();
            Send();

        }
    }
}