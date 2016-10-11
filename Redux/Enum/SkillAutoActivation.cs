using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
    public enum SkillAutoActivation
    {
        None = 0,
        Kill = 1 <<0,
        Forever = 2 << 1,
        Random = 1 << 2,
        Hidden = 1 << 3,
        ExperienceDisable = 1 <<4,
    }
}
