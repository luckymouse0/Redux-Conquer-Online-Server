using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
	public enum ItemLocation:byte
    {
        Inventory = 0,
        Helmet = 1,
        Necklace = 2,
        Armor = 3,
        WeaponR = 4,
        WeaponL = 5,
        Ring = 6,
        Talisman = 7,
        Boots = 8,
        Garment = 9,
        WAREHOUSE_START = 40,
    }
}
