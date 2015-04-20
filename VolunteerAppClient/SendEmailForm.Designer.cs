namespace VolunteerAppClient
{
    partial class SendEmailForm
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
            this.EmailRecipientTextBox = new System.Windows.Forms.TextBox();
            this.EmailSubjectTextbox = new System.Windows.Forms.TextBox();
            this.EmailBodyTextbox = new System.Windows.Forms.TextBox();
            this.SendEmailButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.EmailStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // EmailRecipientTextBox
            // 
            this.EmailRecipientTextBox.Location = new System.Drawing.Point(58, 12);
            this.EmailRecipientTextBox.Name = "EmailRecipientTextBox";
            this.EmailRecipientTextBox.ReadOnly = true;
            this.EmailRecipientTextBox.Size = new System.Drawing.Size(354, 20);
            this.EmailRecipientTextBox.TabIndex = 0;
            // 
            // EmailSubjectTextbox
            // 
            this.EmailSubjectTextbox.Location = new System.Drawing.Point(58, 38);
            this.EmailSubjectTextbox.Name = "EmailSubjectTextbox";
            this.EmailSubjectTextbox.Size = new System.Drawing.Size(354, 20);
            this.EmailSubjectTextbox.TabIndex = 1;
            // 
            // EmailBodyTextbox
            // 
            this.EmailBodyTextbox.AcceptsReturn = true;
            this.EmailBodyTextbox.Location = new System.Drawing.Point(12, 64);
            this.EmailBodyTextbox.Multiline = true;
            this.EmailBodyTextbox.Name = "EmailBodyTextbox";
            this.EmailBodyTextbox.Size = new System.Drawing.Size(460, 158);
            this.EmailBodyTextbox.TabIndex = 2;
            this.EmailBodyTextbox.TextChanged += new System.EventHandler(this.EmailBodyTextbox_TextChanged);
            // 
            // SendEmailButton
            // 
            this.SendEmailButton.BackColor = System.Drawing.Color.PaleGreen;
            this.SendEmailButton.Enabled = false;
            this.SendEmailButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SendEmailButton.Location = new System.Drawing.Point(418, 12);
            this.SendEmailButton.Name = "SendEmailButton";
            this.SendEmailButton.Size = new System.Drawing.Size(54, 46);
            this.SendEmailButton.TabIndex = 3;
            this.SendEmailButton.Text = "Send";
            this.SendEmailButton.UseVisualStyleBackColor = false;
            this.SendEmailButton.Click += new System.EventHandler(this.SendEmailButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "To: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Subject";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EmailStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 225);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(484, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // EmailStatusLabel
            // 
            this.EmailStatusLabel.Name = "EmailStatusLabel";
            this.EmailStatusLabel.Size = new System.Drawing.Size(46, 17);
            this.EmailStatusLabel.Text = "[status]";
            // 
            // SendEmailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 247);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SendEmailButton);
            this.Controls.Add(this.EmailBodyTextbox);
            this.Controls.Add(this.EmailSubjectTextbox);
            this.Controls.Add(this.EmailRecipientTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SendEmailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Send Email To [contact]";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox EmailRecipientTextBox;
        private System.Windows.Forms.TextBox EmailSubjectTextbox;
        private System.Windows.Forms.TextBox EmailBodyTextbox;
        private System.Windows.Forms.Button SendEmailButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel EmailStatusLabel;
    }
}