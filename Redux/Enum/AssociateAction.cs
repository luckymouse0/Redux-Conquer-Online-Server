using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
    public enum AssociateAction : byte
    {
        RequestFriend = 10,
        NewFriend = 11,
        SetOnlineFriend = 12,
        SetOfflineFriend = 13,
        RemoveFriend = 14,
        AddFriend = 15,
        SetOnlineEnemy = 16,
        SetOfflineEnemy = 17,
        RemoveEnemy = 18,
        AddEnemy = 19,
    }
}