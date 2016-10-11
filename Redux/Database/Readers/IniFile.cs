using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

namespace Redux.Database.Readers
{
    public class IniFile
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public string FileName;
        private bool FileExists;

        public IniFile(string Target)
        {
            FileExists = File.Exists(Target);
            FileName = Target;
        }

        public string ReadString(string Section, string Key, string Default)
        {
            if (!FileExists)
                return Default;
            StringBuilder Builder = new StringBuilder(255);
            GetPrivateProfileString(Section, Key, Default, Builder, 255, this.FileName);
            return Builder.ToString();
        }

        public int ReadInteger(string Section, string Key, int Default)
        {
            if (!FileExists)
                return Default;
            return Convert.ToInt32(ReadString(Section, Key, Default.ToString()));
        }

        public bool ReadBool(string Section, string Key, bool Default)
        {
            if (!FileExists)
                return Default;
            return Convert.ToBoolean(ReadString(Section, Key, Convert.ToString(Default)));
        }

        public void WriteString(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.FileName);
        }

        public void WriteInteger(string Section, string Key, int Value)
        {
            WriteString(Section, Key, Value.ToString());
        }

        public void WriteBool(string Section, string Key, bool Value)
        {
            WriteString(Section, Key, Convert.ToString(Value));
        }
    }

}
