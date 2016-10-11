using System;
using System.Collections.Generic;
using Redux.Packets.Game;
using Redux.Enum;

namespace Redux.Npcs
{

    public class NPC_400 : INpc
    {
        /// <summary>
        /// Handles NPC usage for [400] Archer God
        /// </summary>
        public NPC_400(Game_Server.Player _client)
            : base(_client)
        {
            ID = 400;
            Face = 10;
        }

        public override void Run(Game_Server.Player client, ushort linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();

            if (client.ProfessionType != ProfessionType.Archer)
            {
                AddText("Archers do not share their secrets of battle with others. I shall not teach you.");
                AddOption("I see.", 255);
                AddFinish();
                Send();
                return;
            }

            switch (linkback)
            {
                #region Initial Options
                case 0:
                    AddText("Mercilessly fight your way to the top, my friend, because Archers are destined ");
                    AddText("for greatness! Your swift mind will give you the ability to attack from afar, at a ");
                    AddText("pace unmatched by any of your peers. So, what can I do for you?");

                    AddOption("I want to get promoted.", 1);
                    AddOption("Learn class skills.", 3);
                    AddOption("Okay. I see.", 255);
                    break;
                #endregion

                #region Promotion
                case 1:
                    switch (client.ProfessionLevel)
                    {
                        case 0:
                            AddText("The single requirement of becoming an Archer is a minimum level of 15.");
                            AddText(" Would you like to proceed?");
                            AddOption("Yes, please.", 2);
                            AddOption("No.", 255);
                            break;
                        case 1:
                            AddText("You may become a Eagle Archer at level 40. Do you want to continue?");
                            AddOption("Yes, I do.", 2);
                            AddOption("No.", 255);
                            break;
                        case 2:
                            AddText("You may promote to Tiger Archer at level 70.");
                            AddText(" I will require one Emerald, I hear they are dropped by");
                            AddText("  monsters in the desert. Are you ready for promotion?");
                            AddOption("I'm ready!", 2);
                            AddOption("I'm not sure...", 255);
                            break;
                        case 3:
                            AddText("You may promote to Dragon Archer at level 100.");
                            AddText(" I will require one Meteor. Are you ready for promotion?");
                            AddOption("I'm ready!", 2);
                            AddOption("I'm not sure...", 255);
                            break;
                        case 4:
                            AddText("You may promote to Archer Master at level 110.");
                            AddText(" I will require one Moon Box. Are you ready for promotion?");
                            AddOption("I'm ready!", 2);
                            AddOption("I'm not sure...", 255);
                            break;
                        case 5:
                            AddText("You are a true Archer. I cannot promote you anymore!");
                            AddOption("I see.", 255);
                            break;
                    }
                    break;
                case 2:
                    switch (client.ProfessionLevel)
                    {
                        case 0://level 15
                            if (client.Level >= 15)
                            {
                                AddText("You have been promoted! I have taught you XP skills as a reward!");
                                client.CombatManager.LearnNewSkill(SkillID.Fly);
                                client.Character.Profession++;
                                client.Send(new UpdatePacket(client.UID, UpdateType.Profession, client.Character.Profession));
                                client.Recalculate();
                                AddOption("Thank you!", 255);
                            }
                            else
                            {
                                AddText("You are not level 15, I will not promote you yet.");
                                AddOption("I'll work harder.", 255);
                            }
                            break;
                        case 1://level 40
                            if (client.Level >= 40 && client.Strength >= 25 && client.Agility >= 90 && client.Vitality >= 12)
                            {
                                AddText("You have been promoted!");
                                if (client.RebornCount == 1)
                                {
                                    AddText("I've given you a special item to help commemorate the occasion!");
                                    client.CreateItem(410077);//poison blade - need to add stat
                                }
                                client.Character.Profession++;
                                client.Send(new UpdatePacket(client.UID, UpdateType.Profession, client.Character.Profession));
                                client.Recalculate();
                                AddOption("Thank you!", 255);
                            }
                            else
                            {
                                AddText("You must be level 40, I will not promote you yet.");
                                AddOption("I'll work harder.", 255);
                            }
                            break;
                        case 2://level 70
                            if (client.Level >= 70 && client.HasItem(Constants.EMERALD_ID) && client.Strength >= 45 && client.Agility >= 150 && client.Vitality >= 2)
                            {
                                client.DeleteItem(Constants.EMERALD_ID);
                                client.CreateItem(Constants.METEOR_ID);
                                AddText("You have been promoted!");
                                client.Character.Profession++;
                                client.Send(new UpdatePacket(client.UID, UpdateType.Profession, client.Character.Profession));
                                client.Recalculate();
                                AddOption("Thank you!", 255);

                            }
                            else
                            {
                                AddText("You must be level 70 and bring me an Emerald in order to be promoted!");
                                AddOption("I'll work harder.", 255);
                            }
                            break;
                        case 3://level 100
                            if (client.Level >= 100 && client.HasItem(Constants.METEOR_ID) && client.Strength >= 60 && client.Agility >= 215 && client.Vitality >= 32)
                            {
                                client.DeleteItem(Constants.METEOR_ID);
                                AddText("You have been promoted!");
                                switch (client.RebornCount)
                                {
                                    case 0:
                                        client.CreateItem(700031);//normal rainbow gem
                                        break;
                                    default:
                                        client.CreateItem(130087, 0, 0, 0, 255);//unique 1 sock armor
                                        break;
                                }
                                client.Character.Profession++;
                                client.Send(new UpdatePacket(client.UID, UpdateType.Profession, client.Character.Profession));
                                client.Recalculate();
                                AddOption("Thank you!", 255);

                            }
                            else
                            {
                                AddText("You must be level 100 and have a Meteor in order to be promoted!");
                                AddOption("I'll work harder.", 255);
                            }
                            break;
                        case 4://level 110
                            if (client.Level >= 110 && client.HasItem(Constants.MOONBOX_ID) && client.Strength >= 68 && client.Agility >= 235 && client.Vitality >= 34)
                            {
                                client.DeleteItem(Constants.MOONBOX_ID);
                                client.CreateItem(Constants.DRAGONBALL_ID);
                                AddText("You have been promoted!");
                                client.Character.Profession++;
                                client.Send(new UpdatePacket(client.UID, UpdateType.Profession, client.Character.Profession));
                                client.Recalculate();
                                AddOption("Thank you!", 255);
                            }
                            else
                            {
                                AddText("You must be level 110 and have a Moon Box in order to be promoted!");
                                AddOption("I'll work harder.", 255);
                            }
                            break;

                    }
                    break;
                #endregion

                #region Learn Skills
                case 3:
                    AddText("The Archer's Skills are numerous, varied and powerful. Which would you like to learn?");
                    AddOption("Fly", 4);
                    AddOption("Scatter", 5);
                    AddOption("Rapid Fire", 6);
                    AddOption("Intensify", 7);
                    AddOption("Other Skills",8);
                    AddOption("Nothing.", 255);
                    break;
                case 8:
                    AddText("The Archer's Skills are numerous, varied and powerful. Which would you like to learn?");
                    AddOption("Advanced Fly", 9);
                    AddOption("Arrow Rain", 10);
                    AddOption("Previous Skills", 3);
                    AddOption("Nothing.", 255);
                    break;
                case 4:
                    if (client.Level >= 15)
                    {
                        AddText("You have learned Fly successfully");
                        client.CombatManager.LearnNewSkill(SkillID.Fly);
                    }
                    else
                        AddText("You must be at least level 15 to learn this skill!");
                    AddOption("Thanks", 255);
                    break;
                case 5:
                    if (client.Level >= 23)
                    {
                        AddText("You have learned Scatter successfully");
                        client.CombatManager.LearnNewSkill(SkillID.Scatter);
                    }
                    else
                        AddText("You must be at least level 23 to learn this skill!");
                    AddOption("Thanks", 255);
                    break;
                case 6:
                    if (client.Level >= 46)
                    {
                        AddText("You have learned Rapid Fire successfully");
                        client.CombatManager.LearnNewSkill(SkillID.RapidFire);
                    }
                    else
                        AddText("You must be at least level 46 to learn this skill!");
                    AddOption("Thanks", 255);
                    break;
                case 7:
                    if (client.Level >= 71)
                    {
                        AddText("You have learned Intensify successfully");
                        client.CombatManager.LearnNewSkill(SkillID.Intensify);
                    }
                    else
                        AddText("You must be at least level 71 to learn this skill!");
                    AddOption("Thanks", 255);
                    break;
                case 9:
                    if (client.Level >= 70)
                    {
                        AddText("You have learned Advanced Fly successfully");
                        client.CombatManager.LearnNewSkill(SkillID.AdvancedFly);
                    }
                    else
                        AddText("You must be at least level 70 to learn this skill!");
                    AddOption("Thanks", 255);
                    break;
                case 10:
                    if (client.Level >= 70)
                    {
                        AddText("You have learned Arrow Rain successfully");
                        client.CombatManager.LearnNewSkill(SkillID.ArrowRain);
                    }
                    else
                        AddText("You must be at least level 70 to learn this skill!");
                    AddOption("Thanks", 255);
                    break;
                #endregion
            }
            AddFinish();
            Send();
        }
    }
}