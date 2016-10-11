using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
    public enum TradeType : byte
    {
        Request = 1,
        Close = 2,
        ShowTable = 3,
        HideTable = 5,
        AddItem = 6,
        SetMoney = 7,
        ShowMoney = 8,
        Accept = 10,
        RemoveItem = 11,
        ShowConquerPoints = 12,
        SetConquerPoints = 13,
        TimeOut = 17,
    }
}