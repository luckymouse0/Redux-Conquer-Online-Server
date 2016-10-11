using System;
using System.Collections.Generic;
using Redux.Packets.Game;
using Redux.Enum;

namespace Redux.Npcs
{

    public class NPC_10000 : INpc
    {
        private bool isFire;
        /// <summary>
        /// Handles NPC usage for [10000] Taoist Star
        /// </summary>
        public NPC_10000(Game_Server.Player _client)
            : base(_client)
        {
            ID = 400;
            Face = 6;
        }
        public override void Run(Game_Server.Player client, ushort linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();

            if (client.ProfessionType != ProfessionType.Taoist && client.ProfessionType != ProfessionType.FireTaoist && client.ProfessionType != ProfessionType.WaterTaoist)
            {
                AddText("The taoists` secrets are not to be trained to outsiders. Train elsewhere, master conqueror.");
                AddOption("I see.", 255);
                AddFinish();
                Send();
                return;
            }

            switch (linkback)
            {
                #region Initial Options
                case 0: 
                    AddText("Most of the Taoists are little concerned with anything outside the pursuit of ");
                    AddText("advanced spiritual powers. You are gifted in harnessing your inner power, ");
                    AddText("but remember, the roots of success lie in thoroughness and attention to detail.");
                    AddOption("I want to get promoted.", 1);
                    AddOption("Learn basic skills.", 5);//Handles basic taoist skills
                    if (client.ProfessionLevel >= 2)//Handles class specific skills
                        AddOption("Learn advanced skills.", 6);
                    AddOption("Okay. I see.", 255);
                    break;
                #endregion

                #region Promotion
                case 1:
                    switch (client.ProfessionLevel)
                    {
                        case 0:
                            AddText("The single requirement of becoming an Taoist is a minimum level of 15.");
                            AddText(" Would you like to proceed?");
                            AddOption("Yes, please.", 2);
                            AddOption("No.", 255);
                            break;
                        case 1:
                            AddText("A decision stands before you! Become an all powerful fire taoist or a merciful water taoist?");
                            AddOption("Water Taoist", 3);
                            AddOption("Fire Taoist", 4);
                            AddOption("No.", 255);
                            break;
                        case 2:
                            if (client.ProfessionType == ProfessionType.FireTaoist)
                                AddText("You may promote to become a Fire Wizard at level 70.");
                            else
                                AddText("You may promote to become a Water Wizard at level 70.");
                            AddText(" I will require one Emerald, I hear they are dropped by");
                            AddText("  monsters in the desert. Are you ready for promotion?");
                            AddOption("I'm ready!", 2);
                            AddOption("I'm not sure...", 255);
                            break;
                        case 3: 
                            if (client.ProfessionType == ProfessionType.FireTaoist)
                                AddText("You may promote to become a Fire Master at level 100..");
                            else
                                AddText("You may promote to become a Water Master at level 100.");
                            AddText(" I will require one Meteor. Are you ready for promotion?");
                            AddOption("I'm ready!", 2);
                            AddOption("I'm not sure...", 255);
                            break;
                        case 4:
                            if (client.ProfessionType == ProfessionType.FireTaoist)
                                AddText("You may promote to become a Fire Saint at level 100..");
                            else
                                AddText("You may promote to become a Water Saint at level 100.");
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
                                AddText("You have been promoted! I have taught you an XP skill as a reward!");
                                client.CombatManager.AddOrUpdateSkill(SkillID.Thunder, 0);
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
                            if (client.Level >= 40 && client.Agility >= 25 && client.Vitality >= 22 && client.Spirit >= 80)
                            {
                                AddText("You have been promoted!");
                                if (client.RebornCount == 1)
                                {
                                    AddText("I've given you a special item to help commemorate the occasion!");
                                    client.CreateItem(410077);//poison blade - need to add stat
                                }
                                if (isFire)
                                    client.Character.Profession = 142;
                                else
                                    client.Character.Profession = 132;
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
                            if (client.Level >= 70 && client.HasItem(Constants.EMERALD_ID) && client.Agility >= 45 && client.Vitality >= 32 && client.Spirit >= 140)
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
                            if (client.Level >= 100 && client.HasItem(Constants.METEOR_ID) && client.Agility >= 60 && client.Vitality >= 42 && client.Spirit >= 205)
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
                            if (client.Level >= 110 && client.HasItem(Constants.MOONBOX_ID) && client.Agility >= 65 && client.Vitality >= 47 && client.Spirit >= 225)
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
                case 3:
                case 4:
                    isFire = linkback == 4;
                    goto case 2;
                #endregion

                #region Learn Skills
                case 5:
                    AddText("What skill would you like to learn?");
                    AddOption("Basic Spells", 10);
                    AddOption("Xp Skills", 11);
                    AddOption("Fire", 12);
                    AddOption("Meditation", 13);
                    AddOption("Nothing.", 255);
                    break;
                case 6:
                    if (client.ProfessionType == ProfessionType.FireTaoist)
                    {
                        AddText("What skill would you like to learn?");
                        AddOption("Fire Ring", 14);
                        AddOption("Fire Meteor", 15);
                        AddOption("Fire Circle", 16);
                        AddOption("Tornado", 17);
                    }
                    else if (client.ProfessionType == ProfessionType.WaterTaoist)
                    {
                        AddText("What skill would you like to learn?");
                        AddOption("Healing Rain", 18);
                        AddOption("Star of Accuracy", 19);
                        AddOption("Magic Shield", 20);
                        AddOption("Stigma", 21);
                        AddOption("More skills", 30);
                    }
                    else                    
                        AddText("What! You don't belong here. I can only teach these skills to advanced students.");
                    AddOption("Nevermind", 255);
                    break;
                case 30:
                    if (client.ProfessionType == ProfessionType.WaterTaoist)
                    {
                        AddOption("Invisibility", 22);
                        AddOption("Pray", 23);
                        AddOption("Advanced Cure", 24);
                        AddOption("Nectar", 25);
                        AddOption("Previous skills", 6);
                    }
                    else
                        AddText("What! You don't belong here. I can only teach these skills to advanced students.");
                    AddOption("Nevermind", 255);
                    break;

                #region Basic Skills
                case 10:
                    client.CombatManager.LearnNewSkill(SkillID.Cure);
                    client.CombatManager.LearnNewSkill(SkillID.Thunder);
                    AddText("Learned all basic spells.");
                    AddOption("Thanks", 255);
                    break;
                case 11:
                    if (client.Level >= 15)
                    {
                        client.CombatManager.LearnNewSkill(SkillID.Lightning);
                        if (client.Level >= 40)
                        {
                            client.CombatManager.LearnNewSkill(SkillID.Volcano);
                            if (client.ProfessionType == ProfessionType.WaterTaoist)
                                client.CombatManager.LearnNewSkill(SkillID.Revive);
                            if (client.Level >= 70)
                                client.CombatManager.LearnNewSkill(SkillID.SpeedLightning);
                        }
                        
                        AddText("Learned all available XP skills");
                    }
                    else
                        AddText("You must be at least level 15 to learn XP skills");
                    AddOption("Thanks", 255);
                    break;
                case 12:
                    if (client.Level >= 40 && client.CombatManager.KnowsSkillLevel(SkillID.Thunder, 4))
                    {
                        client.CombatManager.LearnNewSkill(SkillID.Fire);
                        AddText("You have learned Fire successfully");
                    }
                    else
                        AddText("You must be at least level 40 with level 4 Thunder to learn this skill");
                    AddOption("Thanks", 255);
                    break;    
                case 13:
                    if (client.Level >= 44)
                    {
                        client.CombatManager.LearnNewSkill(SkillID.Meditation);
                        AddText("You have learned Meditation successfully");
                    }
                    else
                        AddText("You must be at least level 44 to learn this skill");
                    AddOption("Thanks", 255);
                    break;
                #endregion 

                #region Fire Skills
                case 14:
                    if (client.Level >= 55 && client.ProfessionType == ProfessionType.FireTaoist)
                    {
                        client.CombatManager.LearnNewSkill(SkillID.FireRing);
                        AddText("You have learned Fire Ring successfully");
                    }
                    else
                        AddText("You must be at least level 55 to learn this skill");
                    AddOption("Thanks", 255);
                    break;
                case 15:
                    if (client.Level >= 52 && client.ProfessionType == ProfessionType.FireTaoist)
                    {
                        client.CombatManager.LearnNewSkill(SkillID.FireMeteor);
                        AddText("You have learned Fire Meteor successfully");
                    }
                    else
                        AddText("You must be at least level 52 to learn this skill");
                    AddOption("Thanks", 255);
                    break;
                case 16:
                    if (client.Level >= 65 && client.ProfessionType == ProfessionType.FireTaoist)
                    {
                        client.CombatManager.LearnNewSkill(SkillID.FireCircle);
                        AddText("You have learned Fire Circle successfully");
                    }
                    else
                        AddText("You must be at least level 65 to learn this skill");
                    AddOption("Thanks", 255);
                    break;
                case 17:
                    if (client.Level >= 90 && client.ProfessionType == ProfessionType.FireTaoist && client.CombatManager.KnowsSkillLevel(SkillID.Fire, 3))
                    {
                        client.CombatManager.LearnNewSkill(SkillID.Tornado);
                        AddText("You have learned Tornado successfully");
                    }
                    else
                        AddText("You must be at least level 90 with level 3 Fire to learn this skill");
                    AddOption("Thanks", 255);
                    break;
                #endregion

                #region Water Skills
                case 18:
                    if (client.Level >= 40 && client.ProfessionType == ProfessionType.WaterTaoist)
                    {
                        client.CombatManager.LearnNewSkill(SkillID.HealingRain);
                        AddText("You have learned Healing Rain successfully");
                    }
                    else
                        AddText("You must be at least level 40 to learn this skill");
                    AddOption("Thanks", 255);
                    break;
                case 19:
                    if (client.Level >= 45 && client.ProfessionType == ProfessionType.WaterTaoist)
                    {
                        client.CombatManager.LearnNewSkill(SkillID.StarOfAccuracy);
                        AddText("You have learned Star of Accuracy successfully");
                    }
                    else
                        AddText("You must be at least level 45 to learn this skill");
                    AddOption("Thanks", 255);
                    break;
                case 20:
                    if (client.Level >= 50 && client.ProfessionType == ProfessionType.WaterTaoist)
                    {
                        client.CombatManager.LearnNewSkill(SkillID.MagicShield);
                        AddText("You have learned Magic Shield successfully");
                    }
                    else
                        AddText("You must be at least level 50 to learn this skill");
                    AddOption("Thanks", 255);
                    break;
                case 21:
                    if (client.Level >= 55 && client.ProfessionType == ProfessionType.WaterTaoist)
                    {
                        client.CombatManager.LearnNewSkill(SkillID.Stigma);
                        AddText("You have learned Stigma successfully");
                    }
                    else
                        AddText("You must be at least level 55 to learn this skill");
                    AddOption("Thanks", 255);
                    break;
                case 22:
                    if (client.Level >= 60 && client.ProfessionType == ProfessionType.WaterTaoist)
                    {
                        client.CombatManager.LearnNewSkill(SkillID.Invisiblity);
                        AddText("You have learned Invisibility successfully");
                    }
                    else
                        AddText("You must be at least level 60 to learn this skill");
                    AddOption("Thanks", 255);
                    break;
                case 23:
                    if (client.Level >= 70 && client.ProfessionType == ProfessionType.WaterTaoist)
                    {
                        client.CombatManager.LearnNewSkill(SkillID.Pray);
                        AddText("You have learned Pray successfully");
                    }
                    else
                        AddText("You must be at least level 70 to learn this skill");
                    AddOption("Thanks", 255);
                    break;
                case 24:
                    if (client.Level >= 81 && client.ProfessionType == ProfessionType.WaterTaoist)
                    {
                        client.CombatManager.LearnNewSkill(SkillID.AdvancedCure);
                        AddText("You have learned Advanced Cure successfully");
                    }
                    else
                        AddText("You must be at least level 81 to learn this skill");
                    AddOption("Thanks", 255);
                    break;
                case 25:
                    if (client.Level >= 94 && client.ProfessionType == ProfessionType.WaterTaoist)
                    {
                        client.CombatManager.LearnNewSkill(SkillID.Nectar);
                        AddText("You have learned Nectar successfully");
                    }
                    else
                        AddText("You must be at least level 94 to learn this skill");
                    AddOption("Thanks", 255);
                    break;
                #endregion
                #endregion
            }
            AddFinish();
            Send();
        }
    }
}