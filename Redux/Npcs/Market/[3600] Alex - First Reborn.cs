using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Packets.Game;
using Redux.Enum;

namespace Redux.Npcs
{

    public class NPC_3600 : INpc
    {
        private const uint BASE_GEM_ID = 700003;
        private bool isBlessedRebirth;
        private byte baseGemRequest;
        public NPC_3600(Game_Server.Player _client)
            : base(_client)
        {
            ID = 3600;
            Face = 35;
        }

        public override void Run(Game_Server.Player _client, ushort _linkback)
        {
            Responses = new List<NpcDialogPacket>();
            AddAvatar();
            switch (_linkback)
            {
                case 0:
                    if (_client.RebornCount == 0)
                    {
                        if (_client.ProfessionLevel == 5 && _client.Level >= (_client.ProfessionType == Enum.ProfessionType.WaterTaoist ? 110 : 120))
                        {
                            AddText("Even bold adventurers eventually grow tired of their journeys. I help these heroes by offering them life anew.");
                            AddOption("I wish to reborn.", 1);
                            AddOption("I'm not tired!", 255);
                        }
                        else
                        {
                            AddText("Hello young one! I help people seek the ultimate goal of rebirth. ");
                            AddText("You are not yet experienced enough to undertake this adventure. Please come back later.");
                            AddOption("I will.", 255);
                        }
                    }
                    else
                    {
                        AddText("I have helped you as much as I can. Even heroes have their limits.");
                        AddOption("Thanks", 255);
                    }
                    break;
                case 1:
                    if (_client.RebornCount == 0 && _client.Level >= (_client.ProfessionType == Enum.ProfessionType.WaterTaoist ? 110 : 120))
                    {
                        AddText("By accepting the gift of rebirth, you will start your life again. ");
                        AddText("You can unlock great strength as well as new skills in the process! ");
                        AddText("Are you sure you're ready to give up your current life? ");
                        AddOption("I'm ready.", 2);
                        AddOption("Not yet!", 255);
                    }
                    else
                    {
                        AddText("Ooh my! Lets not get ahead of ourselves. You are not yet ready to become reborn.");
                        AddOption("Sorry.", 255);
                    }
                    break;
                case 2:
                    if (_client.HasItem(Constants.CELESTIAL_STONE_ID))
                    {
                        AddText("You can choose to either receive a gem of great power or bless your equipment for even greater strength. Which would you like?");
                        AddOption("Blessed Rebirth", 3);
                        AddOption("Gem Rebirth", 5);
                        AddOption("I'm not sure...", 255);
                    }
                    else
                    {
                        AddText("Rebirth requires the unlocking of great power. Please make sure you have a CelestialStone before coming to me.");
                        AddOption("Sorry.", 255);
                    }
                    break;
                case 3:
                case 4:
                    isBlessedRebirth = _linkback == 3;
                    AddText("What profession would you like in your new life?");
                    AddOption("Trojan", 11);
                    AddOption("Warrior", 12);
                    AddOption("Water Taoist", 13);
                    AddOption("Fire Taoist", 14);
                    AddOption("Archer", 15);
                    AddOption("I'm not sure...", 255);
                    break;
                case 5:
                    AddText("What type of gem would you like?");
                    AddOption("Phoenix Gem", 20);
                    AddOption("Dragon Gem", 21);
                    AddOption("Fury Gem", 22);
                    AddOption("Rainbow Gem", 23);
                    AddOption("More Options", 6);
                    AddOption("I'm not sure...", 255);
                    break;
                case 6:
                    AddText("What type of gem would you like?");
                    AddOption("Kylin Gem", 24);
                    AddOption("Violet Gem", 25);
                    AddOption("Moon Gem", 26);
                    AddOption("Previous Options", 5);
                    AddOption("I'm not sure...", 255);
                    break;
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                    baseGemRequest = (byte)(_linkback % 10);
                    _linkback = 4;
                    goto case 4;
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                    if (_client.ProfessionLevel ==5 && _client.RebornCount == 0 && _client.Level >= (_client.ProfessionType == Enum.ProfessionType.WaterTaoist ? 110 : 120))
                    {
                        if (_client.HasItem(Constants.CELESTIAL_STONE_ID))
                        {
                            _client.DeleteItem(Constants.CELESTIAL_STONE_ID); 
                            if (isBlessedRebirth)//Bless random item.
                            {
                                Structures.ConquerItem item;
                                for (var tries = 0; tries < 50; tries++)
                                {
                                    var loc = (ItemLocation)(Common.Random.Next(10));
                                    item = _client.Equipment.GetItemBySlot(loc);
                                    if (item != null && item.Bless == 0)
                                    { item.Bless = 1; break; }
                                }
                            }
                            else//Give Super Gem                         
                                _client.CreateItem((uint)(BASE_GEM_ID + baseGemRequest * 10));

                            var path = (uint)_client.ProfessionType % 10 * 10 + (uint)_linkback % 10;
                            if (_client.ProfessionType == ProfessionType.Archer)
                                path = 50 + (uint)_linkback % 10;
                            foreach (var pathData in Database.ServerDatabase.Context.RebornPaths.GetRebornByPath(path))
                                if (pathData.IsForget)
                                    _client.CombatManager.TryRemoveSkill(pathData.SkillId);
                                else
                                    _client.CombatManager.AddOrUpdateSkill(pathData.SkillId, 0);
                            foreach (var skill in _client.CombatManager.skills.Values)
                            {
                                skill.Database.PreviousLevel = skill.Database.Level;
                                skill.Database.Level = 0;
                                skill.Database.Experience = 0;
                                skill.Save();
                                skill.Send(_client);
                            }

                            _client.Character.Profession1 = _client.Character.Profession;
                            switch (_linkback % 10)
                            {
                                case 1:
                                    _client.Character.Profession = 11;
                                    break;
                                case 2:
                                    _client.Character.Profession = 21;
                                    break;
                                case 3:
                                    _client.Character.Profession = 132;
                                    break;
                                case 4:
                                    _client.Character.Profession = 142;
                                    break;
                                case 5:
                                    _client.Character.Profession = 41;
                                    break;
                            }

                            #region Down Level Items
                            for (ItemLocation location = ItemLocation.Helmet; location < ItemLocation.Garment; location++)
                            {
                                Structures.ConquerItem item;
                                item = _client.Equipment.GetItemBySlot(location);
                                if (item != null)
                                { item.DownLevelItem(); _client.Send(ItemInformationPacket.Create(item, ItemInfoAction.Update)); }
                            }
                            #endregion

                            #region Set Bonus Stats
                            _client.Character.Strength = 0;
                            _client.Character.Vitality = 8;
                            _client.Character.Agility = 2;
                            _client.Character.Spirit = 0;
                            switch (_client.Level)
                            {
                                case 110:
                                case 111:
                                    _client.ExtraStats = 20;
                                    break;
                                case 112:
                                case 113:
                                    _client.ExtraStats = 21;
                                    break;
                                case 114:
                                case 115:
                                    _client.ExtraStats = 23;
                                    break;
                                case 116:
                                case 117:
                                    _client.ExtraStats = 26;
                                    break;
                                case 118:
                                case 119:
                                    _client.ExtraStats = 30;
                                    break;
                                case 120:
                                    _client.ExtraStats = (ushort)(_client.ProfessionType == ProfessionType.WaterTaoist ? 35 : 20);
                                    break;
                                case 121:
                                    _client.ExtraStats = (ushort)(_client.ProfessionType == ProfessionType.WaterTaoist ? 35 : 21);
                                    break;
                                case 122:
                                    _client.ExtraStats = (ushort)(_client.ProfessionType == ProfessionType.WaterTaoist ? 41 : 23);
                                    break;
                                case 123:
                                    _client.ExtraStats = (ushort)(_client.ProfessionType == ProfessionType.WaterTaoist ? 41 : 26);
                                    break;
                                case 124:
                                    _client.ExtraStats = (ushort)(_client.ProfessionType == ProfessionType.WaterTaoist ? 48 : 30);
                                    break;
                                case 125:
                                    _client.ExtraStats = (ushort)(_client.ProfessionType == ProfessionType.WaterTaoist ? 48 : 35);
                                    break;
                                case 126:
                                    _client.ExtraStats = (ushort)(_client.ProfessionType == ProfessionType.WaterTaoist ? 56 : 41);
                                    break;
                                case 127:
                                    _client.ExtraStats = (ushort)(_client.ProfessionType == ProfessionType.WaterTaoist ? 56 : 48);
                                    break;
                                case 128:
                                    _client.ExtraStats = (ushort)(_client.ProfessionType == ProfessionType.WaterTaoist ? 65 : 56);
                                    break;
                                case 129:
                                    _client.ExtraStats = 65;
                                    break;
                                case 130:
                                    _client.ExtraStats = 75;
                                    break;
                            }
                            #endregion

                            _client.RebornCount = 1;
                            _client.Send(new UpdatePacket(_client.UID, UpdateType.Profession, _client.Character.Profession));
                            _client.Send(new UpdatePacket(_client.UID, UpdateType.Reborn, _client.RebornCount));
                            _client.SpawnPacket.RebornCount = 1;
                            _client.SendToScreen(_client.SpawnPacket, true);
                            _client.SetLevel(15);
                            _client.Experience = 0;
                            _client.Recalculate(true);
                            Managers.PlayerManager.SendToServer(new TalkPacket(ChatType.GM, _client.Name + " has been reborned successfully. Congratulations!"));
                            _client.Save();
                        }
                        else
                        {
                            AddText("Rebirth requires the unlocking of great power. Please make sure you have a CelestialStone before coming to me.");
                            AddOption("Sorry.", 255);
                        }

                    }
                    else
                    {
                        AddText("Lets not get ahead of ourselves! You are not ready to achieve rebirth. Please come back later.");
                        AddOption("Sorry.", 255);
                    }
                    break;

            }
            AddFinish();
            Send();
        }
    }
}
