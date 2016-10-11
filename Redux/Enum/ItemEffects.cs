using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
    public enum ItemEffects : byte
    {
        //Status
        Poison = 10,
        Freeze = 210,
        //Arrows
        Cruel = 11,
        Tracing = 12,
        Fatal = 13,
        Icy = 15,
        Freezing = 16,
        Magic = 17,
        Deft = 20,
        Thunder = 21,
        Bomb = 22,
        Mine = 23,
        Fire = 100,
        Blaze = 101,
        Rocket = 102,
        Powder = 103,
        //Passive skills
        Heal = 150,
        Mana = 151,
        Shield = 152
    }
}
