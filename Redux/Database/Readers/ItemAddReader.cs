using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Redux.Database;
using Redux.Database.Repositories;
namespace Redux.Database.Readers
{
    public static class ItemAddReader
    {
        public static void Read()
        {
            StreamReader reader = new StreamReader(File.Open("ini/itemadd.ini", FileMode.Open));

            foreach (string line in reader.ReadToEnd().Split('\n'))
            {
                if (line.Length < 5)
                    break;
                using (var session = NHibernateHelper.OpenSession())
                {
                    var query = "INSERT INTO itemadd VALUES (";
                    string[] parts = line.Replace("\r","").Split(' ');
                    query += "'" + (int.Parse(parts[0]) *10 + int.Parse(parts[1])).ToString() + "', ";
                    for (int i = 0; i < parts.Length - 1; i++)
                        query += "'" + parts[i] + "', ";
                    query += "'"+parts[parts.Length-1] + "')";
                    var t = session.CreateSQLQuery(query);
                    t.ExecuteUpdate();
                }
            }

            reader.Close();
        }
    }
}
