using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{

    public class NPC_50022 : INpc
    {
        /// <summary>
        /// Handles NPC usage for [50022] Joe
        /// </summary>
        public NPC_50022(Game_Server.Player _client)
            : base(_client)
        {
            ID = 50022;
            Face = 10;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    if (_client.HasItem(721002))
                    {
                        AddText("Thanks a lot. Please kindly give this meteor to Minner. I hope she can understand me and wish her happy all her life.");
                        AddOption("I hope so.", 255);
                    }
                    else
                    {
                        AddText("It is so hot. It would be very nice If I have a bottle of wine. Can you give me some wine?");
                        AddOption("I have a bottle of Amrita.", 1);
                        AddOption("I do not have either.", 255);
                    }
                    break;
                case 1:
                    if (_client.HasItem(1000030) && _client.HasItem(1088001) && _client.HasItem(721001))
                    {
                        AddText("Thank you. You are very kind. I often have no food and water when I travel around. Where did you get this guardian star?");
                        AddOption("It is from Minner.", 2);
                        AddOption("I picked it on the road.", 7);
                    }
                    else
                    {
                        AddText("I cannot take your Amrita. There is little water in the vast desert. You may feel thirsty and need it later. And I need a meteor too.");
                        AddOption("See you then.", 255);
                    }
                    break;
                case 2:
                    AddText("So she sent you to look for me. I have left so long. She must be heartbroken.");
                    AddOption("Why did you leave her?", 3);
                    break;
                case 3:
                    AddText("We love each other deeply. I wish I could stay with her, but my life dream is to travel around. I cannot give her a warm home.");
                    AddOption("You are right.", 4);
                    AddOption("I like it very much.", 255);
                    break;
                case 4:
                    AddText("What a nice meteor you have. I have promised Minner that I shall give her a meteor. Would you please give your meteor to me.");
                    AddOption("Here you are.", 5);
                    break;
                case 5:
                    AddText("Thanks. I shall engrave my love on this meteor. Please kindly give it to Minner. She will understand why I leave her.");
                    AddOption("I shall give it to her.", 6);
                    AddOption("Do not touch my meteor.", 255);
                    break;
                case 6:
                    if (_client.HasItem(1000030) && _client.HasItem(1088001) && _client.HasItem(721001))
                    {
                        _client.DeleteItem(721001);
                        _client.DeleteItem(1088001);
                        _client.CreateItem(721002);
                        AddText("Thanks a lot. Please kindly give this meteor to Minner. I hope she can understand me and wish her happy all her life.");
                        AddOption("I hope so.", 255);
                    }
                    else
                    {
                        AddText("You don't have the items I need!");
                        AddOption("Sorry.", 255);
                    }
                    break;
                case 7:
                    AddText("I must have hurt her a lot. She used to carry this bag wherever she goes. I hope she can forget me and be happy all her life.");
                    AddOption("I got to go. Bye.", 255);
                    break;
            }
            AddFinish();
            Send();
        }
    }
}