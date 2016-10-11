using Redux.Enum;

namespace Redux.Database.Domain
{
    public class DbShopItem
    {
        public virtual uint UID { get; set; }
        public virtual ushort ShopID { get; set; }
        public virtual uint ItemID { get; set; }
        public virtual CurrencyType CurrencyType { get; set; }
    }
}