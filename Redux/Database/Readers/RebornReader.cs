using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Redux.Database.Readers
{
    public static class RebornReader
    {
        public static void Read()
        {
            var reader = new StreamReader(File.Open("reborns.txt", FileMode.Open));
            var lines = reader.ReadToEnd().Replace("\r", "").Split('\n');

            reader.Close();

            var writer = new StreamWriter(File.Create("dump.txt"));
            foreach (var line in lines)
            {
                var parts = line.Split('\t');
                if (parts.Length < 3)
                    continue;
                string text = "INSERT into Reborns (RebornPath,LearnType,SkillId) VALUES (";
                text += parts[0] + ",";
                text += parts[1] + ",";
                text += parts[2] + ");";
                writer.WriteLine(text);
            }
            writer.Close();
        }
    }
}
