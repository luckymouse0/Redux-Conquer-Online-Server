using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
    /// <summary>
    /// [uint]
    /// </summary>
    public enum GroundItemAction : ushort
    {
        /// <summary>
        /// [01]
        /// </summary>
        Create = 1,

        /// <summary>
        /// [02]
        /// </summary>
        Delete = 2,

        /// <summary>
        /// [03]
        /// </summary>
        Pick = 3,

        /// <summary>
        /// [10]
        /// </summary>
        CastTrap = 10,

        /// <summary>
        /// [11]
        /// </summary>
        SynchroTrap = 11,

        /// <summary>
        /// [12]
        /// </summary>
        DropTrap = 12
    }
}
