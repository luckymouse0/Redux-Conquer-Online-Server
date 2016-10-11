using System;
using Redux.Network;
using Redux.Packets.Game;
using Redux.Enum;
using Redux.Managers;
using Redux.Database.Domain;
using System.Collections.Generic;
using Redux.Space;
using System.IO;
using Redux.Threading;

namespace Redux.Game_Server
{
    #region Game Server
    public unsafe sealed class GameServer
    {
        public ThreadBase PacketThread, WorldThread, CombatThread, MonsterThread;
        #region Constructor
        public GameServer(string name, int port)
        {
            //Begin ServerListener
            var server = new NetworkServer(name)
            {
                ClientBufferSize = 0x1000,
                OnConnect = OnConnect,
                OnReceive = OnReceive,
                OnDisconnect = OnDisconnect

            };
            server.Prepare(port, 100);
            server.BeginAccept();

            //Begin Packet Thread
            PacketThread =  new Threading.PacketThread();
            PacketThread.CreateThread();

            //Begin World Thread
            WorldThread = new Threading.WorldThread();
            WorldThread.CreateThread();

            //Begin Combat Thread
            CombatThread = new Threading.CombatThread();
            CombatThread.CreateThread();

            //Start monster thread
            MonsterThread = new Threading.MonsterThread();
            MonsterThread.CreateThread();

            GuildManager.Initialize();

            NobilityManager.UpdateNobility();
        }
        #endregion

        #region Client Connect
        public static void OnConnect(NetworkClient client)
        {
            var player = new Player(client);
            player.StartExchange();
            client.Owner = player;
        }
        #endregion

        #region Receive Data
        private unsafe static void OnReceive(NetworkClient client, byte[] buffer)
        {
            try
            {
                if (client.Owner == null || !(client.Owner is Player)) return;
                var user = client.Owner as Player;
                user.Crypto.Decrypt(buffer);
                if (!user.UseThreading)
                {
                    user.CompleteExchange(buffer);
                    return;
                }
                #region Split
                fixed (byte* ptr = buffer)
                {
                    var offset = 0;
                    while (offset < buffer.Length)
                    {
                        var size = *((ushort*)(ptr + offset)) + 8;
                        var type = *((ushort*)(ptr + offset + 2));

                        if (size < buffer.Length)
                        {
                            var sub = new byte[size];
                            fixed (byte* pSub = sub)
                            {
                                MSVCRT.memcpy(pSub, ptr + offset, size);
                                ProcessPacket(user, pSub, sub, type);
                            }
                        }
                        else if (size > buffer.Length)
                        {
                            user.Disconnect();
                            break;
                        }
                        else // size == buffer.Length
                        {
                            ProcessPacket(user, ptr, buffer, type);
                        }
                        offset += size;
                    }
                }
            }
            catch (Exception e)
            {
                using (var writer = new StreamWriter(File.Open("CrashLog.txt", FileMode.Append)))
                {
                    if (client.Owner == null || !(client.Owner is Player)) return;
                    {
                        var user = client.Owner as Player;
                        writer.WriteLine("ERROR PROCESSING PACKET FOR USER: " + user.Name);
                    }
                    writer.WriteLine(e);
                    writer.Close();
                }
            }
            #endregion
        }
        #endregion

        #region Socket Error
        private static void OnDisconnect(NetworkClient client)
        {
            if (client == null || client.Owner == null || !(client.Owner is Player))
                return;
            var user = client.Owner as Player;
            user.Disconnect(true);
        }
        #endregion

        #region Process Packet
        public static void ProcessPacket(Player client, byte* ptr, byte[] safePacket, ushort type)
        {

            if (Constants.DEBUG_MODE)
                Console.WriteLine("Receiving {0} from {1}", type, client.Name);
            switch (type)
            {
                case Constants.MSG_REGISTER: Process_MsgRegisterPacket(client, ptr); break;
                case Constants.MSG_CONNECT: Process_MsgConnectPacket(client, ptr); break;
                case Constants.MSG_ACTION: Process_GeneralActionPacket(client, ptr); break;
                case Constants.MSG_STRINGS: Process_StringsPacket(client, ptr); break;
                case Constants.MSG_ITEM_ACTION: Process_ItemActionPacket(client, ptr); break;
                case Constants.MSG_TALK: Process_TalkPacket(client, ptr); break;
                case Constants.MSG_WALK: Process_WalkPacket(client, ptr); break;
                case Constants.MSG_INTERACT: if (client.CombatManager != null) client.CombatManager.ProcessInteractionPacket(ptr); break;
                case Constants.MSG_TEAM_INTERACT: Process_TeamInteractPacket(client, ptr); break;
                case Constants.MSG_SOCKET_GEM: Process_SocketGemPacket(client, ptr); break;
                case Constants.MSG_NPC_INITIAL: Process_NpcPacket(client, ptr); break;
                case Constants.MSG_ASSIGN_ATTRIBUTES: Process_AssignAttributesPacket(client, ptr); break;
                case Constants.MSG_NPC_DIALOG: Process_NpcDialogPacket(client, ptr); break;
                case Constants.MSG_WAREHOUSE_ACTION: client.WarehouseManager.Process_WarehouseActionPacket(ptr); break;
                case Constants.MSG_GROUND_ITEM: Process_GroundItemPacket(client, ptr); break;
                case Constants.MSG_ASSOCIATE: Process_AssociatePacket(client, ptr); break;
                case Constants.MSG_COMPOSE: Process_ComposePacket(client, ptr); break;
                case Constants.MSG_GUILD_REQUEST: Process_GuildPacket(client, ptr); break;
                case Constants.MSG_GUILDMEMBERINFO: Process_GuildMemberInfo(client, ptr); break;
                case Constants.MSG_NOBILITY: Process_Nobility(client, ptr); break;
                case Constants.MSG_TRADE: Process_Trade(client, ptr); break;
                case Constants.MSG_OFFLINETG: Process_OfflineTG(client, ptr); break;
                case Constants.MSG_BROADCAST: Process_Broadcast(client, ptr); break;

                default: Console.WriteLine("Unknown packet type {0} from {1} ", type, client.Name == null ? "NO NAME" : client.Name); break;
            }
        }
        #endregion

        #region Handlers
        #region RegisterPacket
        public static void Process_MsgRegisterPacket(Player client, RegisterPacket packet)
        {
            if (!Common.ValidChars.IsMatch(packet.CharacterName) || packet.CharacterName.Length < 3 || packet.CharacterName.Length >= 16 || packet.CharacterName.ToLower().Contains("admin"))
            {
                client.DirectSend(new TalkPacket(ChatType.Register, "Invalid character name"));
                return;
            }
            if (Database.ServerDatabase.Context.Characters.GetByName(packet.CharacterName) != null)
            {
                client.DirectSend(new TalkPacket(ChatType.Register, "Character name already in use"));
                return;
            }
            if (!Common.ValidCharacterMeshes.Contains(packet.Mesh))
            {
                client.DirectSend(new TalkPacket(ChatType.Register, "Invalid character mesh " + packet.Mesh));
                return;
            }
            if (!Common.ValidBaseProfessions.Contains(packet.Profession))
            {
                client.DirectSend(new TalkPacket(ChatType.Register, "Invalid character profession " + packet.Profession));
                return;
            }
            client.CreateDbCharacter(packet.CharacterName, packet.Mesh, packet.Profession);
            client.DirectSend(new TalkPacket(ChatType.Register, Constants.REPLY_OK_STR));


        }
        #endregion

        #region TalkPacket
        public static void Process_TalkPacket(Player client, TalkPacket packet)
        {
            if (packet.Words[0] == Constants.COMMAND_PREFIX)
            {
                Commands.Handle(client, packet.Words.Substring(1).ToLower().Split(' '));
                return;
            }
            switch (packet.Type)
            {
                #region Guild
                case ChatType.Syndicate:
                    if (client.Guild != null)
                        foreach (var p in PlayerManager.Players.Values)
                            if (p.Guild == client.Guild)
                                p.Send(packet);
                    break;
                #endregion

                #region Team
                case ChatType.Team:
                    if (client.Team != null)
                        client.Team.SendToTeam(packet);
                    break;
                #endregion 

                #region Talk
                case ChatType.Talk:
                    client.SendToScreen(packet);
                    break;
                #endregion 

                #region Whisper
                case ChatType.Whisper:
                    {
                        var target = Managers.PlayerManager.GetUser(packet.Hearer);
                        if (target != null)
                        {
                            Database.ServerDatabase.Context.ChatLogs.Add(new DbChatLog(packet));
                            packet.SpeakerLookface = client.Lookface;
                            packet.HearerLookface = target.Lookface;
                            target.Send(packet);
                        }
                        else
                        {
                            packet.Words = packet.Hearer + " is not online";
                            client.Send(packet);
                        }
                        break;
                    }
                #endregion 

                #region Friend
                case ChatType.Friend:
                    foreach (var friend in client.AssociateManager.Friends.Values)
                        friend.Send(packet);
                    break;
                #endregion 

                #region Guild Announcement
                case ChatType.SynAnnounce:
                    if (client.GuildId == 0) return;

                    var guild = GuildManager.GetGuild(client.GuildId);
                    if (guild == null) return;

                    if (client.GuildRank != GuildRank.GuildLeader)
                    {
                        client.SendSysMessage("You have not been authorized!");
                        return;
                    }

                    string announce;
                    if (packet.Words.Length == 0)
                        return;
                    else
                        announce = packet.Words;

                    guild.SetAnnounce(announce);
                    client.Send(packet);
                    break;
                #endregion

                #region Store Announce
                case ChatType.CryOut:
                    if (client.Shop != null)
                    {
                        client.Shop.HawkMsg = packet;
                        client.SendToScreen(packet);
                    }
                    break;
                #endregion 

            }
        }
        #endregion

