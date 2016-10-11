using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Redux.Database;
using Redux.Database.Repositories;
using Redux.Database.Domain;
namespace Redux.Database.Readers
{
    public static class StatReader
    {
        public static void Read()
        {
            StreamReader reader = new StreamReader(File.Open("ini/stats.ini", FileMode.Open));

            foreach (string line in reader.ReadToEnd().Replace("/r", "").Split('\n'))
            {
                var stat = new DbStat();
                if (line.Contains("Archer"))
                    stat.ProfessionType = 40;
                else if (line.Contains("Trojan"))
                    stat.ProfessionType = 10;
                else if (line.Contains("Taoist"))
                    stat.ProfessionType = 100;
                else if (line.Contains("Warrior"))
                    stat.ProfessionType = 20;

                var level = line.Substring(line.IndexOf("[")+1, line.IndexOf("]") - line.IndexOf("[") -1);
                stat.Level = byte.Parse(level);

                var parts = line.Substring(line.IndexOf("=") +1).Split(',');
                stat.Vitality = ushort.Parse(parts[0]);
                stat.Strength = ushort.Parse(parts[1]);
                stat.Agility = ushort.Parse(parts[2]);
                stat.Spirit = ushort.Parse(parts[3]);

                ServerDatabase.Context.Stats.Add(stat);
            }

            reader.Close();
        }
    }
}
