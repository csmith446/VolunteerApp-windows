namespace VolunteerAppServer
{
    partial class VolunteerServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VolunteerServer));
            this.ServerMenuStrip = new System.Windows.Forms.MenuStrip();
            this.FileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ServerStatusStrip = new System.Windows.Forms.StatusStrip();
            this.ServerToolstripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ControlButton = new System.Windows.Forms.Button();
            this.ServerLogListBox = new System.Windows.Forms.ListBox();
            this.ClearLogButton = new System.Windows.Forms.Button();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ServerMenuStrip.SuspendLayout();
            this.ServerStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ServerMenuStrip
            // 
            this.ServerMenuStrip.BackColor = System.Drawing.SystemColors.Control;
            this.ServerMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuItem,
            this.HelpMenuItem});
            this.ServerMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.ServerMenuStrip.Name = "ServerMenuStrip";
            this.ServerMenuStrip.Size = new System.Drawing.Size(478, 24);
            this.ServerMenuStrip.TabIndex = 0;
            this.ServerMenuStrip.Text = "menuStrip1";
            // 
            // FileMenuItem
            // 
            this.FileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExitMenuItem});
            this.FileMenuItem.Name = "FileMenuItem";
            this.FileMenuItem.Size = new System.Drawing.Size(37, 20);
            this.FileMenuItem.Text = "File";
            // 
            // HelpMenuItem
            // 
            this.HelpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AboutMenuItem});
            this.HelpMenuItem.Name = "HelpMenuItem";
            this.HelpMenuItem.Size = new System.Drawing.Size(44, 20);
            this.HelpMenuItem.Text = "Help";
            // 
            // ServerStatusStrip
            // 
            this.ServerStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ServerToolstripLabel});
            this.ServerStatusStrip.Location = new System.Drawing.Point(0, 242);
            this.ServerStatusStrip.Name = "ServerStatusStrip";
            this.ServerStatusStrip.Size = new System.Drawing.Size(478, 22);
            this.ServerStatusStrip.SizingGrip = false;
            this.ServerStatusStrip.TabIndex = 1;
            this.ServerStatusStrip.Text = "statusStrip1";
            // 
            // ServerToolstripLabel
            // 
            this.ServerToolstripLabel.Margin = new System.Windows.Forms.Padding(10, 3, 0, 2);
            this.ServerToolstripLabel.Name = "ServerToolstripLabel";
            this.ServerToolstripLabel.Size = new System.Drawing.Size(80, 17);
            this.ServerToolstripLabel.Text = "Server : Ready";
            // 
            // ControlButton
            // 
            this.ControlButton.BackColor = System.Drawing.Color.LightGreen;
            this.ControlButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ControlButton.Location = new System.Drawing.Point(394, 28);
            this.ControlButton.Name = "ControlButton";
            this.ControlButton.Size = new System.Drawing.Size(75, 30);
            this.ControlButton.TabIndex = 2;
            this.ControlButton.Text = "Start";
            this.ControlButton.UseVisualStyleBackColor = false;
            this.ControlButton.Click += new System.EventHandler(this.ControlButton_Click);
            // 
            // ServerLogListBox
            // 
            this.ServerLogListBox.FormattingEnabled = true;
            this.ServerLogListBox.Location = new System.Drawing.Point(10, 28);
            this.ServerLogListBox.Name = "ServerLogListBox";
            this.ServerLogListBox.Size = new System.Drawing.Size(374, 199);
            this.ServerLogListBox.TabIndex = 3;
            // 
            // ClearLogButton
            // 
            this.ClearLogButton.BackColor = System.Drawing.Color.SandyBrown;
            this.ClearLogButton.Enabled = false;
            this.ClearLogButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ClearLogButton.Location = new System.Drawing.Point(394, 78);
            this.ClearLogButton.Name = "ClearLogButton";
            this.ClearLogButton.Size = new System.Drawing.Size(75, 30);
            this.ClearLogButton.TabIndex = 4;
            this.ClearLogButton.Text = "Clear Log";
            this.ClearLogButton.UseVisualStyleBackColor = false;
            this.ClearLogButton.Click += new System.EventHandler(this.ClearLogButton_Click);
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.Size = new System.Drawing.Size(152, 22);
            this.ExitMenuItem.Text = "Exit";
            this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // AboutMenuItem
            // 
            this.AboutMenuItem.Name = "AboutMenuItem";
            this.AboutMenuItem.Size = new System.Drawing.Size(152, 22);
            this.AboutMenuItem.Text = "About";
            this.AboutMenuItem.Click += new System.EventHandler(this.AboutMenuItem_Click);
            // 
            // VolunteerServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 264);
            this.Controls.Add(this.ClearLogButton);
            this.Controls.Add(this.ServerLogListBox);
            this.Controls.Add(this.ControlButton);
            this.Controls.Add(this.ServerStatusStrip);
            this.Controls.Add(this.ServerMenuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.ServerMenuStrip;
            this.MaximizeBox = false;
            this.Name = "VolunteerServer";
            this.Text = "Volunter Application Server";
            this.ServerMenuStrip.ResumeLayout(false);
            this.ServerMenuStrip.PerformLayout();
            this.ServerStatusStrip.ResumeLayout(false);
            this.ServerStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip ServerMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpMenuItem;
        private System.Windows.Forms.StatusStrip ServerStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel ServerToolstripLabel;
        private System.Windows.Forms.Button ControlButton;
        private System.Windows.Forms.ListBox ServerLogListBox;
        private System.Windows.Forms.Button ClearLogButton;
        private System.Windows.Forms.ToolStripMenuItem AboutMenuItem;
    }
}