        #region ConnectPacket
        public static void Process_MsgConnectPacket(Player client, ConnectPacket packet)
        {
            client.Account = Database.ServerDatabase.Context.Accounts.GetByToken(packet.AccountId);
            client.UID = client.Account.UID;
            if (client.Account.Timestamp + 10 < Common.SecondsServerOnline)
            {
                Console.WriteLine("User {0} is no longer valid", client.Account.Username);
                client.Disconnect(false);
                return;
            }
            if (PlayerManager.Players.ContainsKey(client.Account.UID))
            {
                Player removedPlayer;
                if (PlayerManager.Players.TryRemove(client.Account.UID, out removedPlayer))
                {
                    Console.WriteLine("User {0} is already logged in. Disconnecting old copy of user.", client.Account.Username);
                    removedPlayer.Disconnect();
                }
            }
            var character = Database.ServerDatabase.Context.Characters.GetByUID(client.Account.UID);
            if (character == null)
                client.DirectSend(new TalkPacket(ChatType.Entrance, Constants.NEW_ROLE_STR));
            else
            {
                client.DirectSend(new TalkPacket(ChatType.Entrance, Constants.REPLY_OK_STR));
                client.Populate(character);
            }
        }
        #endregion

        #region GeneralActionPacket
        public static void Process_GeneralActionPacket(Player client, GeneralActionPacket packet)
        {
            switch (packet.Action)
            {
                #region ChangeDirection
                case DataAction.ChangeDirection:
                    client.Direction = (byte)(packet.Data3 % 8);
                    client.SendToScreen(packet);
                    break;
                #endregion
                #region ChangePKMode
                case DataAction.ChangePKMode:
                    PKMode mode = (PKMode)packet.Data1;
                    if (System.Enum.IsDefined(typeof(PKMode), mode))
                    {
                        client.PkMode = (PKMode)packet.Data1;
                        client.Send(packet);
                    }
                    else Console.WriteLine("Unknown PK Mode from {0}: {1}", client.Name, mode);
                    break;
                #endregion
                #region RemoveXP
                case DataAction.RemoveXp:
                    client.RemoveEffect(ClientEffect.XpStart);
                    break;
                #endregion
                #region ChangeFace
                case DataAction.ChangeFace:
                    if (client.Money > 500)
                    {
                        client.Face = packet.Data1Low;
                        client.Money -= 500;
                    }
                    break;
                #endregion
                #region ChangeAction
                case DataAction.ChangeAction:
                    var action = (ActionType)packet.Data1;
                    if (client.Action != action)
                    {
                        if (System.Enum.IsDefined(typeof(ActionType), action))
                        {
                            if (!client.CombatManager.IsInIntone)
                                client.CombatManager.AbortAttack();

                            client.Action = action;

                            if (action == ActionType.Sit || action == ActionType.Lie)
                                client.LastSitAt = Common.Clock;

                            if (client.Mining)
                                client.StopMining();

                            if (Common.Clock > client.LastCool + 3000 && action == ActionType.Cool)
                            {
                                client.LastCool = Common.Clock;
                                if (client.Equipment.IsFullSuper())
                                    packet.Data1 |= (uint)client.Character.Profession * 0x00010000 + 0x01000000;
                                else if (client.Equipment.IsSuperArmor())
                                    packet.Data1 |= (uint)client.Character.Profession * 0x010000;
                            }
                            client.SendToScreen(packet, true);

                        }
                        else Console.WriteLine("Unknown Action from {0}: {1}", client.Name, action);
                    }
                    break;
                #endregion
                #region Hotkeys
                case DataAction.Hotkeys:
                    client.Send(ServerTimePacket.Create());
                    client.Send(Nobility.UpdateIcon(client));
                    client.SpawnPacket.Nobility = (byte)client.NobilityMedal;
                    PlayerManager.SendToServer(new TalkPacket(ChatType.Talk2, client.Name + " Has logged onto the server!"));
                    client.Send(packet);
                    break;
                #endregion
                #region ConfirmFriends
                case DataAction.ConfirmFriends:
                    client.AssociateManager = new AssociateManager(client);
                    client.Send(packet);
                    client.Send(UpdatePacket.Create(client.UID, UpdateType.Merchant, 255));
                    break;
                #endregion
                #region ConfirmProficiencies
                case DataAction.ConfirmProficiencies:
                    client.Send(packet);
                    var update = new UpdatePacket(client.UID);
                    break;
                #endregion
                #region LoadSkills
                case DataAction.ConfirmSkills:
                    client.CombatManager = new CombatManager(client);
                    client.Send(packet);
                    break;
                #endregion
                #region Set Location
                case DataAction.SetLocation:
                    {
                        packet.Data1 = (ushort)client.MapID;
                        packet.Data2Low = (ushort)client.X;
                        packet.Data2High = (ushort)client.Y;
                        client.Send(packet);
                        client.Send(MapStatusPacket.Create(client.Map.MapInfo));
                        break;
                    }
                #endregion
                #region GetSurroundings
                case DataAction.GetSurroundings:
                    if (client.Shop != null && client.Shop.Vending)
                        client.Shop.StopVending();
                    client.UpdateSurroundings(true); break;
                #endregion
                #region Jump
                case DataAction.Jump: client.HandleJump(packet); break;
                #endregion
                #region Use Portal
                case DataAction.UsePortal:
                    try
                    {
                        var portalID = packet.Data3;
                        var mapID = (ushort)client.Map.ID;

                        bool handled = false;
                        foreach (var portal in Common.MapService.MapData[mapID].Portals)
                        {
                            if (Space.Calculations.GetDistance(client.X, client.Y, portal.X, portal.Y) < 3)
                            {
                                var endPortal = Database.ServerDatabase.Context.Passages.GetPortalByMapAndID(client.MapID, portal.ID);
                                client.ChangeMap(endPortal.MapID, endPortal.PortalX, endPortal.PortalY);
                                handled = true;
                                break;
                            }
                        }
                        if (!handled)
                            client.ChangeMap(1002, 400, 400);
                    }
                    catch { Console.WriteLine("Error changing map for player {0}, setting their location to TC", client.Name); client.ChangeMap(1002, 400, 400); }
                    break;
                #endregion
                #region Revive
                case DataAction.Revive:
                    if (packet.Data1 == 1 && client.Character.HeavenBlessExpires > DateTime.Now)
                    {
                        if (!client.HasStatus(Enum.ClientStatus.ReviveTimeout))
                            client.Revive(false);
                    }
                    else
                        if (!client.HasStatus(Enum.ClientStatus.ReviveTimeout))
                            client.Revive();
                    
                    break;
                #endregion
                #region Set Ghost Mesh
                case DataAction.SetGhost:
                    client.Transformation = (ushort)((client.Lookface % 10 > 2) ? 98 : 99);
                    break;
                #endregion
                #region Cancel Disguise
                case DataAction.CancelDisguise:
                    client.RemoveStatus(ClientStatus.TransformationTimeout);
                    client.Transformation = 0;
                    client.Recalculate(true);
                    client.Send(UpdatePacket.Create(client.UID, UpdateType.MaxLife, client.MaximumLife));
                    break;
                #endregion
                #region Observe Equipment
                case DataAction.ObserveFriend:
                case DataAction.ObserveEquipment:
                    {
                        //packet.Action = DataAction.ObserveEquipment;
                        var target = client.Map.Search<Player>(packet.Data1);
                        if (target == null)
                            return;
                        Console.WriteLine(client.Name + " observing " + target.Name);
                        target.SendMessage(client.Name + " is observing your gear carefully.", ChatType.System);
                        client.Send(SpawnEntityPacket.Create(target));
                        for (byte loc = 1; loc < 10; loc++)
                        {
                            Structures.ConquerItem toView;
                            if (target.Equipment.TryGetItemBySlot(loc, out toView))
                                client.Send(ItemInformationPacket.CreateObserveItem(toView, target.UID));
                        }
                        //client.Send(packet);
                        break;
                    }
                #endregion
                #region Friend Info
                case DataAction.FriendInfo:
                    {
                        var target = client.Map.Search<Player>(packet.Data1);
                        if (target == null)
                            return;

                        AssociateInformationPacket assocpacket = AssociateInformationPacket.Create(target);
                        if (client.AssociateManager.Enemies.ContainsKey(target.UID))
                            assocpacket.IsEnemy = true;

                        client.Send(assocpacket);
                        client.Send(packet);
                        break;
                    }
                #endregion
                #region Enemy Info
                case DataAction.EnemyInfo:
                    {
                        var target = client.Map.Search<Player>(packet.Data1);
                        if (target == null)
                            return;

                        AssociateInformationPacket assocpacket = AssociateInformationPacket.Create(target);

                        assocpacket.IsEnemy = true;

                        client.Send(assocpacket);
                        client.Send(packet);
                        break;
                    }
                #endregion
                #region ConfirmGuild
                case DataAction.ConfirmGuild:
                    {

                        //client.GuildAttribute = new Structures.GuildAttr(client);
                        //client.GuildAttribute = Database.ServerDatabase.Context.GuildAttributes.GetById(client.UID);
                        if (client.GuildId > 0)
                        {
                            client.GuildAttribute.SendInfoToClient();
                            var guild = GuildManager.GetGuild(client.GuildId);
                            if (guild != null)
                            {
                                guild = guild.MasterGuild;
                                guild.SendInfoToClient(client);
                            }
                            client.Send(GuildPackets.Create(GuildAction.SetSyndicate, 3, 3));
                            /*if (GuildWar.CurrentWinner == user.Guild)
                                if (user.GuildRank == GuildRank.GuildLeader)
                                    user.AddFlag1(Effect1.TopGuild);
                                else if (user.GuildRank == GuildRank.DeputyLeader)
                                    user.AddFlag1(Effect1.TopDep); */



                            client.Send(new TalkPacket(ChatType.SynAnnounce, client.Guild.Announce));
                        }
                        client.Send(packet);
                        break;
                    }
                #endregion
                #region CompleteLogin
                case DataAction.CompleteLogin:
                    {
                        if (client.Character.LuckyTimeRemaining > 0)
                            client.Send(UpdatePacket.Create(client.UID, UpdateType.LuckyTime, client.Character.LuckyTimeRemaining * Common.MS_PER_SECOND));
                        if (client.Character.HeavenBlessExpires > DateTime.Now)
                        {
                            client.Send(UpdatePacket.Create(client.UID, UpdateType.HeavenBlessing, Common.SecondsFromNow(client.Character.HeavenBlessExpires)));
                            client.Send(UpdatePacket.Create(client.UID, UpdateType.SizeAdd, 2));

                            if (client.MapID != 601)
                                client.Send(UpdatePacket.Create(client.UID, UpdateType.OnlineTraining, 0));
                            else
                                client.Send(UpdatePacket.Create(client.UID, UpdateType.OnlineTraining, 1));
                        }
                        if (client.Character.DoubleExpExpires > DateTime.Now)
                            client.Send(UpdatePacket.Create(client.UID, UpdateType.DoubleExpTime, Common.SecondsFromNow(client.Character.DoubleExpExpires)));
                        client.Send(packet);
                        break;
                    }
                #endregion
                #region InvisibleEntities
                case DataAction.InvisibleEntity:
                    if (client.UID != packet.UID)
                        return;
                    foreach (var user in PlayerManager.Players.Values)
                        if (user.UID == packet.UID)
                            user.Send(SpawnEntityPacket.Create(user));
                    break;
                #endregion
                #region Teammember Map Location
                case DataAction.TeamateLoc:

                    if (client.UID == packet.UID)
                        return;
                    if (client.Team == null)
                        return;
                    Player Player = PlayerManager.GetUser(packet.UID);
                    //Player.Send(Packets.Game.SpawnEntityPacket.Create(Player, Player.Map, DataAction.TeamateLoc));
                    packet.Data1 = Player.MapID;
                    packet.Data2Low = Player.X;
                    packet.Data2High = Player.Y;
                    client.Send(packet);
                    break;
                #endregion
                #region Create Booth
                case DataAction.Vend:
                    if (!client.Map.MapInfo.Type.HasFlag(MapTypeFlags.BoothEnable))
                        return;
                    if (client.Shop == null)
                        client.Shop = new PlayerShop(client);
                    if (client.Shop.Vending)
                    {
                        //Console.WriteLine("Already vending!");
                        client.Shop.StopVending();
                    }
                    client.Shop.StartVending();
                    packet.Data1 = client.Shop.Carpet.UID;
                    client.Shop.Carpet.BaseNpc = Database.ServerDatabase.Context.Npcs.GetById(client.Shop.Carpet.UID);
                    client.Send(packet);
                    client.UpdateSurroundings();
                    break;
                #endregion
                #region Mine
                case DataAction.Mine:
                    if (Common.Clock > client.NextMine && client.Map.MapInfo.Type.HasFlag(MapTypeFlags.MineEnable) && client.Map.MineRules.Count > 0)
                    {
                        packet.UID = client.UID;
                        client.SendToScreen(packet);
                        client.Mining = true;
                        client.NextMine = Common.Clock + (Common.MS_PER_SECOND * 3);
                    }
                    break;
                #endregion
                #region Cancel Fly
                case DataAction.CancelFly:
                    if (client.HasEffect(ClientEffect.Fly))
                        client.RemoveEffect(ClientEffect.Fly);
                    if (client.HasStatus(ClientStatus.Flying))
                        client.RemoveStatus(ClientStatus.Flying);
                    break;
                #endregion
                #region Delete Character
                case DataAction.Delete:
                    client.Disconnect(false);
                    Database.ServerDatabase.Context.Accounts.DeleteCharacter(client.UID);
                    break;
                #endregion
                #region Default
                default: Console.WriteLine("Unhandled MsgActionPacket type {0} from player {1}", packet.Action, client.Character.Name); break;
                #endregion
            }
        }
        #endregion

