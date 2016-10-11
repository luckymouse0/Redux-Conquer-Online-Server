using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
    [Flags]
    public enum GuildDonationFlags : uint
    {
        None = 0,
        Silver = 1u << 0,
        CP = 1u << 1,
        Guide = 1u << 2,
        Pk = 1u << 3,
        Arsenal = 1u << 4,
        Unknown5 = 1u << 5,
        Unknown6 = 1u << 6,
        Unknown7 = 1u << 7,
        Unknown8 = 1u << 8,
        Unknown9 = 1u << 9,
        Unknown10 = 1u << 10,
        Unknown11 = 1u << 11,
        Unknown12 = 1u << 12,
        Unknown13 = 1u << 13,
        Unknown14 = 1u << 14,
        Unknown15 = 1u << 15,
        Unknown16 = 1u << 16,
        Unknown17 = 1u << 17,
        Unknown18 = 1u << 18,
        Unknown19 = 1u << 19,
        Unknown20 = 1u << 20,
        Unknown21 = 1u << 21,
        Unknown22 = 1u << 22,
        Unknown23 = 1u << 23,
        Unknown24 = 1u << 24,
        Unknown25 = 1u << 25,
        Unknown26 = 1u << 26,
        Unknown27 = 1u << 27,
        Unknown28 = 1u << 28,
        Unknown29 = 1u << 29,
        Unknown30 = 1u << 30,
        Unknown31 = 1u << 31,

        AllDonations = Silver | CP | Guide | Pk | Arsenal
    }
}