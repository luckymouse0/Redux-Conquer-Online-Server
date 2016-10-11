using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
    public enum AssociateType : sbyte
    {
        Unknown = 0,
        Friend = 1,
        Enemy = 2,
        EnemyOf = 3,
        TradePartner = 3,
    }
}