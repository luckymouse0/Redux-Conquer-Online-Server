using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
    public enum TargetType
    {
        TargetPlayer = 0,
        BuffPlayer = 1,
        BuffSelf = 2,
        NoTarget = 4,
        WeaponPassive = 8,
    }
}