        #region StringsPacket
        public static void Process_StringsPacket(Player client, StringsPacket packet)
        {
            switch (packet.Type)
            {
                case StringAction.QueryMate:
                    var target = client.Map.Search<Player>(packet.UID);
                    if (target != null)
                    {
                        packet.Strings.SetString(0, target.Spouse);
                        client.Send(packet);
                    }
                    break;
                case StringAction.MemberList:
                    client.Guild.SendMemberList(client);

                    break;

                case StringAction.WhisperInfo:
                    string name;
                    packet.Strings.GetString(0, out name);
                    var speaker = PlayerManager.GetUser(name);
                    if (speaker != null)
                    {
                        string toAdd = speaker.UID + " " + speaker.Level + " ";
                        toAdd += speaker.Level + " #";//battle power
                        toAdd += speaker.Guild == null ? " " : speaker.Guild.Name + " ";//unknown
                        toAdd += "#Orphan ";//unknown
                        toAdd += speaker.Spouse + " ";
                        toAdd += speaker.NobilityMedal + " ";//unknown
                        if (speaker.Lookface % 10 < 3)
                            toAdd += "0 ";
                        else
                            toAdd += "1 ";


                        packet.Strings.AddString(toAdd);
                        client.Send(packet);
                    }
                    break;
            }
        }
        #endregion

