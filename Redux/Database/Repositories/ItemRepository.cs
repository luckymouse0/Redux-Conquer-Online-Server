using System.Collections;
using System;
using System.Collections.Generic;
using Redux.Database.Domain;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using Redux.Structures;

namespace Redux.Database.Repositories
{
    public class ItemRepository : Repository<uint, DbItem>
    {
        public DbItem CreateNewItem(uint owner, ConquerItem info)
        {
            var item = new DbItem();
            item.UniqueID = info.UniqueID;
            item.StaticID = info.StaticID;
            item.Owner = owner;
            item.Plus = info.Plus;
            item.Durability = info.Durability;
            item.Location = info.Location;
            item.Color = info.Color;
            item.Bless = info.Bless;
            item.Enchant = info.Enchant;
            item.Gem1 = info.Gem1;
            item.Gem2 = info.Gem2;
            item.Effect = info.Effect;
            item.Locked = info.Locked;
            ServerDatabase.Context.Items.AddOrUpdate(item);
            return item;
        }
        public ItemRepository()
        {
            PopulateItemGenerator();
        }
        public void PopulateItemGenerator()
        {
            uint item = 0;
            try
            {
                using (var session = NHibernateHelper.OpenSession())
                {
                    item = session
                        .CreateCriteria<DbItem>()
                        .AddOrder(new Order("UniqueID", false))
                        .List<DbItem>()[0].UniqueID; 
                }
            }
            catch (Exception e) { Console.WriteLine("Could not initialize item generator seed. " + e); }
            Common.ItemGenerator = new Utility.ThreadSafeCounter((int)item, int.MaxValue);
        }
        public IList<DbItem> GetItemsByPlayer(uint playerID)
        {
            
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbItem>()
                    .Add(Restrictions.Eq("Owner", playerID))
                    .List<DbItem>();
            }
        }        
    }
}

