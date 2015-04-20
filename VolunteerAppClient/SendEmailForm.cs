using System;
using System.Net.Mail;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VolunteerAppClient
{
    public partial class SendEmailForm : Form
    {
        private SmtpClient SmtpHandle;
        private MailMessage EmailMessage;
        private BackgroundWorker EmailWorker;
        private string Sender;

        public SendEmailForm(string recipient, string sender)
        {
            InitializeComponent();
            Sender = sender;
            EmailRecipientTextBox.Text = recipient;
            this.Text = this.Text.Replace("[contact]", recipient);
            EmailSubjectTextbox.Text = string.Format("{0} - Question about event", sender);
            EmailStatusLabel.Text = "Ready";

            EmailWorker = new BackgroundWorker();
            EmailWorker.DoWork += EmailWorker_DoWork;
            EmailWorker.RunWorkerCompleted += EmailWorker_RunWorkerCompleted;

            var messageSender = new MailAddress("it.3883.volunteerapp@gmail.com", "noreply@volunteerapp.com");
            EmailMessage = new MailMessage();
            EmailMessage.Sender = messageSender;
            EmailMessage.From = messageSender;

            SmtpHandle = new SmtpClient("smtp.gmail.com");
            SmtpHandle.Port = 587;
            SmtpHandle.Credentials = new System.Net.NetworkCredential("it.3883.volunteerapp", "VolunteerApp");
            SmtpHandle.EnableSsl = true;

            EmailBodyTextbox.Focus();
        }

        void EmailWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EmailStatusLabel.Text = "Message Sent";
            MessageBox.Show("Message sent succfessully", "Success");
            this.Close();
        }

        void EmailWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            EmailStatusLabel.Text = "Sending...";
            SendEmail();
        }

        private void SendEmail()
        {
            EmailMessage.To.Add(EmailRecipientTextBox.Text);
            EmailMessage.Subject = EmailSubjectTextbox.Text.Trim();

            var sb = new StringBuilder();
            sb.Append(string.Format("\r\n\r\nThis message has been to you from: {0}.\r\n\r\n\r\n", Sender));
            sb.AppendLine("--------------------------------------------------------------------------------");
            foreach (var line in EmailBodyTextbox.Lines)
            {
                sb.AppendLine(line);
            }
            sb.Append("\r\n\r\nThank you for using Volunteer App!\r\n");
            sb.Append("\r\nVolunteer App Team\r\n\r\n");

            EmailMessage.Body = sb.ToString();
            SmtpHandle.Send(EmailMessage);
        }

        private void SendEmailButton_Click(object sender, EventArgs e)
        {
            EmailWorker.RunWorkerAsync();
        }

        private void EmailBodyTextbox_TextChanged(object sender, EventArgs e)
        {
            SendEmailButton.Enabled = EmailBodyTextbox.Text.Length > 0;
        }
    }
}
