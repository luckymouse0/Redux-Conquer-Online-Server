using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Database.Domain;
namespace Redux.Managers
{
    public static class DropManager
    {
        public static ushort[] HelmetTypes = new ushort[] { 111, 113, 114, 117, 118 };
        public static ushort[] NecklaceTypes = new ushort[] { 120, 121 };
        public static ushort[] RingTypes = new ushort[] { 150, 151, 152 };
        public static ushort[] OneHandWeaponTypes = new ushort[] { 410, 420, 421, 430, 440, 450, 460, 480, 490 };
        public static ushort[] TwoHandWeaponTypes = new ushort[] { 500, 510, 530, 540, 560,561, 562, 580 };
        public static ushort[] ArmorTypes = new ushort[] { 130, 131, 132, 133, 134 };
        public static ushort BootType = 160;
        public static ushort ShieldType = 900;

        static ushort MaxHelmetLevel = 10, MaxNecklaceLevel = 31, MaxRingLevel = 24, MaxWeaponLevel = 33, MaxArmorLevel = 10, MaxBootLevel = 24, MaxShieldLevel = 10;

        public static ushort QUALITY_NORMAL = 5,
            QUALITY_REFINED = 6,
            QUALITY_UNIQUE = 7,
            QUALITY_ELITE = 8,
            QUALITY_SUPER = 9;


        public static uint GenerateDropID(byte _level)
        {
            if (_level > 125)
                _level = 125;
            ushort itemLevel = 0;
            var dropType = Common.Random.Next(8);
            var itemType = 160;
            switch (dropType)
            {
                case 0:
                    itemType = HelmetTypes[Common.Random.Next(HelmetTypes.Length)];
                    itemLevel = (ushort)Common.Random.Next(_level / 20, Math.Min(MaxHelmetLevel, _level / 11 + 1));
                    break;
                case 1:
                    itemType = NecklaceTypes[Common.Random.Next(NecklaceTypes.Length)];
                    itemLevel = (ushort)Common.Random.Next(_level / 15, Math.Min(MaxNecklaceLevel, _level / 5 + 1));
                    break;
                case 2:
                    itemType = RingTypes[Common.Random.Next(RingTypes.Length)];
                    itemLevel = (ushort)Common.Random.Next(_level / 15, Math.Min(MaxRingLevel, _level / 5 + 1));
                    break;
                case 3:
                    itemType = OneHandWeaponTypes[Common.Random.Next(OneHandWeaponTypes.Length)];
                    itemLevel = (ushort)Common.Random.Next(_level / 10, Math.Min(MaxWeaponLevel, _level / 4 + 1));
                    break;
                case 4:
                    itemType = TwoHandWeaponTypes[Common.Random.Next(TwoHandWeaponTypes.Length)];
                    itemLevel = (ushort)Common.Random.Next(_level / 10, Math.Min(MaxWeaponLevel, _level / 4 + 1));
                    break;
                case 5:
                    itemType = ArmorTypes[Common.Random.Next(ArmorTypes.Length)];
                    itemLevel = (ushort)Common.Random.Next(_level / 20, Math.Min(MaxArmorLevel, _level / 11 + 1));
                    break;
                case 6:
                    itemType = ShieldType;
                    itemLevel = (ushort)Common.Random.Next(_level / 20, Math.Min(MaxShieldLevel, _level / 11 + 1));
                    break;
                case 7:
                    itemType = BootType;
                    itemLevel = (ushort)Common.Random.Next(_level / 15, Math.Min(MaxBootLevel, _level / 5 + 1));
                    break;
            }

            var quality = Common.Random.Next(3,6);//rolls 3, 4 or 5

            if (Common.PercentSuccess(Constants.CHANCE_SUPER))
                quality = QUALITY_SUPER;
            else if (Common.PercentSuccess(Constants.CHANCE_ELITE))
                quality = QUALITY_ELITE;
            else if (Common.PercentSuccess(Constants.CHANCE_UNIQUE))
                quality = QUALITY_UNIQUE;
            else if (Common.PercentSuccess(Constants.CHANCE_REFINED))
                quality = QUALITY_REFINED;

            return (uint)((itemType * 1000) + (itemLevel * 10) + quality);          
        }
    }
}
