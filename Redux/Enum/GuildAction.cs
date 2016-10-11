using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
    public enum GuildAction : uint
    {
        /// <summary>
        /// [00]
        /// </summary>
        None = 0,

        /// <summary>
        /// [01]
        /// </summary>
        ApplyJoin,

        /// <summary>
        /// [02]
        /// </summary>
        InviteJoin,

        /// <summary>
        /// [03]
        /// </summary>
        LeaveSyndicate,

        /// <summary>
        /// [04]
        /// </summary>
        KickoutMember,

        /// <summary>
        /// [05]
        /// </summary>
        Unknown5 = 5,

        /// <summary>
        /// [06]
        /// </summary>
        QuerySyndicateName,

        /// <summary>
        /// [07]
        /// </summary>
        SetAlly,

        /// <summary>
        /// [08]
        /// </summary>
        ClearAlly,

        /// <summary>
        /// [09]
        /// </summary>
        SetEnemy,

        /// <summary>
        /// [10]
        /// </summary>
        ClearEnemy = 10,

        /// <summary>
        /// [11]
        /// </summary>
        DonateMoney,

        /// <summary>
        /// [12]
        /// </summary>
        QuerySyndicateAttribute,

        /// <summary>
        /// [13]
        /// </summary>
        Unknown13,

        /// <summary>
        /// [14]
        /// </summary>
        SetSyndicate,

        /// <summary>
        /// [15]
        /// </summary>
        Unknown15 = 15,

        /// <summary>
        /// [16]
        /// </summary>
        Unknown16,

        /// <summary>
        /// [17]
        /// </summary>
        SetWhiteSyndicate,

        /// <summary>
        /// [18]
        /// </summary>
        SetBlackSyndicate,

        /// <summary>
        /// [19]
        /// </summary>
        DestroySyndicate,

        /// <summary>
        /// [20]
        /// </summary>
        DonateEMoney = 20,

        /// <summary>
        /// [23]
        /// </summary>
        Unknown23 = 23,

        /// <summary>
        /// [24]
        /// </summary>
        SetRequirement = 24,

        /// <summary>
        /// [27]
        /// </summary>
        SetAnnounce = 27,

        /// <summary>
        /// [28]
        /// </summary>
        PromoteMember = 28,

        /// <summary>
        /// [30]
        /// </summary>
        DischargeMember = 30,

        /// <summary>
        /// [37]
        /// </summary>
        PromoteInfo = 37,

        /// <summary>
        /// [38]
        /// </summary>
        Unknown38 = 38,

        /// <summary>
        /// [39]
        /// </summary>
        Unknown39 = 39,

        /// <summary>
        /// [41]
        /// </summary>
        Unknown41 = 41,

        /// <summary>
        /// [44]
        /// </summary>
        Unknown44 = 44
    }
}