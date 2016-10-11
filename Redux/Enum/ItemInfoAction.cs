using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
    public enum ItemInfoAction : byte
    {
        None = 0,
        AddItem,
        Trade,
        Update,
        OtherPlayerEquipment,
        Auction
    }
}
