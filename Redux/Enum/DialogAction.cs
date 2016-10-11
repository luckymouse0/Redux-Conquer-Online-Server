using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
    public enum DialogAction : byte
    {
        Dialog = 1,
        Option = 2,
        Input = 3,
        Avatar = 4,
        Finish = 100,
        DeleteMember = 102
    }
}