        #region ItemActionPacket
        public static void Process_ItemActionPacket(Player client, ItemActionPacket packet)
        {
            switch (packet.ActionType)
            {
                #region Player Shops
                #region Add to Booth
                #region Gold Item
                case ItemAction.BoothAdd:
                    {
                        if (client.Shop == null || !client.Shop.Vending)
                            return;
                        if (client.Shop.Items.ContainsKey(packet.UID))
                            return;
                        var toAdd = client.GetItemByUID(packet.UID);
                        if (toAdd != null && toAdd.IsSellable)
                        {
                            var saleItem = new SaleItem(toAdd, packet.ID, false);//data1
                            client.Shop.Items.TryAdd(toAdd.UniqueID, saleItem);
                            client.Send(packet);
                        }
                        break;
                    }
                #endregion
                #region CP Item
                case ItemAction.BoothAddCP:
                    {
                        if (client.Shop == null || !client.Shop.Vending)
                            return;
                        if (client.Shop.Items.ContainsKey(packet.UID))
                            return;
                        var toAdd = client.GetItemByUID(packet.UID);
                        if (toAdd != null && toAdd.IsSellable)
                        {
                            var saleItem = new SaleItem(toAdd, packet.ID, true);
                            client.Shop.Items.TryAdd(toAdd.UniqueID, saleItem);
                            client.Send(packet);
                        }
                        break;
                    }
                #endregion
                #endregion
                #region Remove From Booth
                case ItemAction.BoothDelete:
                    if (client.Shop != null && client.Shop.Vending)
                    {
                        if (client.Shop.Items.ContainsKey(packet.UID))
                        {
                            SaleItem item;
                            client.Shop.Items.TryRemove(packet.UID, out item);
                            client.Send(packet);
                        }
                    }
                    break;
                #endregion
                #region Buy From Booth
                case ItemAction.BoothBuy:
                    {
                        if (client.Inventory.Count > 39)
                            return;
                        var pts = client.Map.QueryScreen(client);
                        foreach (var o in pts)
                        {
                            if (o is Player)
                            {
                                var role = o as Player;
                                if (role == null)
                                    continue;
                                if (role.Shop == null || role.Shop.Carpet == null)
                                    continue;
                                if (!role.Shop.Items.ContainsKey(packet.UID))
                                    continue;
                                if (!role.Inventory.ContainsKey(packet.UID))
                                    continue;
                                bool bought = false;
                                //select item using what?...
                                var vI = role.Shop.Items[packet.UID];
                                if (vI.CpCost)
                                {
                                    if (client.CP < vI.Price)
                                        return;
                                    client.CP -= vI.Price;
                                    role.CP += vI.Price;
                                    bought = true;
                                }
                                else
                                {
                                    if (client.Money < vI.Price)
                                        return;
                                    client.Money -= vI.Price;
                                    role.Money += vI.Price;
                                    bought = true;
                                }
                                if (bought)
                                {
                                    client.Inventory.TryAdd(vI.Item.UniqueID, vI.Item);
                                    client.Send(Packets.Game.ItemInformationPacket.Create(vI.Item));
                                    client.Send(packet);


                                    packet.ActionType = ItemAction.BoothDelete;
                                    role.Send(packet);
                                    packet.ActionType = ItemAction.RemoveInventory;
                                    role.Send(packet);

                                    role.Send(new Packets.Game.TalkPacket(ChatType.System, client.Name + " has purchased your " + vI.Item.BaseItem.Name + " for " + vI.Price + (vI.CpCost ? "CPs" : "Silvers")));

                                    SaleItem item;
                                    Structures.ConquerItem item2;
                                    role.Shop.Items.TryRemove(vI.Item.UniqueID, out item);
                                    role.Inventory.TryRemove(vI.Item.UniqueID, out item2);
                                    //Database.ServerDatabase.Context.Items.ModifyItem(client.UID, "Owner", vI.Item.UniqueID);
                                    vI.Item.SetOwner(client);
                                }
                            }
                        }
                        break;
                    }
                #endregion
                #region Request Player Shop
                case ItemAction.BoothQuery:
                    {
                        var pts = client.Map.QueryScreen(client);
                        foreach (var o in pts)
                        {
                            if (o is Player)
                            {
                                var role = o as Player;
                                if (role != null && role.Shop != null && role.Shop.Carpet != null && role.Shop.Carpet.UID == packet.UID)
                                {
                                    foreach (SaleItem si in role.Shop.Items.Values)
                                    {
                                        SaleItem item;
                                        if (!role.Inventory.ContainsKey(si.Item.UniqueID))
                                        { role.Shop.Items.TryRemove(si.Item.UniqueID, out item); break; }
                                        var z = Packets.Game.VendorItem.Create(packet.UID, si.Item);
                                        z.Price = si.Price;
                                        if (si.CpCost)
                                            z.ViewType = 3;
                                        else z.ViewType = 1;
                                        client.Send(z);

                                    }
                                }
                            }
                        }
                    }
                    break;
                #endregion
                #endregion
                #region Equip Item
                case ItemAction.EquipItem: client.HandleItemEquipPacket(packet); break;
                #endregion
                #region Unequip Item
                case ItemAction.UnequipItem: client.HandleItemUnequipPacket(packet); break;
                #endregion
                #region Ping
                case ItemAction.Ping: client.LastPingReceived = Common.Clock; client.DirectSend(packet); break;
                #endregion
                #region Request Warehouse
                case ItemAction.ViewWarehouse:
                    if (!client.VisibleObjects.ContainsKey(packet.UID))
                        return;
                    packet.ID = client.WhMoney;
                    client.Send(packet);
                    break;
                #endregion
                #region Deposit Warehouse Money
                case ItemAction.WarehouseDeposit:
                    if (!client.VisibleObjects.ContainsKey(packet.UID))
                        return;
                    if (client.Money >= packet.ID)
                    {
                        client.WhMoney += packet.ID;
                        client.Money -= packet.ID;
                    }
                    break;
                #endregion
                #region Withdraw Warehouse Money
                case ItemAction.WarehouseWithdraw:
                    if (!client.VisibleObjects.ContainsKey(packet.UID))
                        return;
                    if (client.WhMoney >= packet.ID)
                    {
                        client.Money += packet.ID;
                        client.WhMoney -= packet.ID;
                    }
                    break;
                #endregion
                #region Repair Item
                case ItemAction.RepairItem:
                    {
                        //Note: I'm missing something in this calculation. Not sure if it's quality factoring in or what.
                        var item = client.GetItemByUID(packet.UID);
                        if (item == null || item.Durability == item.MaximumDurability)
                            return;
                        var cost = item.BaseItem.Price * (1.0 - (double)item.Durability / (double)item.MaximumDurability);
                        if (client.Money >= cost)
                        {
                            client.Money -= (uint)cost;
                            item.Durability = item.MaximumDurability;
                            client.Send(ItemInformationPacket.Create(item, ItemInfoAction.Update));
                            item.Save();
                        }
                        break;
                    }
                #endregion
                #region Meteor Level Upgrade
                case ItemAction.MeteorUpgrade:
                    {
                        //Pull relevant items
                        var mainItem = client.GetItemByUID(packet.UID);
                        var subItem = client.GetItemByUID(packet.ID);

                        //Sanity check - Valid upgradable item and a meteor.
                        if (mainItem == null ||
                            !mainItem.IsEquipment ||
                            subItem == null ||
                            subItem.StaticID != 1088001)
                            return;

                        var upChance = mainItem.ChanceToUpgradeLevel();
                        if (upChance == 0 || !client.DeleteItem(subItem))
                        { client.SendMessage("You cannot upgrade your " + mainItem.BaseItem.Name + " any further", ChatType.System); return; }
                        if (Common.PercentSuccess(mainItem.ChanceToUpgradeLevel()))
                        {
                            mainItem.ChangeItemID(mainItem.GetNextItemLevel());
                            if (Common.PercentSuccess(Constants.SOCKET_RATE))
                            {
                                if (mainItem.Gem1 == 0)
                                {
                                    mainItem.Gem1 = 255;
                                    PlayerManager.SendToServer(new TalkPacket(ChatType.GM, "As a very lucky player, " + client.Name + " has added the first socket to his/her " + mainItem.BaseItem.Name));
                                }
                                else if (mainItem.Gem2 == 0)
                                {
                                    mainItem.Gem2 = 255;
                                    PlayerManager.SendToServer(new TalkPacket(ChatType.GM, "As a very lucky player, " + client.Name + " has added the second socket to his/her " + mainItem.BaseItem.Name));
                                }
                                mainItem.Save();
                            }
                            client.Send(ItemInformationPacket.Create(mainItem, ItemInfoAction.Update));
                            client.SendMessage("You have successfully upgraded the level of your " + mainItem.BaseItem.Name, ChatType.System);
                        }
                        else
                        {
                            client.SendMessage("You have failed to upgrade the level of your " + mainItem.BaseItem.Name, ChatType.System);
                            mainItem.Durability = (ushort)(mainItem.MaximumDurability / 2);
                            client.Send(ItemInformationPacket.Create(mainItem, ItemInfoAction.Update));
                            mainItem.Save();

                        }

                        break;
                    }
                #endregion
                #region Dragon Ball Quality Upgrade
                case ItemAction.DragonBallUpgrade:
                    {
                        var mainItem = client.GetItemByUID(packet.UID);
                        var subItem = client.GetItemByUID(packet.ID);

                        //Sanity check - Valid upgradable item and a dragon ball.
                        if (mainItem == null ||
                            !mainItem.IsEquipment ||
                            subItem == null ||
                            subItem.StaticID != 1088000)
                            return;
                        var upChance = mainItem.ChanceToUpgradeQuality();
                        if (upChance == 0 || !client.DeleteItem(subItem))
                        { client.SendMessage("You cannot upgrade your " + mainItem.BaseItem.Name + " any further", ChatType.System); return; }
                        if (Common.PercentSuccess(upChance))
                        {
                            mainItem.ChangeItemID(mainItem.GetNextItemQuality());
                            if (Common.PercentSuccess(Constants.SOCKET_RATE * 2))
                            {
                                if (mainItem.Gem1 == 0)
                                {
                                    mainItem.Gem1 = 255;
                                    PlayerManager.SendToServer(new TalkPacket(ChatType.GM, "As a very lucky player, " + client.Name + " has added the first socket to his/her " + mainItem.BaseItem.Name));
                                }
                                else if (mainItem.Gem2 == 0)
                                {
                                    mainItem.Gem2 = 255;
                                    PlayerManager.SendToServer(new TalkPacket(ChatType.GM, "As a very lucky player, " + client.Name + " has added the second socket to his/her " + mainItem.BaseItem.Name));
                                }
                                mainItem.Save();
                            }

                            client.Send(ItemInformationPacket.Create(mainItem, ItemInfoAction.Update));
                            client.SendMessage("You have successfully upgraded the quality of your " + mainItem.BaseItem.Name, ChatType.System);
                        }
                        else
                        {
                            client.SendMessage("You have failed to upgrade the quality of your " + mainItem.BaseItem.Name, ChatType.System);
                            mainItem.Durability = (ushort)(mainItem.MaximumDurability / 2);
                            client.Send(ItemInformationPacket.Create(mainItem, ItemInfoAction.Update));
                            mainItem.Save();
                        }

                    }
                    break;
                #endregion
                #region Drop item
                case ItemAction.DropItem:
                    {
                        var item = client.GetItemByUID(packet.UID);
                        if (item != null && item.IsDropable)
                        {
                            var loc = client.Location;
                            for (var i = 0; i < 9; i++)
                                if (!client.Map.IsValidItemLocation(loc))
                                {
                                    loc.X = client.Location.X + Common.DeltaX[i];
                                    loc.Y = client.Location.Y + Common.DeltaY[i];
                                }
                            if (client.Map.IsValidItemLocation(loc))
                            {
                                var log = new DbItemLog(item.DbItem);
                                if (client.RemoveItem(item))
                                {
                                    Database.ServerDatabase.Context.ItemLogs.Add(log);
                                    item.Delete();
                                    var gi = new GroundItem(item, (uint)client.Map.ItemCounter.Counter, loc, client.Map);
                                    gi.AddToMap();
                                    break;
                                }
                            }

                        }
                        break;
                    }
                #endregion
                #region Buy From NPC
                case ItemAction.BuyFromNPC:
                    if (packet.Amount < 1)
                        packet.Amount = 1;
                    var shopItem = Database.ServerDatabase.Context.ShopItems.GetShopItem((ushort)packet.UID, packet.ID);
                    if (shopItem == null)
                        Console.WriteLine("{0} trying to buy nonexistant item ID {1} from shop ID {2}", client.Name, packet.ID, packet.UID);
                    else if (shopItem.CurrencyType == CurrencyType.Silver && !client.VisibleObjects.ContainsKey(packet.UID))
                        Console.WriteLine("{0} trying to buy item from non visible shop ID {1}", client.Name, packet.UID);
                    else
                    {
                        var toBuy = Database.ServerDatabase.Context.ItemInformation.GetById(packet.ID);
                        if (toBuy != null)
                            for (var i = 0; i < packet.Amount; i++)
                            {
                                if (client.Inventory.Count >= 40)
                                    break;
                                bool success = true;
                                if (shopItem.CurrencyType == CurrencyType.Silver && client.Money >= toBuy.Price)
                                    client.Money -= toBuy.Price;
                                else if (shopItem.CurrencyType == CurrencyType.CP && client.CP >= toBuy.PriceCP)
                                    client.CP -= toBuy.PriceCP;
                                else
                                    success = false;
                                if (success)
                                    client.CreateItem(toBuy);
                            }
                    }
                    break;
                #endregion
                #region Sell To NPC
                /// <summary>
                /// Handles selling item to NPC.
                /// Written by Aceking 9-21-13
                /// </summary>
                case ItemAction.SellToNPC:
                    var sellItem = client.GetItemByUID(packet.ID);
                    if (sellItem != null)
                    {
                        uint sellPrice = sellItem.BaseItem.Price / 3;
                        if (sellItem.Durability == 0)
                            sellPrice = 0;
                        else if (sellItem.Durability < sellItem.MaximumDurability)
                        {
                            sellPrice *= (uint)(sellItem.Durability / sellItem.MaximumDurability);
                        }
                        client.Money += sellPrice;
                        client.DeleteItem(sellItem);
                    }
                    break;
                #endregion
                case ItemAction.Enchant:
                    if (!client.Alive)
                        return;
                    var main = client.GetItemByUID(packet.UID);
                    if (main == null || main.Location > 0)
                        return;
                    if (main.Enchant >= 255)
                        return;
                    var minor = client.GetItemByUID(packet.ID);
                    if (minor == null || minor.StaticID / 100000 != 7)
                        return;
                    var gemID = (byte)(minor.StaticID % 100);
                    var enchant = 0;
                    switch (gemID)
                    {
                        case 1:
                        case 11:
                        case 21:
                        case 31:
                        case 41:
                        case 51:
                        case 61:
                        case 71:
                            enchant = Common.Random.Next(1, 60);
                            break;
                        case 12:
                            enchant = Common.Random.Next(100, 160);
                            break;
                        case 2:
                        case 52:
                        case 62:
                            enchant = Common.Random.Next(60, 110);
                            break;
                        case 22:
                        case 42:
                        case 72:
                            enchant = Common.Random.Next(40, 90);
                            break;
                        case 32:
                            enchant = Common.Random.Next(80, 130);
                            break;
                        case 13:
                            enchant = Common.Random.Next(200, 256);
                            break;
                        case 3:
                        case 33:
                        case 73:
                            enchant = Common.Random.Next(170, 230);
                            break;
                        case 53:
                        case 63:
                            enchant = Common.Random.Next(140, 200);
                            break;
                        case 43:
                            enchant = Common.Random.Next(70, 120);
                            break;
                    }
                    client.DeleteItem(minor);
                    packet.ID = (uint)enchant;
                    client.Send(packet);
                    if (main.Enchant < enchant)
                    {
                        main.Enchant = (byte)enchant;
                        main.Save();
                        client.Send(ItemInformationPacket.Create(main, ItemInfoAction.Update));
                    }
                    break;
                default: Console.WriteLine("Unhandled ItemActionPacket type {0} from player {1}", packet.ActionType, client.Character.Name); break;
            }
        }
        #endregion

