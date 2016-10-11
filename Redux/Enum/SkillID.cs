using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
    public enum SkillID : ushort
    {
        //Mana
        Thunder = 1000,
        Fire = 1001,
        Tornado = 1002,
        Cure = 1005,
        HealingRain = 1055,
        Invisiblity = 1075,
        StarOfAccuracy = 1085,
        MagicShield = 1090,
        Stigma = 1095,
        Pray = 1100,
        Restore = 1105,
        //... etc ...//
        Nectar = 1170,
        AdvancedCure = 1175,
        FireMeteor = 1180,
        FireCircle = 1120,
        FireRing = 1150,
        Bomb = 1160,
        FireOfHell = 1165,
        
        //XP
        Lightning = 1010,
        Accuracy = 1015,
        Shield = 1020,
        Superman = 1025,
        Roar = 1040,
        ///<summary>XP Revive</summary>
        Revive = 1050,
        Cyclone = 1110,
        Volcano = 1125,
        Robot = 1270,
        WaterElf = 1280,
        FlyingMoon = 1320,
        NightDevil = 1360,
        SpeedLightning = 5001,

        //Stamina
        FastBlade = 1045,
        ScentSword = 1046,
        Summon = 1060,
        SpiritHealing = 1190,
        Meditation = 1195,
        Hercules = 1115,
        DivineHare = 1350,


        //Weapon Skills
        WideStrike = 1250,
        SpeedGun = 1260,
        Penetration = 1290,
        Halt = 1300,
        Snow = 5010,
        StrandedMonster = 5020,
        Phoenix = 5030,
        Boom = 5040,
        Boreas = 5050,
        Seizer = 7000,
        Earthquake = 7010,
        Rage = 7020,
        Celestial = 7030,
        Roamer = 7040,
        Dash = 1051,

        //Dances
        Dance2 = 1380,
        Dance3 = 1385, 
        Dance4 = 1390,
        Dance5 = 1395,
        Dance6 = 1400,
        Dance7 = 1405,
        Dance8 = 1410,

        //Archer
        RapidFire = 8000,
        Scatter = 8001,
        Fly = 8002,
        AdvancedFly = 8003,
        Intensify = 9000,
        ArrowRain = 8030,
    }
}