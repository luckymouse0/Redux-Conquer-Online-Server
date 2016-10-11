using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
    public enum WarehouseAction:byte
    {
        /// <summary>
        /// [00]
        /// </summary>
        ListItems = 0,

        /// <summary>
        /// [01]
        /// </summary>
        AddItem,

        /// <summary>
        /// [02]
        /// </summary>
        RemoveItem,

        /// <summary>
        /// [03]
        /// </summary>
        GetCount,
    }
}
