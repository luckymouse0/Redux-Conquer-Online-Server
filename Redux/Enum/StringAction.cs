using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
    /// <summary>
    /// [byte]
    /// </summary>
    public enum StringAction : byte
    {
        /// <summary>
        /// [00]
        /// </summary>
        None = 0,

        /// <summary>
        /// [01]
        /// </summary>
        Fireworks,

        /// <summary>
        /// [02]
        /// </summary>
        CreateGuild,

        /// <summary>
        /// [03]
        /// </summary>
        Guild,

        /// <summary>
        /// [04]
        /// </summary>
        ChangeTitle,

        /// <summary>
        /// [05]
        /// </summary>
        DeleteRole = 5,

        /// <summary>
        /// [06]
        /// </summary>
        Mate,

        /// <summary>
        /// [07]
        /// </summary>
        QueryNpc,

        /// <summary>
        /// [08]
        /// </summary>
        Wanted,

        /// <summary>
        /// [09]
        /// </summary>
        MapEffect,

        /// <summary>
        /// [10]
        /// </summary>
        RoleEffect = 10,

        /// <summary>
        /// [11]
        /// </summary>
        MemberList,

        /// <summary>
        /// [12]
        /// </summary>
        KickoutGuildMember,

        /// <summary>
        /// [13]
        /// </summary>
        QueryWanted,

        /// <summary>
        /// [14]
        /// </summary>
        QueryPoliceWanted,

        /// <summary>
        /// [15]
        /// </summary>
        PoliceWanted = 15,

        /// <summary>
        /// [16]
        /// </summary>
        QueryMate,

        /// <summary>
        /// [17]
        /// </summary>
        AddDicePlayer,

        /// <summary>
        /// [18]
        /// </summary>
        DeleteDicePlayer,

        /// <summary>
        /// [19]
        /// </summary>
        DiceBonus,

        /// <summary>
        /// [20]
        /// </summary>
        PlayerWave = 20,

        /// <summary>
        /// [21]
        /// </summary>
        SetAlly,

        /// <summary>
        /// [22]
        /// </summary>
        SetEnemy,

        /// <summary>
        /// [26]
        /// </summary>
        WhisperInfo = 26,
    }
}