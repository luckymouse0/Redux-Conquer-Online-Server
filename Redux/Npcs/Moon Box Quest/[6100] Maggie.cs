using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{
    /// <summary>
    /// Handles NPC usage for [6100] Maggie
    /// Written by Matt
    /// </summary>
    public class NPC_6100 : INpc
    {

        public NPC_6100(Game_Server.Player _client)
            : base(_client)
        {
            ID = 6100;
            Face = 20;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    if (_client.HasItem(721010) && _client.HasItem(721011) && _client.HasItem(721012) &&
                        _client.HasItem(721013) && _client.HasItem(721014) && _client.HasItem(721015))
                    {
                        AddText("Wow. I can't believe you were able to obtain all six tokens. I will use these tokens to teleport you ");
                        AddText("to the life tatic where you can help some poor soul. When you teleport, the tokens will be formed into a soul jade.");
                        AddText("Give this jade to a any of LonelyGhost so save the ghosts soul. You will recieve a moonbox for your kindness. Are you ready?");
                        AddOption("Yes teleport me", 3);
                        AddOption("Not yet.", 255);
                    }
                    else
                    {
                        AddText("Hello " + _client.Name + ". My name is Maggie. I tried to conquer these tatics many years ago and was killed. ");
                        AddText("I chose to gaurd the entrance in the afterlife to stop others from following in my footsteps. If you chose to ");
                        AddText("ignore my warning, I will send you in. I cannot control what tatic I send you to unless you have all tokens.");
                        AddOption("Tokens?", 1);
                        AddOption("This is too dangerous, I must leave at once.", 255);
                    }
                    break;
                case 1:
                    AddText("There are six different tokens you must gain from killing monsters. The monsters in each tatic will drop a different ");
                    AddText("token. Once you have all six, I can can channel them for you and teleport you to the life tatic where you can help save ");
                    AddText("some poor souls. I should warn you, there is one more tatic you can be sent to called the Death Tatic. Like the name suggests, ");
                    AddText("if you get sent their, you will die. Good luck adventurer.");
                    AddOption("Send me to a tatic", 2);
                    AddOption("This is too dangerous, I must leave at once.", 255);
                    break;
                case 2:
                    if (Common.PercentSuccess(5.0))
                    {
                        //death tatic
                        _client.ChangeMap(1049, 211, 156);
                    }
                    else
                    {
                        //get random number between 0 and 100
                        int taticNum = Common.Random.Next(0, 101);

                        //Send to a tatic based off random number
                        if (taticNum < 17)
                            _client.ChangeMap(1043, 211, 156);
                        else if (taticNum < 33)
                            _client.ChangeMap(1044, 211, 156);
                        else if (taticNum < 49)
                            _client.ChangeMap(1045, 211, 156);
                        else if (taticNum < 65)
                            _client.ChangeMap(1046, 211, 156);
                        else if (taticNum < 81)
                            _client.ChangeMap(1047, 211, 156);
                        else
                            _client.ChangeMap(1048, 211, 156);
                    }
                    break;
                case 3:
                    if (_client.HasItem(721010) && _client.HasItem(721011) && _client.HasItem(721012) &&
                        _client.HasItem(721013) && _client.HasItem(721014) && _client.HasItem(721015))
                    {
                        //remove the 6 tokens
                        _client.DeleteItem(721010);
                        _client.DeleteItem(721011);
                        _client.DeleteItem(721012);
                        _client.DeleteItem(721013);
                        _client.DeleteItem(721014);
                        _client.DeleteItem(721015);

                        //give player soul jade
                        _client.CreateItem(721072);

                        //teleport player to last map
                        _client.ChangeMap(1050, 211, 156);
                    }
                    break;
            }
            AddFinish();
            Send();
        }
    }
}