        #region TeamInteractPacket
        public static void Process_TeamInteractPacket(Player client, TeamInteractPacket packet)
        {
            switch (packet.Type)
            {
                #region Create

                case TeamInteractType.Create:
                    if (client.Team == null)
                    {
                        client.Team = new TeamManager(client);
                        client.AddEffect(ClientEffect.TeamLeader, 0);
                        client.Send(packet);
                    }
                    break;

                #endregion
                #region RequestJoin

                case TeamInteractType.RequestJoin:
                    {
                        if (client.Team != null)
                        {
                            client.SendMessage("You are already in a team", ChatType.System);
                            return;
                        }
                        var target = client.Map.Search<Player>(packet.UID);
                        if (target == null)
                        {
                            client.SendMessage("Invalid target", ChatType.System);
                            return;
                        }
                        if (target.Team == null)
                        {
                            client.SendMessage(target.Name + " does not have a team!", ChatType.System);
                            return;
                        }
                        if (target.Team.GetMemberCount > 3)
                        {
                            client.SendMessage(target.Name + "'s team is full!", ChatType.System);
                            return;
                        }
                        if (!target.Team.IsJoinEnabled)
                        {
                            client.SendMessage(target.Name + "'s team does not allow new members!", ChatType.System);
                            return;
                        }

                        packet.UID = client.UID;
                        target.Team.Leader.Send(packet);
                        client.SendMessage("Request to join team has been sent", ChatType.System);
                        break;
                    }

                #endregion
                #region AcceptJoin

                case TeamInteractType.AcceptJoin:
                    {
                        if (client.Team == null || client.Team.Leader != client || client.Team.GetMemberCount > 3 ||
                            !client.Team.IsJoinEnabled)
                        {
                            client.SendMessage("You cannot accept new team members", ChatType.System);
                            return;
                        }

                        var target = client.Map.Search<Player>(packet.UID);
                        if (target == null || target.Team != null || client.Team.Members.Contains(target))
                        {
                            client.SendMessage("Target failed to join team", ChatType.System);
                            break;
                        }

                        target.Send(packet);
                        //Add Leader
                        target.Send(TeamMemberInformation.Create(client, TeamMemberAction.AddMember));
                        //Add themselves
                        target.Send(TeamMemberInformation.Create(target, TeamMemberAction.AddMember));
                        //Add the new member to the team+
                        client.Team.SendToTeam(TeamMemberInformation.Create(target, TeamMemberAction.AddMember));

                        client.Team.Members.Add(target);
                        target.Team = client.Team;

                        target.SendMessage("Request to join team has been accepted.", ChatType.System);
                        break;
                    }

                #endregion
                #region RequestInvite

                case TeamInteractType.RequestInvite:
                    {
                        if (client.Team == null || client.Team.Leader != client || client.Team.GetMemberCount > 3)
                        {
                            client.SendMessage("You cannot invite new players", ChatType.System);
                            return;
                        }

                        var target = client.Map.Search<Player>(packet.UID);
                        if (target == null || target.Team != null)
                        {
                            client.SendMessage("Cannot invite target to team", ChatType.System);
                            return;
                        }
                        packet.UID = client.UID;
                        target.Send(packet);
                        client.SendMessage("Team invitation has been sent", ChatType.System);
                        break;
                    }

                #endregion
                #region Accept Invite
                case TeamInteractType.AcceptInvite:
                    {
                        if (client.Team != null)
                            return;
                        var target = client.Map.Search<Player>(packet.UID);
                        if (target == null || target.Team == null || target.Team.Leader != target ||
                            !target.Team.IsJoinEnabled || target.Team.GetMemberCount > 3 ||
                            target.Team.Members.Contains(client))
                        {
                            client.SendMessage("Failed to join team", ChatType.System);
                            return;
                        }

                        target.Send(packet);
                        //Add Leader
                        client.Send(TeamMemberInformation.Create(target, TeamMemberAction.AddMember));
                        //Add themselves
                        client.Send(TeamMemberInformation.Create(client, TeamMemberAction.AddMember));
                        //Add the new member to the team+
                        target.Team.SendToTeam(TeamMemberInformation.Create(client, TeamMemberAction.AddMember));

                        client.Team = target.Team;
                        client.Team.Members.Add(client);
                    }
                    break;
                #endregion
                #region LeaveTeam
                case TeamInteractType.LeaveTeam:
                    {
                        if (client.Team == null || client.Team.Leader == client)
                            return;

                        client.Team.SendToTeam(packet);
                        if (client.Team.Members.Contains(client))
                            client.Team.Members.Remove(client);

                        client.Team.SendToTeam(new TalkPacket(ChatType.System, client.Name + " has left the team."));
                        client.Team = null;

                        break;
                    }
                #endregion
                #region Dismiss
                case TeamInteractType.Dismiss:
                    {
                        if (client.Team == null || client.Team.Leader != client)
                        {
                            client.SendMessage("Cannot dismiss team", ChatType.System);
                            return;
                        }

                        foreach (var member in client.Team.Members)
                        {
                            member.Team = null;
                            packet.UID = member.UID;
                            member.Send(packet);
                        }

                        client.Send(packet);


                        client.Team = null;
                        client.RemoveEffect(ClientEffect.TeamLeader);

                        break;
                    }
                #endregion
                #region Kick
                case TeamInteractType.Kick:
                    {
                        if (client.Team == null)
                            return;
                        var target = PlayerManager.GetUser(packet.UID);
                        if (target == null)
                            return;
                        if (client.Team.Leader.UID == client.UID || client.Team.Members.Contains(target))
                        {
                            client.Team.SendToTeam(TeamInteractPacket.Create(target.UID, TeamInteractType.Kick));
                            client.Team.Members.Remove(target);
                            //target.Send(packet);
                            target.Team = null;
                        }
                        break;
                    }
                #endregion
                #region ForbidNewMembers
                case TeamInteractType.ForbidNewMembers:
                    if (client.Team != null && client == client.Team.Leader)
                    {
                        client.Team.IsJoinEnabled = false;
                        client.Team.SendToTeam(packet);
                    }
                    break;
                #endregion
                #region AllowNewMembers
                case TeamInteractType.AllowNewMembers:
                    if (client.Team != null && client == client.Team.Leader)
                    {
                        client.Team.IsJoinEnabled = true;
                        client.Team.SendToTeam(packet);
                    }
                    break;
                #endregion
                #region ForbidItems
                case TeamInteractType.ForbidItems:
                    if (client.Team != null && client == client.Team.Leader)
                    {
                        client.Team.IsItemEnabled = false;
                        client.Team.SendToTeam(packet);
                    }
                    break;
                #endregion
                #region AllowItems
                case TeamInteractType.AllowItems:
                    if (client.Team != null && client == client.Team.Leader)
                    {
                        client.Team.IsItemEnabled = true;
                        client.Team.SendToTeam(packet);
                    }
                    break;
                #endregion
                #region ForbidMoney
                case TeamInteractType.ForbidMoney:
                    if (client.Team != null && client == client.Team.Leader)
                    {
                        client.Team.IsMoneyEnabled = false;
                        client.Team.SendToTeam(packet);
                    }
                    break;
                #endregion
                #region AllowMoney
                case TeamInteractType.AllowMoney:
                    if (client.Team != null && client == client.Team.Leader)
                    {
                        client.Team.IsMoneyEnabled = true;
                        client.Team.SendToTeam(packet);
                    }
                    break;
                #endregion
                default:
                    Console.WriteLine("Unhandled TeamInteractPacket type {0} from player {1}", packet.Type,
                                      client.Character.Name);
                    break;
            }
        }
        #endregion

