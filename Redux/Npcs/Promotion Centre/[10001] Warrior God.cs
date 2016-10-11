using System;
using System.Collections.Generic;
using Redux.Packets.Game;
using Redux.Enum;

namespace Redux.Npcs
{

    public class NPC_10001 : INpc
    {
        /// <summary>
        /// Handles NPC usage for [10001] Warrior God
        /// </summary>
        public NPC_10001(Game_Server.Player _client)
            : base(_client)
        {
            ID = 10001;
            Face = 8;
        }

        public override void Run(Game_Server.Player client, ushort linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();

            if (client.ProfessionType != ProfessionType.Warrior)
            {
                AddText("Warriors do not share their secrets of battle with others. I shall not teach you.");
                AddOption("I see.", 255);
                AddFinish();
                Send();
                return;
            }

            switch (linkback)
            {
                #region Initial Options
                case 0:
                    AddText("To be a good Warrior, you must know your weapon, your armor be a 2nd");
                    AddText(" skin, and use your shild to ward off your enemy! Remember, fortune favors");
                    AddText(" the brave, and you must keep your head, even in the darkest hours. So, what");
                    AddText(" can I do for you?");
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
                            AddText("The single requirement of becoming a Warrior is a minimum level of 15.");
                            AddText(" Would you like to proceed?");
                            AddOption("Yes, please.", 2);
                            AddOption("No.", 255);
                            break;
                        case 1:
                            AddText("You may become a Brass Warrior at level 40. Do you want to continue?");
                            AddOption("Yes, I do.", 2);
                            AddOption("No.", 255);
                            break;
                        case 2:
                            AddText("You may promote to Silver Warrior at level 70.");
                            AddText(" I will require one Emerald, I hear they are dropped by");
                            AddText("  monsters in the desert. Are you ready for promotion?");
                            AddOption("I'm ready!", 2);
                            AddOption("I'm not sure...", 255);
                            break;
                        case 3:
                            AddText("You may promote to Gold Warrior at level 100.");
                            AddText(" I will require one Meteor. Are you ready for promotion?");
                            AddOption("I'm ready!", 2);
                            AddOption("I'm not sure...", 255);
                            break;
                        case 4:
                            AddText("You may promote to Warrior Master at level 110.");
                            AddText(" I will require one Moon Box. Are you ready for promotion?");
                            AddOption("I'm ready!", 2);
                            AddOption("I'm not sure...", 255);
                            break;
                        case 5:
                            AddText("You are a true Warrior. I cannot promote you anymore!");
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
                                client.CombatManager.LearnNewSkill(SkillID.Accuracy);
                                client.CombatManager.LearnNewSkill(SkillID.Shield);
                                client.CombatManager.LearnNewSkill(SkillID.Superman);
                                client.CombatManager.LearnNewSkill(SkillID.Roar);
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
                            if (client.Level >= 40 && client.Strength >= 80 && client.Agility >= 25 && client.Vitality >= 20)
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
                            if (client.Level >= 70 && client.HasItem(Constants.EMERALD_ID) && client.Strength >= 140 && client.Agility >= 45 && client.Vitality >= 32)
                            {
                                client.DeleteItem(Constants.EMERALD_ID);
                                AddText("You have been promoted!");
                                client.CreateItem(Constants.METEOR_ID);//meteor
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
                            if (client.Level >= 100 && client.HasItem(Constants.METEOR_ID) && client.Strength >= 205 && client.Agility >= 60 && client.Vitality >= 42)
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
                            if (client.Level >= 110 && client.HasItem(Constants.MOONBOX_ID) && client.Strength >= 225 && client.Agility >= 65 && client.Vitality >= 47)
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
                    AddText("What skill would you like to learn?");
                    AddOption("XP Skills", 4);
                    AddOption("Dash", 5);
                    AddOption("Nothing.", 255);
                    break;
                case 4:
                    if (client.Level >= 15)
                    {
                        AddText("You have learned your XP skills successfully");
                        client.CombatManager.LearnNewSkill(SkillID.Accuracy);
                        client.CombatManager.LearnNewSkill(SkillID.Shield);
                        client.CombatManager.LearnNewSkill(SkillID.Superman);
                        client.CombatManager.LearnNewSkill(SkillID.Roar);
                    }
                    else
                        AddText("You must be at least level 15 to learn these skills!");
                    AddOption("Thanks", 255);
                    break;
                case 5:
                    if (client.Level >= 63)
                    {
                        AddText("You have learned Dash successfully");
                        client.CombatManager.LearnNewSkill(SkillID.Dash);
                    }
                    else
                        AddText("You must be at least level 63 to learn this skill!");
                    AddOption("Thanks", 255);
                    break;
                #endregion
            }
            AddFinish();
            Send();
        }
    }
}