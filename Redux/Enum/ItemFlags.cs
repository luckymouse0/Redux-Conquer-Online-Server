using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
    [Flags]
    public enum ItemFlags : byte
    {
        None = 0,
        TradeDisable = 1 << 0,
        StorageDisable = 1 << 1,
        DropHint = 1 << 2,
        SellHint = 1 << 3,
        NeverDropWhenDead = 1 << 4,
        SellDisable = 1 << 5
    }
}
