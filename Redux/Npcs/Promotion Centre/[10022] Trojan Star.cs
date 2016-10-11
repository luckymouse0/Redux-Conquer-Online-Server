using System;
using System.Collections.Generic;
using Redux.Packets.Game;
using Redux.Enum;

namespace Redux.Npcs
{

    public class NPC_10022 : INpc
    {
        /// <summary>
        /// Handles NPC usage for [10022] General Peace
        /// </summary>
        public NPC_10022(Game_Server.Player _client)
            : base(_client)
        {
            ID = 10022;
            Face = 155;
        }

        public override void Run(Game_Server.Player client, ushort linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();

            if (client.ProfessionType != ProfessionType.Trojan)
            {
                AddText("Trojans do not share their secrets of battle with others. I shall not teach you.");
                AddOption("I see.", 255);
                AddFinish();
                Send();
                return;
            }

            switch (linkback)
            {
                #region Initial Options
                case 0:
                    AddText("Trojans can wield dual weapons, and fearlessly take on anyone in melee");
                    AddText(" combat! Whatever the cost, always remember that courage is the foundation");
                    AddText(" of victory! So, what can I do for you?");
                    AddOption("I would like to get promoted.", 1);
                    AddOption("I wish to learn skills.", 3);
                    AddOption("Nothing.", 255);
                    break;
                #endregion

                #region Promotion
                case 1:
                    switch (client.ProfessionLevel)
                    {
                        case 0:
                            AddText("The single requirement of becoming a Trojan is a minimum level of 15.");
                            AddText(" Would you like to proceed?");
                            AddOption("Yes, please.", 2);
                            AddOption("No.", 255);
                            break;
                        case 1:
                            AddText("You may become a Veteran Trojan at level 40. Do you want to continue?");
                            AddOption("Yes, I do.", 2);
                            AddOption("No.", 255);
                            break;
                        case 2:
                            AddText("You may promote to Tiger Trojan at level 70.");
                            AddText(" I will require one Emerald, I hear they are dropped by");
                            AddText("  monsters in the desert. Are you ready for promotion?");
                            AddOption("I'm ready!", 2);
                            AddOption("I'm not sure...", 255);
                            break;
                        case 3:
                            AddText("You may promote to Tiger Trojan at level 100.");
                            AddText(" I will require one Meteor. Are you ready for promotion?");
                            AddOption("I'm ready!", 2);
                            AddOption("I'm not sure...", 255);
                            break;
                        case 4:
                            AddText("You may promote to Tiger Trojan at level 110.");
                            AddText(" I will require one Moon Box. Are you ready for promotion?");
                            AddOption("I'm ready!", 2);
                            AddOption("I'm not sure...", 255);
                            break;
                        case 5:
                            AddText("You are a true Trojan. I cannot promote you anymore!");
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
                                client.CombatManager.LearnNewSkill(SkillID.Cyclone);
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
                            if (client.Level >= 40 && client.Strength >= 60 && client.Agility >= 25 && client.Vitality >= 25)
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
                            if (client.Level >= 70 && client.HasItem(Constants.EMERALD_ID) && client.Strength >= 110 && client.Agility >= 42 && client.Vitality >= 45)
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
                            if (client.Level >= 100 && client.HasItem(Constants.METEOR_ID) && client.Strength >= 110 && client.Agility >= 42 && client.Vitality >= 45)
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
                            if (client.Level >= 110 && client.HasItem(Constants.MOONBOX_ID) && client.Strength >= 170 && client.Agility >= 65 && client.Vitality >= 100)
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
                    AddOption("Hercules", 5);
                    AddOption("Spirit Healing", 6);
                    AddOption("Robot", 7);
                    AddOption("Nothing.", 255);
                    break;
                case 4:
                    if (client.Level >= 15)
                    {
                        AddText("You have learned Cyclone and Accuracy successfully");
                        client.CombatManager.LearnNewSkill(SkillID.Cyclone);
                        client.CombatManager.LearnNewSkill(SkillID.Accuracy);
                    }
                    else
                        AddText("You must be at least level 15 to learn these skills!");
                    AddOption("Thanks", 255);
                    break;
                case 5:
                    if (client.Level >= 40)
                    {
                        AddText("You have learned Hercules successfully");
                        client.CombatManager.LearnNewSkill(SkillID.Hercules);
                    }
                    else
                        AddText("You must be at least level 40 to learn this skill!");
                    AddOption("Thanks", 255);
                    break;
                case 6:
                    if (client.Level >= 40)
                    {
                        AddText("You have learned Spirit Healing successfully");
                        client.CombatManager.LearnNewSkill(SkillID.SpiritHealing);
                    }
                    else
                        AddText("You must be at least level 40 to learn this skill!");
                    AddOption("Thanks", 255);
                    break;
                case 7:
                    if (client.Level >= 40)
                    {
                        AddText("You have learned Robot successfully");
                        client.CombatManager.LearnNewSkill(SkillID.Robot);
                    }
                    else
                        AddText("You must be at least level 40 to learn this skill!");
                    AddOption("Thanks", 255);
                    break;
                #endregion
            }
            AddFinish();
            Send();
        }
    }
}