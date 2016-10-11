using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Structures;
using Redux.Enum;
using Redux.Packets.Game;
namespace Redux.Npcs
{

    /// <summary>
    /// Handles NPC usage for [3825] Unknown Man
    /// </summary>
    public class NPC_3825 : INpc
    {

        public NPC_3825(Game_Server.Player _client)
            : base(_client)
        {
            ID = 3825;
            Face = 67;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0: 
                    if (_client.Tasks.ContainsKey(TaskType.UnknownMan) && DateTime.Now < _client.Tasks[TaskType.UnknownMan].Expires)
                    {
                        AddText("I need some time to recover. Come see me tomorrow");
                        AddOption("I shall", 255);
                    }
                    else if (_client.HasItem(722185))
                    {
                        AddText("Is that wine for me? I'm so very thirsty!");
                        AddOption("Here you go.", 2);
                        AddOption("No! Get away from me", 255);
                    }
                    else
                    {
                        AddText("Hey, you there! Please take pity on a homeless old man! I am so hungry but can`t even afford a bowl of noodles. Could you help ");
                        AddText("me out here? The gods always smile on kindness, you know!");
                        AddOption("Here are 100 silvers. Take care.", 1);
                        AddOption("No way. I don`t do charity.", 255);
                    }
                    break;
                case 1:
                    AddText("Thank you so much stranger! The only thing that could improve this meal is some wine to wash it down. ");
                    AddText("Can you bring me some DrunkCelestial wine please?");
                    AddOption("I'll get it", 255);
                    break;
                case 2:
                    if (_client.HasItem(722185))
                    {
                        _client.DeleteItem(722185);
                        AddText("Ahhhh... that hit the spot. Your generosity is legendary! ");
                        AddText("Would you like this old man to help you unlock the secret of the DragonBalls?");
                        AddOption("Teach me please", 3);
                        AddOption("No thanks", 255);
                    }
                    break;
                case 3:
                    {
                        var cost = _client.Level > 99 ? 2 : 1;
                        AddText("I will require " + cost + " DragonBalls to help you unlock infinite power. This is your last chance to leave");
                        AddOption("Do it", 4);
                        AddOption("Maybe later", 255);
                        break;
                    }
                case 4:
                    {
                        var cost = _client.Level > 99 ? 2 : 1;
                        if(_client.Tasks.ContainsKey(TaskType.UnknownMan) && DateTime.Now < _client.Tasks[TaskType.UnknownMan].Expires)
                        {
                            AddText("I need some time to recover. Come see me tomorrow");
                            AddOption("I shall", 255);
                        }
                        else if (_client.HasItem(Constants.DRAGONBALL_ID, cost))
                        {
                            if (!_client.Tasks.ContainsKey(TaskType.UnknownMan))
                                _client.Tasks.TryAdd(TaskType.UnknownMan, new Task(_client.UID, TaskType.UnknownMan, DateTime.Now.AddHours(12)));
                            else
                                _client.Tasks[TaskType.UnknownMan].Expires = DateTime.Now.AddHours(12);
                            for (var i = 0; i < cost; i++)
                                _client.DeleteItem(Constants.DRAGONBALL_ID);
                            _client.GainExpBall(6000);
                            AddText("It is done! I need some time to recover before you come see me again.");
                            AddOption("Thank you", 255);
                        }
                        else
                        {
                            AddText("You do not have the " + cost + " DragonBalls I require. Come back when you have them");
                            AddOption("Yes sir", 255);
                            break;
                        }
                        break;
                    }

            }
            AddFinish();
            Send();

        }
    }
}
