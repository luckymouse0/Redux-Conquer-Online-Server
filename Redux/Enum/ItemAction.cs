using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Enum
{
    public enum ItemAction : uint
    {
        BuyFromNPC = 1,
        SellToNPC = 2,
        RemoveInventory = 3,
        DropItem = 3,
        EquipItem = 4,
        SetEquipItem = 5,
        UnequipItem = 6,
        SplitItem = 7,
        ViewWarehouse = 9,
        WarehouseDeposit = 10,
        WarehouseWithdraw = 11,
        DropMoney = 12,
        RepairItem = 14,
        DragonBallUpgrade = 19,
        MeteorUpgrade = 20,
        BoothQuery,
        BoothAdd,
        BoothDelete,
        BoothBuy,
        Ping = 27,
        Enchant = 28,
        BoothAddCP
    }
}
