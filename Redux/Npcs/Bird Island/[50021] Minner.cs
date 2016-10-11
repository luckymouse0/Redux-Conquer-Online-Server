using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;

namespace Redux.Npcs
{

    public class NPC_50021 : INpc
    {
        /// <summary>
        /// Handles NPC usage for [50021] Minner
        /// </summary>
        public NPC_50021(Game_Server.Player _client)
            : base(_client)
        {
            ID = 50021;
            Face = 2;
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
                        AddText("I cannot help crying. Where did you get this sad meteor? What happened?");
                        AddOption("It is from Joe.", 6);
                        AddOption("Sorry, I cannot tell you.", 255);
					}
                    else
                    {
                        AddText("Should auld acquaintance be forgotten and never brought to mind? Should auld acquaintance be forgotten and auld lang syne?");
                        AddOption("Why are you so sad?", 1);
                        AddOption("Nice song.", 255);
                    }					
					break;
                case 1:
                    if (_client.HasItem(721000))
                    {
						AddText("Thanks. I am so pleased to hear from my sister. I have not seen her for a long time. It is very thoughtful of her. Sigh!");
						AddOption("Why are you so sad?", 2);
						AddOption("I had better leave now.", 255);
                    }
					else
					{					
                        AddText("I miss you like crazy every time of every day. I love you deeply all my life. Why are you so cruel to break my heart.");
                        AddOption("I got to go.", 255);
					}
                    break;
                case 2:
                    AddText("My lover has left me without any reason. I tried to find him but got no news since he left. Can you help me to find him?");
                    AddOption("OK. I shall help you.", 4);
                    AddOption("Sorry, I am too busy.", 3);
                    break;
                case 3:                    
					//No check needed - We are exiting, not rewarding
					_client.DeleteItem(721000);
					AddText("Thank you for the letter. Good bye.");
					AddOption("See you.", 255);
					break;                    
                case 4:                    
					AddText("Thanks. Joe gave me this bag as our love token. Give it to him when you find him and tell him I am always here waiting for him.");
					AddOption("I shall try my best!", 5);
					break;				
                case 5:                    
					if(_client.HasItem(721000))
					{				
						//No need to check inventory space. We're deleting and adding at the same time
						_client.DeleteItem(721000);			
						_client.CreateItem(721001);
					}
					break;				
                case 6:
                    {
                        AddText("Joe! You have seen him? Where is he now? Is everything going on well with him?");
                        AddOption("Do not cry.", 7);
                        AddOption("Do not ruin the meteor!", 255);
                        break;
                    }
                case 7:                    
					_client.SendMessage("Joe loves and misses you very much. He engraved his love on this meteor. He said you will understand after you see it.");
					AddText("It is said that true love can move the meteor to cry and turn it to meteor tear. I am so glad that Joe does love me.");
					AddOption("All will be fine.", 8);
					AddOption("Return my meteor.", 255);
					break;				
                case 8:                    
					AddText("Joe, Can you hear me? I love you. wherever you go, whatever you do, I will be right here waiting for you.");
					AddOption("Sad love story. Sigh.", 9);
					break;
				case 9:                    
					AddText("Since Joe enjoys travelling round the world, I only wish he is happy. I shall be very delighted Whenever he thinks of me.");
					AddOption("How about you then?", 10);
					break;                    
                case 10:				
					AddText("It is said a star in the sky represents a person on the earth. When I miss him, I can look at his star and it will twinkle.");
					AddOption("I hope Joe will return.", 11);
					break;                    
                case 11:
                    if(_client.HasItem(721002))
					{
                        _client.DeleteItem(721002);
                        _client.CreateItem(1088002);
                        AddText("Thanks for your news. Please take this meteor tear. I hope you are happy all your life.");
                        AddOption("Thanks.", 255);
                    }
					break;
			}
            AddFinish();
            Send();

        }
    }
}