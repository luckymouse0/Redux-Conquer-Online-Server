using System;
using System.Collections.Generic;
using Redux.Enum;
using Redux.Game_Server;
using Redux.Packets.Game;
using Redux.Structures;
using Redux.Database;
using Redux.Database.Domain;
using Redux.Database.Repositories;
namespace Redux
{
    public static class Commands
    {
        private delegate void IngameCommandHandler(Player client, string[] command);
        private static readonly Dictionary<string, IngameCommandHandler> _handlers;
        public static void Handle(Player client, string[] command)
        {
            if (_handlers.ContainsKey(command[0]))
                _handlers[command[0]].Invoke(client, command);
            else
                client.Send(new TalkPacket(ChatType.Talk, "Error: No such command [" + command[0] + "]"));
        }
        static Commands()
        {    
            _handlers = new Dictionary<string, IngameCommandHandler>
                {
                {"exit", Process_Exit},
                {"heal", Process_Heal},
                {"item", Process_Item},
                {"level", Process_Level},
                {"money", Process_Money},
                {"cp", Process_CP},
                {"str", Process_Str},
                {"vit", Process_Vit},
                {"agi", Process_Agi},
                {"spi", Process_Spi},
                {"job", Process_Job},
                {"effect", Process_Effect},
                {"skill", Process_Skill},
                {"prof", Process_Prof},
                {"map", Process_Map},
                {"debug", Process_Debug},
                {"transform", Process_Transform},
                {"mapflag", Process_MapFlag},
                {"xp", Process_Xp},
                {"deletenpc", Process_Delete_Npc},
                {"testnpc", Process_Test_Npc},
                {"addnpc", Process_Add_Npc},
                {"npctalk", Process_Test_NpcTalk},
                {"popup", Process_Popup},
                {"update", Process_Update},
                {"test", Process_Test},
                {"downlevel", Process_Down},
                {"dumpskills", Process_DumpSkills},
                {"maptest", Process_MapTest},
                {"data", Process_Data},
                {"broadcast", Process_Broadcast},
                {"report", Process_Report},
                {"goto", Process_GoTo},
                {"summon", Process_Summon},
                {"string", Process_String},
                {"addspawn", Process_AddSpawn},
            };
        }
        private static void Process_Exit(Player client, string[] command)
        {
            client.Disconnect();
        }
        private static void Process_Heal(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            client.Life = client.CombatStats.MaxLife;
            client.Mana = client.CombatStats.MaxMana;
        }
        private static void Process_Item(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            uint uid;
            if(command.Length < 2 || !uint.TryParse(command[1], out uid))
            {            	
            	client.SendMessage("Error: Format should be /item {id}");
            	return;
            }
            var info = Database.ServerDatabase.Context.ItemInformation.GetById(uid);
            if (info == null)
            {
                client.SendMessage("Error: No such item ID as " + uid);
                return;
            }
            var item = new ConquerItem((uint)Common.ItemGenerator.Counter, info);
            byte val;
            if (command.Length > 2)
                if (byte.TryParse(command[2], out val))
                    item.Plus = val;
                else
                    client.SendMessage("Error: Format should be /item {id} {+}");

            if(command.Length > 3)
                if(byte.TryParse(command[3], out val))
                    item.Bless = val;
                else
                    client.SendMessage("Error: Format should be /item {id} {+} {-}");

            if (command.Length > 4)
                if (byte.TryParse(command[4], out val))
                    item.Enchant = val;
                else
                    client.SendMessage("Error: Format should be /item {id} {+} {-} {hp}");

            if (command.Length > 5)
                if (byte.TryParse(command[5], out val))
                    item.Gem1 = val;
                else
                    client.SendMessage("Error: Format should be /item {id} {+} {-} {hp} {gem1}");

            if (command.Length > 6)
                if (byte.TryParse(command[6], out val))
                    item.Gem2 = val;
                else
                    client.SendMessage("Error: Format should be /item {id} {+} {-} {hp} {gem1} {gem2}");
            
            item.SetOwner(client);
            if(client.AddItem(item))
            	client.Send(ItemInformationPacket.Create(item));
            else
            	client.SendMessage("Error adding item");             
        }

        private static void Process_Level(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            byte val;
            if (command.Length > 1 && byte.TryParse(command[1], out val))
            { client.SetLevel(val); client.Experience = 0; }
            else
                client.SendMessage("Error: Format should be /level {#}");
        }
        
