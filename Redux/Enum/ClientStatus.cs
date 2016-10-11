using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
    public enum ClientStatus
    {
        None,
        PkProtect,
        BonusHitrate = 7,
        BonusDefense,        
        BonusAttack,

        Superman = 18,

        Invisible = 22,
        BonusDodge = 26,
        Flying = 27,
        Intensify = 28,

        ReviveTimeout,
        ReviveProtection,

        TransformationTimeout
    }
}