        #region WalkPacket
        public static void Process_WalkPacket(Player client, WalkPacket packet)
        {
            byte direction = (byte)(packet.Direction % 8);
            ushort x = (ushort)(client.X + Common.DeltaX[direction]);
            ushort y = (ushort)(client.Y + Common.DeltaY[direction]);

            // Validate action:
            if (client.Map.IsValidPlayerLocation(new Point(x, y)))
            {
                client.SendToScreen(packet, true);
                client.Direction = direction;
                client.X = x;
                client.Y = y;
                client.OnMove();
                client.UpdateSurroundings();
            }
            else
            {
                client.Send(new GeneralActionPacket()
                {
                    UID = client.UID,
                    Data1 = client.MapID,
                    Data2Low = client.X,
                    Data2High = client.Y,
                    Action = DataAction.NewCoordinates
                });
                Console.WriteLine("Invalid walk location for {0}", client.Name);
                //client.Disconnect(true);
            }
        }
        #endregion

        #region AssociatePacket
        public static void Process_AssociatePacket(Player client, AssociatePacket packet)
        {
            switch (packet.Action)
            {
                #region RequestFriend
                case AssociateAction.RequestFriend:
                    if (!client.AssociateManager.HasFriend(packet.FriendID))
                    {
                        Player friend = client.Map.Search<Player>(packet.FriendID);
                        if (friend != null)
                        {
                            #region Accept
                            if (friend.AssociateManager.FriendRequestUID == client.UID)
                            {
                                friend.AssociateManager.AddFriend(client);
                                client.AssociateManager.AddFriend(friend);
                                friend.Send(new TalkPacket(ChatType.Talk, "You and " + client.Name + " are now friends!"));
                                client.Send(new TalkPacket(ChatType.Talk, "You and " + friend.Name + " are now friends!"));
                                friend.AssociateManager.FriendRequestUID = 0;
                                client.AssociateManager.FriendRequestUID = 0;
                            }
                            #endregion
                            #region Request
                            else
                            {
                                client.AssociateManager.FriendRequestUID = friend.UID;
                                client.Send(new TalkPacket(ChatType.System, "Friend request has been sent out.", ChatColour.Red));
                                friend.Send(new TalkPacket(ChatType.System, client.Name + " wishes to make friends with you.", ChatColour.Red));
                            }
                            #endregion
                        }
                        else
                            client.Send(new TalkPacket(ChatType.Talk, "Friend request failed: user could not be found.", ChatColour.Red));
                    }
                    else
                        client.Send(new TalkPacket(ChatType.System, "You are already friends!", ChatColour.Red));
                    break;
                #endregion
                #region RemoveFriend
                case AssociateAction.RemoveFriend:
                    client.AssociateManager.RemoveFriend(packet.FriendID);
                    break;
                #endregion
                #region RemoveEnemy
                case AssociateAction.RemoveEnemy:
                    client.AssociateManager.RemoveEnemy(packet.FriendID);
                    break;
                #endregion
                default: Console.WriteLine("Unhandled AssociatePacket type {0} from player {1}", packet.Action, client.Character.Name); break;
            }
        }
        #endregion

        #region SocketGemPacket
        public static void Process_SocketGemPacket(Player client, SocketGemPacket packet)
        {
            var item = client.GetItemByUID(packet.ItemID);
            if (item == null)
                return;
            switch (packet.Action)
            {
                #region Remove Gem
                case SocketGemAction.RemoveGem:
                    if (packet.Location == 1 && item.Gem1 > 0)
                    {
                        item.Gem1 = 255;
                        client.Send(ItemInformationPacket.Create(item, ItemInfoAction.Update));
                        item.Save();
                    }
                    if (packet.Location == 2 && item.Gem2 > 0)
                    {
                        item.Gem2 = 255;
                        client.Send(ItemInformationPacket.Create(item, ItemInfoAction.Update));
                        item.Save();
                    }
                    break;
                #endregion
                #region Add Gem
                case SocketGemAction.AddGem:
                    var gem = client.GetItemByUID(packet.GemID);
                    if (gem == null)
                        return;
                    if (packet.Location == 1 && item.Gem1 == 255)
                    {
                        item.Gem1 = (byte)(gem.StaticID % 1000);
                        client.RemoveItem(gem);
                        client.Send(ItemInformationPacket.Create(item, ItemInfoAction.Update));
                        item.Save();
                        gem.Delete();
                    }
                    else if (packet.Location == 2 && item.Gem2 == 255)
                    {
                        item.Gem2 = (byte)(gem.StaticID % 1000);
                        client.RemoveItem(gem);
                        client.Send(ItemInformationPacket.Create(item, ItemInfoAction.Update));
                        item.Save();
                        gem.Delete();
                    }
                    break;
                #endregion
            }
        }
        #endregion

        #region NpcPacket
        public static void Process_NpcPacket(Player client, NpcPacket packet)
        {
            Npcs.Manager.ProcessNpc(client, packet.UID, packet.Type);
        }
        #endregion

        #region AssignAttributesPacket
        public static void Process_AssignAttributesPacket(Player client, AssignAttributesPacket packet)
        {
            if (packet.Strength > 0 && client.ExtraStats >= packet.Strength)
            {
                client.Strength += packet.Strength;
                client.ExtraStats -= packet.Strength;
            }
            if (packet.Agility > 0 && client.ExtraStats >= packet.Agility)
            {
                client.Agility += packet.Agility;
                client.ExtraStats -= packet.Agility;
            }
            if (packet.Vitality > 0 && client.ExtraStats >= packet.Vitality)
            {
                client.Vitality += packet.Vitality;
                client.ExtraStats -= packet.Vitality;
            }
            if (packet.Spirit > 0 && client.ExtraStats >= packet.Spirit)
            {
                client.Spirit += packet.Spirit;
                client.ExtraStats -= packet.Spirit;
            }
        }
        #endregion

