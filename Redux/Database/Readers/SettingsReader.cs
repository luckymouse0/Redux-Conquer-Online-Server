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
    public static class SettingsReader
    {
        public static void Read()
        {
            IniFile Ini = new IniFile("./" + Environment.MachineName + ".ini");            
            try
            {
                Constants.SERVER_NAME = Ini.ReadString("GENERAL", "SERVER_NAME", "Redux_Beta");
                Constants.GAME_IP = Ini.ReadString("GENERAL", "GAME_IP", string.Empty);
                Constants.GAME_PORT = Ini.ReadInteger("GENERAL", "GAME_PORT", 5816);
                Constants.LOGIN_PORT = Ini.ReadInteger("GENERAL", "LOGIN_PORT", 9958);
            }
            catch
            {
                Console.WriteLine("Make sure there is a corrent inialization file.");
                Console.WriteLine("This file should be in your debug folder.");
                Console.WriteLine("Check whether all values are in the correct format.");
                Console.WriteLine("Check whether the file is called " + Environment.MachineName + ".ini.");
                Console.WriteLine("SERVER TERMINATED! PRESS ENTER TO CLOSE THE SERVER.");
                Console.ReadLine();
                Environment.Exit(0);
            }

        }
    }
}
