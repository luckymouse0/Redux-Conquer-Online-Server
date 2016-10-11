using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
    public enum ReportStatus
    {
        /// <summary>
        /// Bug report has not yet been read
        /// </summary>
        Unread = 0,

        /// <summary>
        /// Bug has been read and is being investigated by a developer (in progress)
        /// </summary>
        Read = 1,

        /// <summary>
        /// Bug has been corrected in working folder
        /// </summary>
        Fixed = 2,

        /// <summary>
        /// Bug has been corrected in live server
        /// </summary>
        Deployed = 3,
    }
}
