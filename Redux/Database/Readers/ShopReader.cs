using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Redux.Database;
using Redux.Database.Repositories;

namespace Redux.Database.Readers
{
    public static class ShopReader
    {
        public static void Read()
        {
            StreamReader reader = new StreamReader(File.Open("ini/Shop.dat", FileMode.Open));
            int shopID = 0, shopCurrency = 0;
            int count = 0;
            foreach (string line in reader.ReadToEnd().Replace("\r", "").Split('\n'))
            {
                var part = line.Split('=');
                if (part[0] == "ID")
                    shopID = int.Parse(part[1]);
                if (part[0] == "MoneyType")
                    shopCurrency = int.Parse(part[1]);
                if (part[0].Contains("Item") && !part[0].Contains("Amount"))
                {
                    using (var session = NHibernateHelper.OpenSession())
                    {
                        var query = "INSERT INTO Shops VALUES ('" + count + "','" + shopID + "','" + part[1] + "','" + shopCurrency + "')";
                        var t = session.CreateSQLQuery(query);
                        t.ExecuteUpdate();
                    }
                    count++;
                }
            }

            reader.Close();
        }
    }
}