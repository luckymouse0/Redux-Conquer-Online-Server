using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Redux
{
    public partial class ConfigCreatorForm : Form
    {
        public ConfigCreatorForm()
        {
            InitializeComponent();
            MachineNameTextBox.Text = Environment.MachineName;
        }
        private void WriteConfigButton_Click(object sender, EventArgs e)
        {
            if (IPTextBox.Text.Split('.').Length < 4)
            { MessageBox.Show("Please type a valid IPv4 Address"); return; }
            if (MachineNameTextBox.Text.Length < 2)
            { MessageBox.Show("Machine name must be populated"); return; }
            if (DbNameTextBox.Text.Length < 2)
            { MessageBox.Show("Please type a valid database name"); return; }
            if (DbUserTextBox.Text.Length < 2)
            { MessageBox.Show("Please type a valid database username"); return; }
            if (DbPassTextbox.Text.Length < 2)
            { MessageBox.Show("Please type a valid database password"); return; }
            var writer = new StreamWriter(File.Create(MachineNameTextBox.Text + ".ini"));
            writer.WriteLine("[GENERAL]");
            writer.WriteLine("SERVER_NAME=" + ServerNameTextBox.Text);
            writer.WriteLine("GAME_IP=" + IPTextBox.Text);
            writer.WriteLine("LOGIN_PORT=9958");
            writer.WriteLine("GAME_PORT=5816");
            writer.Close();

            writer = new StreamWriter(File.Create(MachineNameTextBox.Text + ".cfg.xml"));
            writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            writer.WriteLine("<hibernate-configuration  xmlns=\"urn:nhibernate-configuration-2.2\" >");
            writer.WriteLine("  <session-factory name=\"NHibernateProject\">");
            writer.WriteLine("    <property name=\"connection.driver_class\">NHibernate.Driver.MySqlDataDriver</property>");
            writer.WriteLine("    <property name=\"connection.connection_string\">");
            writer.WriteLine("      Database=" + DbNameTextBox.Text + ";Data Source=localhost;User Id=" + DbUserTextBox.Text + ";Password=" + DbPassTextbox.Text);
            writer.WriteLine("    </property>");
            writer.WriteLine("    <property name=\"dialect\">NHibernate.Dialect.MySQLDialect</property>");
            writer.WriteLine("    <property name=\"proxyfactory.factory_class\">NHibernate.ByteCode.LinFu.ProxyFactoryFactory, NHibernate.ByteCode.LinFu</property>");
            writer.WriteLine("  </session-factory>");
            writer.WriteLine("</hibernate-configuration>");
            writer.Close();
            MessageBox.Show("Configuration complete! Server will now start");
            Close();
        }


    }
}
