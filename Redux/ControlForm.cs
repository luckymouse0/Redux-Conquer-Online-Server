using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Redux.Login_Server;
using Redux.Game_Server;
using Redux.Database.Readers;

namespace Redux
{
    public partial class ControlForm : Form
    {
        public static ControlForm GUI;
        public static LoginServer Login;
        public static GameServer Game;
        public ControlForm()
        {
            InitializeComponent();             
            Database.ServerDatabase.InitializeSql();

            GUI = this;
            CheckForIllegalCrossThreadCalls = false;

            foreach(var sob in Database.ServerDatabase.Context.SOB.GetSOBByMap(1039))
            {
                if (sob.Mesh / 10 % 3 == 0)
                {
                    sob.Level = (byte)(20 + (sob.Mesh - 427) / 30 * 5);
                    Database.ServerDatabase.Context.SOB.AddOrUpdate(sob);
                }
                
            }

            SettingsReader.Read();

            //Begin login server
            Login = new LoginServer("AuthServer", Constants.LOGIN_PORT);

            //Begin game server
            Game = new GameServer("GameServer", Constants.GAME_PORT);          
            
            //GUI Update
            Text += " - " +  Constants.SERVER_NAME;
            Console.WriteLine(Constants.SERVER_NAME + " Ready to log in");            
        }

        protected override void OnClosed(EventArgs e)
        {           
            foreach (var user in Managers.PlayerManager.Players.Values)
                user.Save();
            base.OnClosed(e);
            Environment.Exit(-1);
        }
            
        public void SetOnlineCount(int value)
        {
            OnlineCountTextbox.Text = value.ToString();
        }

        private void OffsetNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Common.offset = (int)OffsetNumericUpDown.Value;
        }

        private void ValueNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Common.value = (int)ValueNumericUpDown.Value;
        }
    }
}