        #region NpcDialogPacket
        public static void Process_NpcDialogPacket(Player _client, NpcDialogPacket _packet)
        {
            switch (_packet.Action)
            {
                case DialogAction.DeleteMember:
                    if (_packet.Action == DialogAction.DeleteMember)
                    {
                        String Name;
                        _packet.Strings.GetString(1, out Name);

                        Name = Name.Substring(11, Name.Length - 12);
                        string[] split = Name.Split('\0');

                        Name = split[0];

                        if (_client.GuildRank == GuildRank.GuildLeader && _client.Name != Name)
                        {

                            if (PlayerManager.GetUser(Name) != null)
                            {
                                var player = PlayerManager.GetUser(Name);
                                player.Send(new TalkPacket(ChatType.Syndicate, Name + " has been kicked out of the guild."));
                                player.GuildAttribute.LeaveGuild();

                            }
                            _client.Guild.SaveInfo();

                            foreach (var member in _client.Guild.Members())
                            {
                                member.Send(new TalkPacket(ChatType.Syndicate, Name + " has been kicked out of the guild."));
                                member.GuildAttribute.SendInfoToClient();
                            }





                            _client.Guild.SendMemberList(_client);
                        }
                        else
                            _client.SendMessage("You can not discharge yourself! ", ChatType.System);
                    }

                    break;
                default:
                    if (_packet.Linkback == 255)
                        _client.CurrentNPC = null;
                    if (_client.CurrentNPC != null)
                    {
                        String str;
                        _packet.Strings.GetString(1, out str);

                        str = str.Substring(11, str.Length - 12);
                        string[] split = str.Split('\0');

                        _client.NpcInputBox = split[0];

                        _client.CurrentNPC.Run(_client, _packet.Linkback);
                    }
                    break;


            }
        }
        #endregion

        #region GroundItemPacket
        public static void Process_GroundItemPacket(Player client, GroundItemPacket packet)
        {
            switch (packet.Action)
            {
                case GroundItemAction.Pick:
                    var gi = client.Map.Search<GroundItem>(packet.UID);
                    if (gi != null)
                    {
                        switch (gi.Currency)
                        {
                            case CurrencyType.CP:
                                client.CP += gi.Value;
                                client.SendMessage("You have looted " + gi.Value + " CP", ChatType.System);
                                gi.RemoveFromMap();
                                break;
                            case CurrencyType.Silver:
                                client.Money += gi.Value;
                                client.SendMessage("You have looted " + gi.Value + " Silver", ChatType.System);
                                gi.RemoveFromMap();
                                break;
                            case CurrencyType.None:
                                if (gi.Item.UniqueID == gi.UID)
                                    gi.Item.UniqueID = (uint)Common.ItemGenerator.Counter;
                                if (client.AddItem(gi.Item))
                                {
                                    client.Send(ItemInformationPacket.Create(gi.Item));
                                    gi.Item.SetOwner(client);
                                    gi.RemoveFromMap();
                                }
                                break;
                        }
                    }
                    break;
                default:
                    Console.WriteLine("Unhandled GroundItemPacket action {0}", packet.Action);
                    break;
            }
        }
        #endregion

        #region Compose Packet
        public static void Process_ComposePacket(Player client, ComposePacket packet)
        {
            switch (packet.Action)
            {
                case 0:
                    {
                        if (packet.Values.Count < 3)
                        {
                            Console.WriteLine("Error: Not enough values for compose packet from client {0}", client.Name);
                            return;
                        }

                        var mainItem = client.GetItemByUID(packet.Values[0]);
                        var minor1 = client.GetItemByUID(packet.Values[1]);
                        var minor2 = client.GetItemByUID(packet.Values[2]);

                        //Check that items exist in our inventory
                        if (mainItem == null || minor1 == null || minor2 == null)
                            return;

                        //Cannot upgrade past +9
                        if (mainItem.plus >= 9)
                        { client.SendMessage("You cannot upgrade your " + mainItem.BaseItem.Name + " any further", ChatType.System); return; }

                        //Minor items must be at least +1 and have matching values.
                        if (minor1.plus == 0 || minor2.plus == 0 || minor1.plus != minor2.plus)
                        { client.SendMessage("You must insert two minor items of equal value", ChatType.System); return; }

                        //If all 3 items are NOT same +, assign minor item + value (EG: 2x +1 = +1)
                        if (minor1.plus - mainItem.plus > 0)
                        {
                            mainItem.Plus = minor1.plus;
                            client.Send(ItemInformationPacket.Create(mainItem, ItemInfoAction.Update));
                            mainItem.Save();
                        }
                        //If all 3 items ARE same +, boost composition by 1 (EG: 3x +1 = +2)
                        else
                        {
                            mainItem.Plus += 1;
                            client.Send(ItemInformationPacket.Create(mainItem, ItemInfoAction.Update));
                            mainItem.Save();
                        }

                        client.DeleteItem(minor1);
                        client.DeleteItem(minor2);
                        break;
                    }
                default:
                    Console.WriteLine("Error: Unknown Compose Action {0} from {1}", packet.Action, client.Name);
                    break;
            }
        }
        #endregion

        #region Guild Packet
        public static void Process_GuildPacket(Player client, GuildPackets packet)
        {
            switch (packet.Action)
            {
                case GuildAction.ApplyJoin:
                    {
                        if (packet.Data == 0 || packet.Data == client.UID)
                            return;

                        var target = PlayerManager.GetUser(packet.Data);
                        if (target == null) return;

                        var guildId = client.GuildId;
                        var targetGuildId = target.GuildId;
                        var targetGuildRank = target.GuildRank;

                        if (guildId != 0 || targetGuildId == 0 || targetGuildRank < GuildRank.Member)
                        {
                            client.SendSysMessage("Error: failed to join!");
                            return;
                        }

                        var targetGuild = GuildManager.GetGuild(targetGuildId);
                        if (targetGuild == null) return;

                        /* if (!targetGuild.CanJoin(client))
                         {
                             client.SendSysMessage("You are unable to join!");
                             return;
                         }*/

                        if (!target.FetchApply(ApplyType.InviteJoinGuild, client.UID))
                        {
                            client.SetApply(ApplyType.JoinGuild, target.UID);
                            target.Send(GuildPackets.Create(GuildAction.ApplyJoin, client.UID, 0));
                            return;
                        }

                        client.GuildAttribute.JoinGuild(targetGuildId);

                        break;
                    }
                case GuildAction.InviteJoin:
                    {
                        if (packet.Data == 0 || packet.Data == client.UID)
                            return;

                        var target = PlayerManager.GetUser(packet.Data);
                        if (target == null) return;

                        var guildId = client.GuildId;
                        var guildRank = client.GuildRank;
                        var targetGuildId = target.GuildId;
                        var targetGuildRank = target.GuildRank;

                        if (guildId == 0 || guildRank < GuildRank.Member || targetGuildId != 0)
                        {
                            client.SendSysMessage("Error: failed to join!");
                            return;
                        }

                        var guild = GuildManager.GetGuild(guildId);
                        if (guild == null) return;

                        if (!target.FetchApply(ApplyType.JoinGuild, client.UID))
                        {
                            client.SetApply(ApplyType.InviteJoinGuild, target.UID);
                            target.Send(GuildPackets.Create(GuildAction.InviteJoin, client.UID, 0));
                            return;
                        }
                        // guild.Amount--;
                        target.GuildAttribute.JoinGuild(guildId);
                        target.Guild.SaveInfo();

                        foreach (var member in client.Guild.Members())
                        {
                            member.Send(new TalkPacket(ChatType.Syndicate, target.Name + " has joined the guild."));
                            member.GuildAttribute.SendInfoToClient();
                        }

                        break;
                    }
                case GuildAction.LeaveSyndicate:
                    {
                        var guildId = client.GuildId;
                        if (guildId == 0) return;

                        var rank = client.GuildRank;
                        if (rank == GuildRank.GuildLeader)
                        {
                            client.SendSysMessage("The guild leader cannot leave the guild. Please transfer leadership.");
                            return;
                        }

                        client.GuildAttribute.LeaveGuild();

                        break;
                    }
                case GuildAction.QuerySyndicateName:
                    {
                        var guildId = packet.Data;
                        var guild = GuildManager.GetGuild(guildId);
                        if (guild == null) return;

                        var masterGuild = guild.MasterGuild;
                        if (masterGuild == null) return;

                        var msg = StringsPacket.Create(guildId, StringAction.Guild, masterGuild.StringInfo);
                        if (guild.Id != masterGuild.Id)
                        {
                            msg.Strings.AddString(guild.Name);
                        }
                        client.Send(msg);
                        break;
                    }
                case GuildAction.SetAlly:
                    {
                        if (client.GuildId == 0) return;

                        string name;
                        if (!packet.Strings.GetString(0, out name)) return;

                        client.GuildAttribute.AddAlly(name);
                        break;
                    }
                case GuildAction.ClearAlly:
                    {
                        if (client.GuildId == 0) return;

                        string name;
                        if (!packet.Strings.GetString(0, out name)) return;

                        var guildId = client.GuildId;
                        var guild = GuildManager.GetGuild(guildId);
                        if (guild == null) return;

                        if (client.GuildRank != GuildRank.GuildLeader)
                            return;

                        var targetGuild = GuildManager.GetGuildByName(name);
                        if (targetGuild == null) return;
                        targetGuild = targetGuild.MasterGuild;

                        for (var i = 0; i < 5; i++)
                        {
                            var targetId = guild.GetAlly(i);
                            if (targetId == targetGuild.Id)
                            {
                                guild.SetAlly(i, 0);
                                guild.BroadcastGuildMsg(GuildPackets.Create(GuildAction.ClearAlly, targetId, 0));

                                guild.BroadcastGuildMsg(string.Format("[rank 1000] {0} has removed Guild {1} from the allies list!", client.Name, targetGuild.Name));
                            }
                        }

                        for (var i = 0; i < 5; i++)
                        {
                            var targetId = targetGuild.GetAlly(i);
                            if (targetId == guild.Id)
                            {
                                targetGuild.SetAlly(i, 0);
                                targetGuild.BroadcastGuildMsg(GuildPackets.Create(GuildAction.ClearAlly, targetId, 0));

                                targetGuild.BroadcastGuildMsg(string.Format("[rank 1000] {0} has removed Guild {1} from the allies list!", client.Name, targetGuild.Name));
                            }
                        }

                        guild.SynchroInfo();
                        targetGuild.SynchroInfo();

                        break;
                    }
                case GuildAction.SetEnemy:
                    {
                        if (client.GuildId == 0) return;

                        string name;
                        if (!packet.Strings.GetString(0, out name)) return;

                        client.GuildAttribute.AddEnemy(name);
                        break;
                    }
                case GuildAction.ClearEnemy:
                    {
                        if (client.GuildId == 0) return;

                        string name;
                        if (!packet.Strings.GetString(0, out name)) return;

                        client.GuildAttribute.RemoveEnemy(name);
                        break;
                    }
                case GuildAction.DonateMoney:
                    {
                        if (client.GuildId == 0) return;

                        client.GuildAttribute.DonateMoney(packet.Data);
                        client.GuildAttribute.SaveInfo();
                        break;
                    }
                case GuildAction.QuerySyndicateAttribute:
                    {
                        client.GuildAttribute.SendInfoToClient();
                        break;
                    }

                case GuildAction.Unknown23:
                    {
                        if (client.GuildId == 0) return;
                        if (packet.Data == 0 || packet.Data == client.GuildId) return;

                        string name;
                        if (!packet.Strings.GetString(0, out name)) return;

                        var targetGuild = GuildManager.GetGuildByName(name);
                        if (targetGuild == null) return;

                        client.GuildAttribute.AddAlly(name);
                        break;
                    }
                case GuildAction.SetAnnounce:
                    {
                        if (client.GuildId == 0) return;

                        var guild = GuildManager.GetGuild(client.GuildId);
                        if (guild == null) return;

                        if (client.GuildRank != GuildRank.GuildLeader)
                        {
                            client.SendSysMessage("You have not been authorized!");
                            return;
                        }

                        string announce;
                        if (!packet.Strings.GetString(0, out announce)) return;

                        guild.SetAnnounce(announce);
                        break;
                    }
                case GuildAction.PromoteMember:
                    {
                        if (client.GuildId == 0) return;

                        string name;
                        if (!packet.Strings.GetString(0, out name)) return;

                        client.GuildAttribute.PromoteMember(name, GuildRank.DeputyLeader);
                        break;
                    }
                case GuildAction.DischargeMember:
                    {
                        if (client.GuildId == 0) return;

                        string name;
                        if (!packet.Strings.GetString(0, out name)) return;

                        client.GuildAttribute.DemoteMember(name);
                        break;
                    }
                case GuildAction.PromoteInfo:
                    {
                        if (client.GuildId == 0) return;

                        client.GuildAttribute.SendPromotionInfoToClient();
                        break;
                    }

            }

        }

