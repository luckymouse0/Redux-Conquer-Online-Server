using Redux.Database.Domain;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace Redux.Database.Repositories
{
    public class ShopItemRepository : Repository<uint, DbShopItem>
    {
        public DbShopItem GetShopItem(ushort _shopID, uint _itemID)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return
                    session.CreateCriteria<DbShopItem>()
                    .Add(Restrictions.Eq("ShopID", _shopID))
                    .Add(Restrictions.Eq("ItemID", _itemID))
                    .UniqueResult<DbShopItem>();
            }
        }
    }
}

