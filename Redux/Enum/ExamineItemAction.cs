using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
    public enum ExamineItemAction
    {
        /// <summary>
        /// [00]
        /// </summary>
        None = 0,

        /// <summary>
        /// [01]
        /// </summary>
        Booth,

        /// <summary>
        /// [02]
        /// </summary>
        Equipment,

        /// <summary>
        /// [03]
        /// </summary>
        BoothEMoney,

        /// <summary>
        /// [04]
        /// </summary>
        OtherPlayerEquipment
    }
}