        #endregion

        #region GuildMemberInfo
        public static void Process_GuildMemberInfo(Player client, GuildMemberInformation packet)
        {
            if (packet.Name == null)
                return;

            var member = Database.ServerDatabase.Context.Characters.GetByName(packet.Name);

            var guildinfo = Database.ServerDatabase.Context.GuildAttributes.GetById(member.UID);

            packet.Rank = guildinfo.Rank;
            packet.Donation = guildinfo.SilverDonation;
            client.Send(packet);
        }
        #endregion

        #region Nobility
        public static void Process_Nobility(Player client, Nobility packet)
        {
            switch (packet.Type)
            {
                case NobilityAction.Donate:
                    if (client.Money >= packet.Donation)
                    {
                        client.Donation += packet.Donation;
                        client.Money -= (uint)packet.Donation;

                        client.Send(Nobility.UpdateIcon(client));
                    }
                    else if (client.CP >= packet.Donation / 50000)
                    {
                        client.Donation += packet.Donation;
                        client.CP -= (uint)(packet.Donation / 50000);

                        client.Send(Nobility.UpdateIcon(client));
                    }

                    break;

                case NobilityAction.List:
                    packet.Strings = Managers.NobilityManager.GetPage((int)packet.Data1);

                    client.Send(packet);
                    break;

                case NobilityAction.QueryRemainingSilver:
                    switch (packet.Data1)
                    {
                        case 12://King
                            if (NobilityManager.AllRanks.Count >= 3)
                                packet.Data1 = Math.Max((uint)(NobilityManager.AllRanks[2].Donation - client.Donation), 3000000);
                            else
                                packet.Data1 = 3000000;
                            break;

                        case 9://Prince
                            if (NobilityManager.AllRanks.Count >= 12)
                                packet.Data1 = Math.Max((uint)(NobilityManager.AllRanks[11].Donation - client.Donation), 3000000);
                            else
                                packet.Data1 = 3000000;
                            break;

                        case 7://Duke
                            if (NobilityManager.AllRanks.Count >= 35)
                                packet.Data1 = Math.Max((uint)(NobilityManager.AllRanks[34].Donation - client.Donation), 3000000);
                            else
                                packet.Data1 = 3000000;
                            break;

                        case 5://Earl
                            packet.Data1 = Math.Max((uint)(200000000 - client.Donation), 3000000);

                            break;

                        case 3://Baron
                            packet.Data1 = Math.Max((uint)(100000000 - client.Donation), 3000000);

                            break;

                        case 1://Knight
                            if (NobilityManager.AllRanks.Count >= 3)
                                packet.Data1 = Math.Max((uint)(30000000 - client.Donation), 3000000);

                            break;
                    }
                    packet.Data1 += 1;//Add 1 so they are 1 gold in front of the last person in that rank.
                    client.Send(packet);
                    break;
            }
        }
        #endregion

        #region Trade
        public static void Process_Trade(Player client, TradePacket packet)
        {
            if (client.Trade == null)
            {
                client.Trade = new Managers.TradeSequence
                {
                    Owner = client
                };
            }
            client.Trade.ProcessTradePacket(client, packet);
        }
        #endregion

        #region OfflineTG
        public static void Process_OfflineTG(Player client, OfflineTGPacket packet)
        {
            switch (packet.Type)
            {
                //Requesting time that can be trained
                case 0:
                    {
                        //uint Time = (uint)(((Common.Clock - client.LoginTime) / Common.MS_PER_MINUTE) * 10);
                        long Time = Math.Min(900, ((DateTime.Now.Ticks - client.LoginTime) / TimeSpan.TicksPerMinute) * 10);

                        client.Character.TrainingTime = (uint)Time;
                        packet.TrainingTime = Time;

                        client.Send(packet);
                        break;
                    }
                //Request to enter TG
                case 1:
                    {
                        if (client.Character.HeavenBlessExpires > DateTime.Now && client.MapID != 1036 && client.MapID != 1039)
                        {

                            long Time = Math.Min(900, ((DateTime.Now.Ticks - client.LoginTime) / TimeSpan.TicksPerMinute) * 10);

                            client.Character.TrainingTime = Math.Min(900, client.Character.TrainingTime += (uint)Time);

                            client.Character.OfflineTGEntered = DateTime.Now;
                            client.Send(packet);

                        }
                        else
                            client.SendMessage("You may not enter Offline Training Grounds.", ChatType.System);

                        break;
                    }
                //Requesting Exp Rewards
                case 3:
                    {
                        client.Send(OfflineInfoPacket.Create(client));
                        break;
                    }
                //Claiming Rewards
                case 4:
                    {
                        uint Time = (uint)Math.Min(client.Character.TrainingTime, DateTime.Now.Subtract(client.Character.OfflineTGEntered).TotalMinutes);

                        client.GainExpBall(Time);

                        client.Character.TrainingTime = Math.Max(0, client.Character.TrainingTime -= Time);

                        client.Character.OfflineTGEntered = DateTime.MinValue;
                        client.ChangeMap((ushort)client.Character.Map, client.Character.X, client.Character.Y);

                        break;
                    }
            }

        }
        #endregion

        #region Broadcast
        public static void Process_Broadcast(Player client, BroadcastPacket packet)
        {
            switch (packet.Subtype)
            {

                case 3://Send broadcast
                    if (client.Level < 50)
                        return;
                    if (client.CP < 5)
                        return;

                    string Message;
                    packet.StringPacker.GetString(0, out Message);
                    if (Message == null)
                        return;
                    TalkPacket broadcast = new TalkPacket(Message);
                    broadcast.Type = ChatType.Broadcast;
                    broadcast.Speaker = client.Name;
                    client.CP -= 5;
                    PlayerManager.Broadcasts.Add(broadcast);
                    break;

            }
        }
        #endregion

        #endregion


    }
    #endregion
}
