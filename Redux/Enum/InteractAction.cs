using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
    /// <summary>
    /// [int]
    /// </summary>
    public enum InteractAction
    {
        /// <summary>
        /// [00]
        /// </summary>
        None = 0,

        /// <summary>
        /// [01]
        /// </summary>
        Steal,

        /// <summary>
        /// [02]
        /// </summary>
        Attack,

        /// <summary>
        /// [03]
        /// </summary>
        Heal,

        /// <summary>
        /// [04]
        /// </summary>
        Poison,

        /// <summary>
        /// [05]
        /// </summary>
        Assassinate = 5,

        /// <summary>
        /// [06]
        /// </summary>
        Freeze,

        /// <summary>
        /// [07
        /// </summary>
        Unfreeze,

        /// <summary>
        /// [08]
        /// </summary>
        Court,

        /// <summary>
        /// [09]
        /// </summary>
        Marry,

        /// <summary>
        /// [10]
        /// </summary>
        Divorce = 10,

        /// <summary>
        /// [11]
        /// </summary>
        PresentMoney,

        /// <summary>
        /// [12]
        /// </summary>
        PresentItem,

        /// <summary>
        /// [13]
        /// </summary>
        SendFlowers,

        /// <summary>
        /// [14]
        /// </summary>
        Kill,

        /// <summary>
        /// [15]
        /// </summary>
        JoinGuild = 15,

        /// <summary>
        /// [16]
        /// </summary>
        AcceptGuildMember,

        /// <summary>
        /// [17]
        /// </summary>
        KickoutGuildMember,

        /// <summary>
        /// [18]
        /// </summary>
        PresentPower,

        /// <summary>
        /// [19]
        /// </summary>
        QueryInfo,

        /// <summary>
        /// [20]
        /// </summary>
        RushAttack = 20,

        /// <summary>
        /// [21]
        /// </summary>
        MagicAttack,

        /// <summary>
        /// [22]
        /// </summary>
        AbortMagic,

        /// <summary>
        /// [23]
        /// </summary>
        ReflectWeapon,

        /// <summary>
        /// [24]
        /// </summary>
        Bump,

        /// <summary>
        /// [25]
        /// </summary>
        Shoot = 25,

        /// <summary>
        /// [26]
        /// </summary>
        ReflectMagic,

        /// <summary>
        /// [27]
        /// </summary>
        CutMeat,

        /// <summary>
        /// [28]
        /// </summary>
        Mine,

        /// <summary>
        /// [29]
        /// </summary>
        Quarry,

        /// <summary>
        /// [30]
        /// </summary>
        Chop = 30,

        /// <summary>
        /// [31]
        /// </summary>
        Hustle,

        /// <summary>
        /// [32]
        /// </summary>
        Soul
    }
}
