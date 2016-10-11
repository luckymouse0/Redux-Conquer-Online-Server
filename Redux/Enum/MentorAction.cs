using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
    public enum MentorAction
    {
        RequestApprentice = 1,
        RequestMentor = 2,
            LeaveMentor = 3,
            ExpellApprentice = 4,
            AcceptRequestApprentice = 8,
            AcceptRequestMentor = 9,
            DumpApprentice = 18,
            DumpMentor = 19
    }
}
