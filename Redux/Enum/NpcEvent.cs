using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
    public enum NpcEvent : ushort
    {
        Activate = 0,
        AddNpc,
        LeaveMap,
        DeleteNpc,
        ChangePosition,
        LayNpc
    }
}
