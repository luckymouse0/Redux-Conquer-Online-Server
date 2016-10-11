using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
    public enum UpdateType : uint
    {
        None = uint.MaxValue,
        Life = 0,
        MaxLife,
        Mana,
        MaxMana,
        Money,
        Experience = 5,
        Pk = 6,
        Profession = 7,
        SizeAdd = 8,
        Stamina = 9,
        MoneySaved = 10,
        AdditionalPoint,
        Lookface,
        Level,
        Spirit,
        Vitality = 15,
        Strength,
        Agility,
        HeavenBlessing =18,
        DoubleExpTime =19,
        GuildDonation = 20,
        CurseTime = 21, 
        AddTime = 22,
        Reborn = 23,
        UserStatus = 25,
        StatusEffects,
        Hair,
        Xp,
        LuckyTime,
        CP,
        OnlineTraining = 32,
        ExtraBP = 37,
        Merchant = 39,
        Quiz = 40,
        EnlightPoints = 41,
        BonusBP = 44,
        BoundCp = 45,
        AzureShield = 49,
    }
}
