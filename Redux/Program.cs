using System;
using System.Windows.Forms;
using System.IO;

namespace Redux
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if(!File.Exists(Environment.MachineName+".ini"))
                Application.Run(new ConfigCreatorForm());
            Application.Run(new ControlForm());
        }
    }
}
