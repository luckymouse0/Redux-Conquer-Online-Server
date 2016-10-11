using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Redux.Database;
using Redux.Database.Domain;
using Redux.Game_Server;
using Redux.Packets.Game;
using Redux.Enum;

namespace Redux.Managers
{
    public class AssociateManager
    {
        public ConcurrentDictionary<uint, Associate> Friends;
        public ConcurrentDictionary<uint, Associate> Enemies;
        public ConcurrentDictionary<uint, Associate> EnemyOf;

        public Player Owner;

        public uint FriendRequestUID;

        public AssociateManager(Player client)
        {
            Owner = client;
            Friends = new ConcurrentDictionary<uint, Associate>();
            Enemies = new ConcurrentDictionary<uint, Associate>();
            EnemyOf = new ConcurrentDictionary<uint, Associate>();
            foreach (DbAssociate rel in ServerDatabase.Context.Associates.GetUserAssociates(Owner.UID))
            {
                Associate associate = new Associate(rel);
                if (associate.Type == AssociateType.Friend)
                    Friends.TryAdd(associate.UID, associate);
                else if (associate.Type == AssociateType.Enemy)
                    Enemies.TryAdd(associate.UID, associate);
                else if (associate.Type == AssociateType.EnemyOf)
                    EnemyOf.TryAdd(associate.UID, associate);
            }
            InformAssociates(true);
        }


        public void InformAssociates(bool online)
        {
            #region Friends
            foreach (Associate friend in Friends.Values)
            {
                if (online)
                {
                    Owner.Send(AssociatePacket.Create(friend, AssociateAction.AddFriend));
                    if (friend.IsOnline)
                        friend.Send(AssociatePacket.Create(Owner.UID, AssociateAction.SetOnlineFriend, true, Owner.Name));
                }
                else if (friend.IsOnline && friend.Type == AssociateType.Friend)
                    friend.Send(AssociatePacket.Create(Owner.UID, AssociateAction.SetOfflineFriend, false, Owner.Name));
            }
            #endregion
            #region Enemies
            foreach (Associate enemy in Enemies.Values)
            {
                if (online)
                    Owner.Send(AssociatePacket.Create(enemy.UID, AssociateAction.AddEnemy, true, enemy.Name));
            }
            #endregion
            #region EnemyOf
            foreach (var enemyof in EnemyOf.Values)
            {
                if (online)
                    enemyof.Send(AssociatePacket.Create(Owner.UID, AssociateAction.SetOnlineEnemy, true, Owner.Name));
                else
                    enemyof.Send(AssociatePacket.Create(Owner.UID, AssociateAction.SetOfflineEnemy, false, Owner.Name));
            }
            #endregion
        }

        #region Friend Functions
        public void AddFriend(Player friend)
        {
            if (friend == null)
                return;
            if (HasFriend(friend.UID))
                return;
            DbAssociate assoc = new DbAssociate();
            assoc.AssociateID = friend.UID;
            assoc.Name = friend.Name;
            assoc.Type = (byte)AssociateType.Friend;
            assoc.UID = Owner.UID;
            ServerDatabase.Context.Associates.Add(assoc);
            var associate = new Associate(assoc);
            Friends.TryAdd(friend.UID, associate);
            Owner.Send(AssociatePacket.Create(associate, AssociateAction.AddFriend));
        }
        public void RemoveFriend(uint friendid)
        {
            if (!HasFriend(friendid))
                return;
            var associate = Friends[friendid];
            Owner.Send(AssociatePacket.Create(friendid, AssociateAction.RemoveFriend, true, associate.Name));
            associate.Send(AssociatePacket.Create(Owner.UID, AssociateAction.RemoveFriend, true, Owner.Name));
            ServerDatabase.Context.Associates.Remove(Owner.UID, friendid, AssociateType.Friend);
            ServerDatabase.Context.Associates.Remove(friendid, Owner.UID, AssociateType.Friend);
        }    
        public bool HasFriend(uint FriendID)
        {
            return (Friends.ContainsKey(FriendID));
        }
        public Player GetFriend(uint friendid)
        {
            if (!HasFriend(friendid))
                return null;
            return PlayerManager.GetUser(friendid);
        }
        #endregion
        #region Enemy Functions
        public void AddEnemy(Player enemy)
        {
            if (enemy == null)
                return;
            if (HasEnemy(enemy.UID))
                return;
            DbAssociate assoc = new DbAssociate();
            assoc.AssociateID = enemy.UID;
            assoc.Name = enemy.Name;
            assoc.Type = (byte)AssociateType.Enemy;
            assoc.UID = Owner.UID;
            ServerDatabase.Context.Associates.Add(assoc);
            var associate = new Associate(assoc);
            Enemies.TryAdd(enemy.UID, associate);
            Owner.Send(AssociatePacket.Create(associate, AssociateAction.AddEnemy));
        }
        public void RemoveEnemy(uint enemyid)
        {
            if (!HasEnemy(enemyid))
                return;
            var associate = Enemies[enemyid];
            Owner.Send(AssociatePacket.Create(enemyid, AssociateAction.RemoveEnemy, true, associate.Name));
            ServerDatabase.Context.Associates.Remove(Owner.UID, enemyid, AssociateType.Enemy);
            //ServerDatabase.Context.Associates.Remove(enemyid, Owner.UID, AssociateType.EnemyOf);
        }
        public bool HasEnemy(uint EnemyID)
        {
            return Enemies.ContainsKey(EnemyID);
        }
        public Player GetEnemy(uint enemyid)
        {
            if (!HasEnemy(enemyid))
                return null;
            return PlayerManager.GetUser(enemyid);
        }
        #endregion
        #region EnemyOf Functions
        public void AddEnemyOf(Player enemyof)
        {
            if (enemyof == null)
                return;
            if (HasEnemyOf(enemyof.UID))
                return;
            DbAssociate assoc = new DbAssociate();
            assoc.AssociateID = enemyof.UID;
            assoc.Name = enemyof.Name;
            assoc.Type = (byte)AssociateType.EnemyOf;
            assoc.UID = Owner.UID;
            ServerDatabase.Context.Associates.Add(assoc);
            var associate = new Associate(assoc);
            EnemyOf.TryAdd(enemyof.UID, associate);
        }

        public bool HasEnemyOf(uint EnemyID)
        {
            return EnemyOf.ContainsKey(EnemyID);
        }
        #endregion

        public void Close()
        {
            InformAssociates(false);
        }
    }
}