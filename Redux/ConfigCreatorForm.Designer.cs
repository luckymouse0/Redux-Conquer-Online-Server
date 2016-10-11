namespace Redux
{
    partial class ConfigCreatorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.WriteConfigButton = new System.Windows.Forms.Button();
            this.MachineNameLabel = new System.Windows.Forms.Label();
            this.MachineNameTextBox = new System.Windows.Forms.TextBox();
            this.DbNameTextBox = new System.Windows.Forms.TextBox();
            this.IPTextBox = new System.Windows.Forms.TextBox();
            this.IPLabel = new System.Windows.Forms.Label();
            this.DBNameLabel = new System.Windows.Forms.Label();
            this.DBUserLabel = new System.Windows.Forms.Label();
            this.DbUserTextBox = new System.Windows.Forms.TextBox();
            this.DbPassLabel = new System.Windows.Forms.Label();
            this.DbPassTextbox = new System.Windows.Forms.TextBox();
            this.ServerNameTextBox = new System.Windows.Forms.TextBox();
            this.ServerNameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // WriteConfigButton
            // 
            this.WriteConfigButton.Location = new System.Drawing.Point(118, 282);
            this.WriteConfigButton.Name = "WriteConfigButton";
            this.WriteConfigButton.Size = new System.Drawing.Size(173, 50);
            this.WriteConfigButton.TabIndex = 7;
            this.WriteConfigButton.Text = "Write Configuration";
            this.WriteConfigButton.UseVisualStyleBackColor = true;
            this.WriteConfigButton.Click += new System.EventHandler(this.WriteConfigButton_Click);
            // 
            // MachineNameLabel
            // 
            this.MachineNameLabel.AutoSize = true;
            this.MachineNameLabel.Location = new System.Drawing.Point(157, 15);
            this.MachineNameLabel.Name = "MachineNameLabel";
            this.MachineNameLabel.Size = new System.Drawing.Size(113, 18);
            this.MachineNameLabel.TabIndex = 1;
            this.MachineNameLabel.Text = "Machine Name";
            // 
            // MachineNameTextBox
            // 
            this.MachineNameTextBox.Enabled = false;
            this.MachineNameTextBox.Location = new System.Drawing.Point(77, 36);
            this.MachineNameTextBox.Name = "MachineNameTextBox";
            this.MachineNameTextBox.Size = new System.Drawing.Size(273, 26);
            this.MachineNameTextBox.TabIndex = 1;
            // 
            // DbNameTextBox
            // 
            this.DbNameTextBox.Location = new System.Drawing.Point(8, 236);
            this.DbNameTextBox.Name = "DbNameTextBox";
            this.DbNameTextBox.Size = new System.Drawing.Size(129, 26);
            this.DbNameTextBox.TabIndex = 4;
            this.DbNameTextBox.Text = "redux";
            // 
            // IPTextBox
            // 
            this.IPTextBox.Location = new System.Drawing.Point(77, 102);
            this.IPTextBox.Name = "IPTextBox";
            this.IPTextBox.Size = new System.Drawing.Size(273, 26);
            this.IPTextBox.TabIndex = 2;
            // 
            // IPLabel
            // 
            this.IPLabel.AutoSize = true;
            this.IPLabel.Location = new System.Drawing.Point(177, 81);
            this.IPLabel.Name = "IPLabel";
            this.IPLabel.Size = new System.Drawing.Size(72, 18);
            this.IPLabel.TabIndex = 6;
            this.IPLabel.Text = "Server IP";
            // 
            // DBNameLabel
            // 
            this.DBNameLabel.AutoSize = true;
            this.DBNameLabel.Location = new System.Drawing.Point(34, 215);
            this.DBNameLabel.Name = "DBNameLabel";
            this.DBNameLabel.Size = new System.Drawing.Size(77, 18);
            this.DBNameLabel.TabIndex = 8;
            this.DBNameLabel.Text = "DB Name";
            // 
            // DBUserLabel
            // 
            this.DBUserLabel.AutoSize = true;
            this.DBUserLabel.Location = new System.Drawing.Point(160, 215);
            this.DBUserLabel.Name = "DBUserLabel";
            this.DBUserLabel.Size = new System.Drawing.Size(107, 18);
            this.DBUserLabel.TabIndex = 12;
            this.DBUserLabel.Text = "DB Username";
            // 
            // DbUserTextBox
            // 
            this.DbUserTextBox.Location = new System.Drawing.Point(149, 236);
            this.DbUserTextBox.Name = "DbUserTextBox";
            this.DbUserTextBox.Size = new System.Drawing.Size(129, 26);
            this.DbUserTextBox.TabIndex = 5;
            this.DbUserTextBox.Text = "root";
            // 
            // DbPassLabel
            // 
            this.DbPassLabel.AutoSize = true;
            this.DbPassLabel.Location = new System.Drawing.Point(302, 216);
            this.DbPassLabel.Name = "DbPassLabel";
            this.DbPassLabel.Size = new System.Drawing.Size(105, 18);
            this.DbPassLabel.TabIndex = 14;
            this.DbPassLabel.Text = "DB Password";
            // 
            // DbPassTextbox
            // 
            this.DbPassTextbox.Location = new System.Drawing.Point(290, 236);
            this.DbPassTextbox.Name = "DbPassTextbox";
            this.DbPassTextbox.Size = new System.Drawing.Size(129, 26);
            this.DbPassTextbox.TabIndex = 6;
            // 
            // ServerNameTextBox
            // 
            this.ServerNameTextBox.Location = new System.Drawing.Point(77, 168);
            this.ServerNameTextBox.Name = "ServerNameTextBox";
            this.ServerNameTextBox.Size = new System.Drawing.Size(273, 26);
            this.ServerNameTextBox.TabIndex = 3;
            this.ServerNameTextBox.Text = "[Redux]";
            // 
            // ServerNameLabel
            // 
            this.ServerNameLabel.AutoSize = true;
            this.ServerNameLabel.Location = new System.Drawing.Point(163, 147);
            this.ServerNameLabel.Name = "ServerNameLabel";
            this.ServerNameLabel.Size = new System.Drawing.Size(100, 18);
            this.ServerNameLabel.TabIndex = 15;
            this.ServerNameLabel.Text = "Server Name";
            // 
            // ConfigCreatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 347);
            this.Controls.Add(this.ServerNameTextBox);
            this.Controls.Add(this.ServerNameLabel);
            this.Controls.Add(this.DbPassLabel);
            this.Controls.Add(this.DbPassTextbox);
            this.Controls.Add(this.DBUserLabel);
            this.Controls.Add(this.DbUserTextBox);
            this.Controls.Add(this.DBNameLabel);
            this.Controls.Add(this.IPTextBox);
            this.Controls.Add(this.IPLabel);
            this.Controls.Add(this.DbNameTextBox);
            this.Controls.Add(this.MachineNameTextBox);
            this.Controls.Add(this.MachineNameLabel);
            this.Controls.Add(this.WriteConfigButton);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "ConfigCreatorForm";
            this.Text = "[Create Server Configuration]";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button WriteConfigButton;
        private System.Windows.Forms.Label MachineNameLabel;
        private System.Windows.Forms.TextBox MachineNameTextBox;
        private System.Windows.Forms.TextBox DbNameTextBox;
        private System.Windows.Forms.TextBox IPTextBox;
        private System.Windows.Forms.Label IPLabel;
        private System.Windows.Forms.Label DBNameLabel;
        private System.Windows.Forms.Label DBUserLabel;
        private System.Windows.Forms.TextBox DbUserTextBox;
        private System.Windows.Forms.Label DbPassLabel;
        private System.Windows.Forms.TextBox DbPassTextbox;
        private System.Windows.Forms.TextBox ServerNameTextBox;
        private System.Windows.Forms.Label ServerNameLabel;
    }
}