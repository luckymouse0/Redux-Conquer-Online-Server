namespace Redux
{
    partial class ControlForm
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
            this.OffsetNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.ValueNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.OnlineCountTextbox = new System.Windows.Forms.TextBox();
            this.OnlineCountLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.OffsetNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ValueNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // OffsetNumericUpDown
            // 
            this.OffsetNumericUpDown.Location = new System.Drawing.Point(301, 324);
            this.OffsetNumericUpDown.Name = "OffsetNumericUpDown";
            this.OffsetNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.OffsetNumericUpDown.TabIndex = 0;
            this.OffsetNumericUpDown.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.OffsetNumericUpDown.ValueChanged += new System.EventHandler(this.OffsetNumericUpDown_ValueChanged);
            // 
            // ValueNumericUpDown
            // 
            this.ValueNumericUpDown.Location = new System.Drawing.Point(301, 298);
            this.ValueNumericUpDown.Name = "ValueNumericUpDown";
            this.ValueNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.ValueNumericUpDown.TabIndex = 1;
            this.ValueNumericUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ValueNumericUpDown.ValueChanged += new System.EventHandler(this.ValueNumericUpDown_ValueChanged);
            // 
            // OnlineCountTextbox
            // 
            this.OnlineCountTextbox.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.OnlineCountTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.OnlineCountTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OnlineCountTextbox.Location = new System.Drawing.Point(78, 12);
            this.OnlineCountTextbox.Name = "OnlineCountTextbox";
            this.OnlineCountTextbox.Size = new System.Drawing.Size(76, 22);
            this.OnlineCountTextbox.TabIndex = 2;
            this.OnlineCountTextbox.Text = "0";
            this.OnlineCountTextbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // OnlineCountLabel
            // 
            this.OnlineCountLabel.AutoSize = true;
            this.OnlineCountLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OnlineCountLabel.Location = new System.Drawing.Point(10, 11);
            this.OnlineCountLabel.Name = "OnlineCountLabel";
            this.OnlineCountLabel.Size = new System.Drawing.Size(71, 24);
            this.OnlineCountLabel.TabIndex = 3;
            this.OnlineCountLabel.Text = "Online:";
            // 
            // ControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(433, 356);
            this.Controls.Add(this.OnlineCountLabel);
            this.Controls.Add(this.OnlineCountTextbox);
            this.Controls.Add(this.ValueNumericUpDown);
            this.Controls.Add(this.OffsetNumericUpDown);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ControlForm";
            this.Text = "[Redux]";
            ((System.ComponentModel.ISupportInitialize)(this.OffsetNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ValueNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown OffsetNumericUpDown;
        private System.Windows.Forms.NumericUpDown ValueNumericUpDown;
        private System.Windows.Forms.TextBox OnlineCountTextbox;
        private System.Windows.Forms.Label OnlineCountLabel;
    }
}

