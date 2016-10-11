using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Redux.Database;
using Redux.Database.Repositories;
namespace Redux
{
    public static class ItemTypeReader
    {
        public static void Read()
        {
            StreamReader reader = new StreamReader(File.Open("ini/itemtype.txt", FileMode.Open));
            int count = int.Parse(reader.ReadLine().Split('=')[1]);
            for (var i = 0; i < count -1; i++)
            {
                string[] parts = reader.ReadLine().Replace("'", "").Split(' ');
                using (var session = NHibernateHelper.OpenSession())
                {

                    var query = "INSERT INTO itemtype VALUES ('";
                    foreach (string part in parts)
                        query += part + "','";
                    if (query.EndsWith("','','"))
                        query = query.Substring(0, query.Length - 5);
                    if (query.EndsWith("','"))
                        query = query.Substring(0, query.Length - 2);
                    query += ")";

                    var t = session.CreateSQLQuery(query);
                    t.ExecuteUpdate();
                }

            }            

            reader.Close();
        }
    }
}
