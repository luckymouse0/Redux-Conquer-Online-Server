using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
    public enum WarehouseType:byte
    {
        /// <summary>
        /// [00]
        /// </summary>
        None = 0,

        /// <summary>
        /// [10]
        /// </summary>
        Storage = 10,

        /// <summary>
        /// [20]
        /// </summary>
        Trunk = 20,

        /// <summary>
        /// [30]
        /// </summary>
        Chest = 30
    }
}