        private static void Process_Money(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            uint val;
            if (command.Length > 1 && uint.TryParse(command[1], out val))            
            	client.Money = val;            
            else
                client.SendMessage("Error: Format should be /money {#}");
        }

        private static void Process_CP(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            uint val;
            if (command.Length > 1 && uint.TryParse(command[1], out val))
                client.CP = val;
            else
                client.SendMessage("Error: Format should be /cp {#}");
        }

        private static void Process_Str(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            ushort val;
            if (command.Length > 1 && ushort.TryParse(command[1], out val))
            {
                client.Strength = val;
                client.Recalculate();
            }
            else
                client.SendMessage("Error: Format should be /str {#}");
        }

        private static void Process_Vit(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            ushort val;
            if (command.Length > 1 && ushort.TryParse(command[1], out val))
            {
                client.Vitality = val;
                client.Recalculate();
            }
            else
                client.SendMessage("Error: Format should be /vit {#}");
        }

        private static void Process_Agi(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            ushort val;
            if (command.Length > 1 && ushort.TryParse(command[1], out val))
            {
                client.Agility = val;
                client.Recalculate();
            }
            else
                client.SendMessage("Error: Format should be /agi {#}");
        }

        private static void Process_Spi(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            ushort val;
            if (command.Length > 1 && ushort.TryParse(command[1], out val))
            {
                client.Spirit = val;
                client.Recalculate();
            }
            else
                client.SendMessage("Error: Format should be /spi {#}");
        }
        private static void Process_Job(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            byte val;
            if (command.Length > 1 && byte.TryParse(command[1], out val))
            {
                client.Character.Profession = val;
                client.Send(new UpdatePacket(client.UID, UpdateType.Profession, client.Character.Profession));
                client.Recalculate();
            }
            else
                client.SendMessage("Error: Format should be /job {#}");
        }
        private static void Process_Effect(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            byte val;
            int duration = 2;
            if(command.Length > 2)
                int.TryParse(command[2], out duration);
            if (byte.TryParse(command[1], out val))
            {
                client.AddEffect((ClientEffect)(1ul << val), duration * Common.MS_PER_SECOND);
            }
            else
                client.SendMessage("Error: Format should be /effect {#}");
        }
        private static void Process_Skill(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            ushort id, level;
            if (command.Length > 2 && ushort.TryParse(command[1], out id) && ushort.TryParse(command[2], out level))
                client.CombatManager.AddOrUpdateSkill(id, level);             
            else
                client.SendMessage("Error: Format should be /skill {###} {#}");
        }
        private static void Process_Prof(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            ushort id, level;
            if (command.Length > 2 && ushort.TryParse(command[1], out id) && ushort.TryParse(command[2], out level))
                client.CombatManager.AddOrUpdateProf(id, level);
            else
                client.SendMessage("Error: Format should be /prof id {lvl}");
        }
        private static void Process_Map(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            ushort id, x, y;
            if (command.Length == 2 && ushort.TryParse(command[1], out id))
            {
                var m = Database.ServerDatabase.Context.Maps.GetById(id);
                if (m != null)
                    client.ChangeMap(m.SpawnID, m.SpawnX, m.SpawnY);
                else
                    client.SendMessage("Error:Unhandled map ID " + id);
            }
            else if (command.Length > 3 && ushort.TryParse(command[2], out x) && ushort.TryParse(command[3], out y) && ushort.TryParse(command[1], out id))
                client.ChangeMap(id, x, y);
            else
                client.SendMessage("Error: Format should be /map {id} {x} {y}");
        }
        private static void Process_Debug(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;

            Constants.DEBUG_MODE = !Constants.DEBUG_MODE;
            client.SendMessage("Debug mode is now " + Constants.DEBUG_MODE);
        }
        private static void Process_Transform(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;

            ushort transform;
            if (command.Length > 1 && ushort.TryParse(command[1], out transform))
            {
                var monsterType = Database.ServerDatabase.Context.Monstertype.GetById(transform);
                if (monsterType != null)
                    client.SetDisguise(monsterType, 30 * Common.MS_PER_SECOND);
            }
            else
                client.SendMessage("Error: Format should be /transform {id}");
        }
        private static void Process_MapFlag(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;

            byte shift;
            if (command.Length > 1 && byte.TryParse(command[1], out shift))
            {
                var flag = (MapTypeFlags)(1U << shift);
                var has = client.Map.MapInfo.Type.HasFlag(flag);
                if (has)
                    client.Map.MapInfo.Type &= ~flag;
                else
                    client.Map.MapInfo.Type |= flag;
                Database.ServerDatabase.Context.Maps.Update(client.Map.MapInfo);
                client.SendMessage("Map flag " + flag + " now set to " + !has + " with overal effect pool of " + (uint)client.Map.MapInfo.Type);
            }
            else
                client.SendMessage("Error: Format should be /mapflag {flag}");
        }
        private static void Process_Xp(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;

            byte xp;
            if (command.Length > 1 && byte.TryParse(command[1], out xp))
            {
                client.Xp = xp;
            }
            else
                client.SendMessage("Error: Format should be /xp {#}");
        }
        private static void Process_Drop(Player client, string[] command) //TODO: Why is this unused?
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            if(command.Length < 3)
                return;
            uint uid, id;
            if(uint.TryParse(command[1], out uid) && 
                uint.TryParse(command[2], out id))
            {
                var gi = new GroundItemPacket
                    {Action = GroundItemAction.Create, UID = uid, ID = id, X = client.X, Y = client.Y};
                client.Send(gi);

            }
                
        }
        private static void Process_Delete_Npc(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            uint id;
            if (command.Length > 1 && uint.TryParse(command[1], out id))
            {
                var npc = Database.ServerDatabase.Context.Npcs.GetById(id);
                if (npc != null)
                {
                    Database.ServerDatabase.Context.Npcs.Remove(npc);
                    foreach (var map in Managers.MapManager.ActiveMaps.Values)
                        if (map.Objects.ContainsKey(npc.UID))
                        {
                            Space.ILocatableObject x;
                            map.Objects.TryRemove(npc.UID, out x);
                            if (x != null)
                                foreach (var player in map.QueryPlayers(x))
                                    player.Send(GeneralActionPacket.Create(id, DataAction.RemoveEntity, 0, 0));
                        }
                }
            }
        }
        private static void Process_Test_Npc(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            uint id;
            ushort mesh;
            if (command.Length > 2 && uint.TryParse(command[1], out id) && ushort.TryParse(command[2], out mesh))
            {
                var spawnPacket = SpawnNpcPacket.Create(id, mesh, client.Location);
                uint type;
                if (command.Length > 3 && uint.TryParse(command[3], out type))
                    spawnPacket.Type = (NpcType)type;                
                client.Send(spawnPacket);
            }
            else
                client.SendMessage("Error: Format should be /testnpc id mesh");
        }
        private static void Process_Add_Npc(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            if (command.Length < 2)            
                client.SendMessage("ERROR: Correct format should be /addnpc mesh");
            ushort mesh;
            if (ushort.TryParse(command[1], out mesh))
            {
                var npc = new DbNpc();
                npc.Map = client.MapID;
                npc.X = client.X;
                npc.Y = client.Y;
                byte flag = 2;
                if (command.Length > 2)
                    byte.TryParse(command[2], out flag);
                npc.Type = (NpcType)flag;
                npc.Mesh = mesh;
                ServerDatabase.Context.Npcs.Add(npc);
                client.Map.Insert(new Npc(npc, client.Map));                    

            }
            
            //TODO: FINISH

        }
        private static void Process_Test_NpcTalk(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            try
            {
                var packet = NpcDialogPacket.Create();
                packet.Action = DialogAction.Dialog;
                packet.Strings.AddString("TEST STRING");

            }
            catch (Exception p) { Console.WriteLine(p); }
        }
        private static void Process_Popup(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            try
            {
                var packet = GeneralActionPacket.Create(client.UID, DataAction.OpenWindow, ushort.Parse(command[1]), ushort.Parse(command[2]));
                client.Send(packet);

            }
            catch (Exception p) { Console.WriteLine(p); }
        }
        private static void Process_Update(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            try
            {
                var packet = UpdatePacket.Create(client.UID, (UpdateType)int.Parse(command[3]), uint.Parse(command[1]), uint.Parse(command[2]));
                client.Send(packet);

            }
            catch (Exception p) { Console.WriteLine(p); }
        }
        private static void Process_Test(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            try
            {
                foreach (var skill in client.CombatManager.skills.Keys)
                    client.CombatManager.TryRemoveSkill(skill);
                switch (command[1].ToLower())
                {
                    case "tro":
                        client.CombatManager.AddOrUpdateSkill(SkillID.Cyclone, 0);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Hercules, 4);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Rage, 9);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Phoenix, 9);
                        client.CombatManager.AddOrUpdateSkill(SkillID.ScentSword, 4);
                        client.CombatManager.AddOrUpdateSkill(SkillID.FastBlade, 4);
                        client.CombatManager.AddOrUpdateSkill(SkillID.SpiritHealing, 2);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Robot, 2);
                        break;
                    case "war":
                        client.CombatManager.AddOrUpdateSkill(SkillID.Superman, 0);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Shield, 0);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Accuracy, 0);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Roar, 0);
                        client.CombatManager.AddOrUpdateSkill(SkillID.FlyingMoon, 0);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Dash, 0);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Rage, 9);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Phoenix, 9);
                        client.CombatManager.AddOrUpdateSkill(SkillID.ScentSword, 4);
                        client.CombatManager.AddOrUpdateSkill(SkillID.FastBlade, 4);
                        break;
                    case "wat":
                        client.CombatManager.AddOrUpdateSkill(SkillID.Thunder, 4);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Fire, 4);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Cure, 4);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Meditation, 2);
                        client.CombatManager.AddOrUpdateSkill(SkillID.StarOfAccuracy, 4);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Stigma, 4);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Invisiblity, 4);
                        client.CombatManager.AddOrUpdateSkill(SkillID.MagicShield, 4);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Revive, 0);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Pray, 0);
                        client.CombatManager.AddOrUpdateSkill(SkillID.AdvancedCure, 4);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Nectar, 2);
                        client.CombatManager.AddOrUpdateSkill(SkillID.HealingRain, 3);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Volcano, 0);
                        client.CombatManager.AddOrUpdateSkill(SkillID.SpeedLightning, 0);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Lightning, 0);
                        break;
                    case "fire":
                        client.CombatManager.AddOrUpdateSkill(SkillID.Thunder, 4);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Fire, 4);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Cure, 4);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Tornado, 4);
                        client.CombatManager.AddOrUpdateSkill(SkillID.FireMeteor, 4);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Bomb, 3);
                        client.CombatManager.AddOrUpdateSkill(SkillID.FireCircle, 3);
                        client.CombatManager.AddOrUpdateSkill(SkillID.FireRing, 3);
                        client.CombatManager.AddOrUpdateSkill(SkillID.FireOfHell, 3);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Volcano, 0);
                        client.CombatManager.AddOrUpdateSkill(SkillID.SpeedLightning, 0);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Lightning, 0);
                        break;
                    case "archer":
                        client.CombatManager.AddOrUpdateSkill(SkillID.Fly, 0);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Scatter, 3);
                        client.CombatManager.AddOrUpdateSkill(SkillID.AdvancedFly, 0);
                        client.CombatManager.AddOrUpdateSkill(SkillID.RapidFire, 1);
                        client.CombatManager.AddOrUpdateSkill(SkillID.Intensify, 2);
                        client.CombatManager.AddOrUpdateSkill(SkillID.ArrowRain, 0);
                        break;
                }

            }
            catch (Exception p) { Console.WriteLine(p); }
        }
        private static void Process_Down(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            try
            {
                for (ItemLocation location = ItemLocation.Helmet; location < ItemLocation.Garment; location++)
                {
                    Structures.ConquerItem item;
                    item = client.Equipment.GetItemBySlot(location);
                    if (item != null)
                    { item.DownLevelItem(); client.Send(ItemInformationPacket.Create(item, ItemInfoAction.Update)); }
                }

            }
            catch (Exception p) { Console.WriteLine(p); }
        }
        private static void Process_DumpSkills(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            try
            {
                foreach (var skill in client.CombatManager.skills.Keys)
                    client.CombatManager.TryRemoveSkill(skill);
            }
            catch (Exception p) { Console.WriteLine(p); }
        }
        private static void Process_MapTest(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            try
            {

                uint id;
                if (uint.TryParse(command[1], out id))
                {
                    for (ushort x = 0; x < 1000; x++)
                        for (ushort y = 0; y < 1000; y++)

                            if (Common.MapService.Valid((ushort)id, x, y))
                            {
                                client.ChangeMap((ushort)id, x, y);
                                return;
                            }
                    Console.WriteLine("Unable to find any valid coords for map id {0}", id);

                }
            }
            catch (Exception p) { Console.WriteLine(p); }
        }
        private static void Process_Data(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            try
            {
                var packet = new GeneralActionPacket();
                packet.UID = client.UID;
                packet.Data2Low = client.X;
                packet.Data2High = client.Y;
                packet.Data1 = uint.Parse(command[2]);
                packet.Action = (DataAction)uint.Parse(command[1]);
                client.Send(packet);
            }
            catch (Exception p) { Console.WriteLine(p); }
        }
        private static void Process_Broadcast(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            try
            {
                Managers.PlayerManager.SendToServer(new TalkPacket(ChatType.Broadcast, string.Join(" ", command, 1, command.Length - 1)));
            }
            catch (Exception p) { Console.WriteLine(p); }
        }
        private static void Process_Report(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            try
            {
                var report = new Database.Domain.DbBugReport();
                report.Reporter = client.Name;
                report.Description = string.Join(" ", command, 1, command.Length - 1);
                report.ReportedAt = DateTime.Now;
                Database.ServerDatabase.Context.BugReports.Add(report);
                client.SendMessage("Bug report has been added successfully!");
            }
            catch (Exception p) { Console.WriteLine(p); }
        }
        private static void Process_GoTo(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;

            if (client.Account.Permission < PlayerPermission.GM)
                return;
            if (command.Length < 2)
                client.SendMessage("Error: Proper format is /goto PlayerName");
            else
            {
                Player target = null;
                foreach (var p in Managers.PlayerManager.Players.Values)
                    if (p.Name.ToLower() == command[1].ToLower())
                    {
                        target = p;
                        break;
                    }
                if (target == null)
                    client.SendMessage(command[1] + " is not online or could not be found");
                else
                {
                    target.SendMessage(client.Name + " has teleported to your location!");
                    client.ChangeMap(target.MapID, target.X, target.Y);}
            }
        }
        private static void Process_Summon(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            if (command.Length < 2)
                client.SendMessage("Error: Proper format is /summon PlayerName");
            else
            {
                Player target = null;
                foreach (var p in Managers.PlayerManager.Players.Values)
                    if (p.Name.ToLower() == command[1].ToLower())
                    {
                        target = p;
                        break;
                    }
                if (target == null)
                    client.SendMessage(command[1] + " is not online or could not be found");
                else
                {
                    target.SendMessage(client.Name + " has teleported you to their location!");
                    target.ChangeMap(client.MapID, client.X, client.Y);
                }
            }

        }
        private static void Process_String(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;
            if (command.Length < 2)
                client.SendMessage("Error: Proper format is /string Content");
            else
                client.SendToScreen(StringsPacket.Create(client.UID, StringAction.Fireworks, command[1]), true);


        }
        private static void Process_AddSpawn(Player client, string[] command)
        {
            if (client.Account.Permission < PlayerPermission.GM)
                return;

            if (command.Length < 4)
                client.SendMessage("Error: Proper format is /addspawn id count radius");
            else
            {
                uint uid, id;
                int count;
                    ushort radius;
                if (uint.TryParse(command[1], out id) &&
                    int.TryParse(command[2], out count) &&
                    ushort.TryParse(command[3], out radius))
                {
                    var monsterType = ServerDatabase.Context.Monstertype.GetById(id);
                    if (monsterType == null)
                        client.SendMessage("ERROR: No such monster ID: " + id);
                    else
                    {
                        var dbSpawn = new DbSpawn();
                        dbSpawn.MonsterType = monsterType.ID;
                        dbSpawn.X1 = (ushort)(client.Location.X - radius);
                        dbSpawn.Y1 = (ushort)(client.Location.Y - radius);
                        dbSpawn.X2 = (ushort)(client.Location.X + radius);
                        dbSpawn.Y2 = (ushort)(client.Location.Y + radius);
                        dbSpawn.Map = client.MapID;
                        dbSpawn.AmountMax = count;
                        dbSpawn.AmountPer = count / 2;
                        dbSpawn.Frequency = 10;
                        ServerDatabase.Context.Spawns.Add(dbSpawn);
                        client.Map.Spawns.Add(new Managers.SpawnManager(dbSpawn, client.Map));
                    }
                }

            }
        }      
    }
}
