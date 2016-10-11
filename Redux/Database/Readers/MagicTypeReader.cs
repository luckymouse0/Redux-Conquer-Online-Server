using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Redux.Structures;
using Redux.Database;
using Redux.Database.Repositories;
namespace Redux.Database.Readers
{
    public static class MagicTypeReader
    {
        public static void Read()
        {
            BinaryReader reader = new BinaryReader(File.Open("ini/magictype.dat", FileMode.Open));
            int count = reader.ReadInt32();
            var line = "";
            for (var x = 0; x < count; x++)
            {
                //for (var y = 0; y < 8; y++)
                    line += reader.ReadUInt32().ToString() + " ";
                Console.WriteLine(line);
                line = "";
            }


            reader.Close();
        }
    }
}
