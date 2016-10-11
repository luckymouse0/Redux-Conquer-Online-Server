using System;

namespace Redux.Enum
{
    [Flags]
    public enum ClientEffect : uint
    {
        /// <summary>
        /// [0x00000000]
        /// </summary>
        None = 0U,

        /// <summary>
        /// [0x00000001]
        /// </summary>
        Blue = 1U << 0,

        /// <summary>
        /// [0x00000002]
        /// </summary>
        Poison = 1U << 1,

        /// <summary>
        /// [0x00000004]
        /// </summary>
        NoBody = 1U << 2,

        /// <summary>
        /// [0x00000008]
        /// </summary>
        Die = 1U << 3,

        /// <summary>
        /// [0x00000010]
        /// </summary>
        XpStart = 1U << 4,

        /// <summary>
        /// [0x00000020]
        /// </summary>
        Dead = 1U << 5,

        /// <summary>
        /// [0x00000040]
        /// </summary>
        TeamLeader = 1U << 6,

        /// <summary>
        /// [0x00000080]
        /// </summary>
        Accuracy = 1U << 7,

        /// <summary>
        /// [0x00000100]
        /// </summary>
        Shield = 1U << 8,

        /// <summary>
        /// [0x00000200]
        /// </summary>
        Stigma = 1U << 9,

        /// <summary>
        /// [0x00000400]
        /// </summary>
        Ghost = 1U << 10,

        /// <summary>
        /// [0x00000800]
        /// </summary>
        Fade = 1U << 11,

        /// <summary>
        /// [0x00001000]
        /// </summary>
        Unknown12 = 1U << 12,

        /// <summary>
        /// [0x00002000]
        /// </summary>
        Unknown13 = 1U << 13,

        /// <summary>
        /// [0x00004000]
        /// </summary>
        Red = 1U << 14,

        /// <summary>
        /// [0x00008000]
        /// </summary>
        Black = 1U << 15,

        /// <summary>
        /// [0x00010000]
        /// </summary>
        Unknown16 = 1U << 16,

        /// <summary>
        /// [0x00020000]
        /// </summary>
        Reflect = 1U << 17,

        /// <summary>
        /// [0x00040000]
        /// </summary>
        Superman = 1U << 18,

        /// <summary>
        /// [0x00080000]
        /// </summary>
        Unknown19 = 1U << 19,

        /// <summary>
        /// [0x00100000]
        /// </summary>
        Unknown20 = 1U << 20,

        /// <summary>
        /// [0x00200000]
        /// </summary>
        Unknown21 = 1U << 21,

        /// <summary>
        /// [0x00400000]
        /// </summary>
        Invisible = 1U << 22,

        /// <summary>
        /// [0x00800000]
        /// </summary>
        Cyclone = 1U << 23,

        /// <summary>
        /// [0x01000000]
        /// </summary>
        Unknown24 = 1U << 24,

        /// <summary>
        /// [0x02000000]
        /// </summary>
        Unknown25 = 1U << 25,

        /// <summary>
        /// [0x04000000]
        /// </summary>
        Dodge = 1U << 26,

        /// <summary>
        /// [0x08000000]
        /// </summary>
        Fly = 1U << 27,

        /// <summary>
        /// [0x10000000]
        /// </summary>
        Intensify = 1U << 28,

        /// <summary>
        /// [0x20000000]
        /// </summary>
        Unknown29 = 1U << 29,

        /// <summary>
        /// [0x40000000]
        /// </summary>
        LuckDiffuse = 1U << 30,

        /// <summary>
        /// [0x80000000]
        /// </summary>
        LuckAbsorb = 1U << 31,

        /// <summary>
        /// [0xffffffff]
        /// </summary>
        All = uint.MaxValue
    }
}