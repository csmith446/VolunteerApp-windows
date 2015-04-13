using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Client;
using VolunteerAppCommonLib;

namespace VolunteerAppClient
{
    public partial class LoginForm : Form
    {
        private static IScsServiceClient<IVolunteerServer> Server;
        private static ClientService Client;

        public LoginForm()
        {
            InitializeComponent();
            Client = new ClientService(this);
            Server = ScsServiceClientBuilder.CreateClient<IVolunteerServer>(
                new ScsTcpEndPoint("127.0.0.1", 31415), Client);

            Server.Connected += Server_Connected;
            Server.Disconnected += Server_Disconnected;

            //debug use
            EmailAddressTextBox.Text = "csmith34@spsu.edu";
            PasswordTextBox.Text = "Chaeson1";
        }

        private void ConnectClient()
        {
            string username = EmailAddressTextBox.Text.ToLower();
            string password = PasswordTextBox.Text;

            if (ValidateForm(username, password))
            {
                Server.Connect();
            }
            else
            {
                ErrorMessageLabel.Text = "The email and password cannot be blank.";
                ErrorMessageLabel.Visible = true;
            }
        }

        void Server_Disconnected(object sender, EventArgs e)
        {
            //show login window
        }

        void Server_Connected(object sender, EventArgs e)
        {
            Server.ServiceProxy.CheckCredentials(EmailAddressTextBox.Text.ToLower(),
                MD5Hasher.GetHashedValue(PasswordTextBox.Text));
        }

        public void ShowLoginResult(bool result)
        {
            string status = result ? "yay!" : "boo!";
            MessageBox.Show(status);

            if (result == false)
                Server.Disconnect();
        }

        private bool ValidateForm(string username, string password)
        {
            if (!String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(password))
                return true;

            return false;
        }

        //private void BeginUserLogin()
        //{
        //    string username = EmailAddressTextBox.Text.ToLower();
        //    string password = PasswordTextBox.Text;

        //    if (ValidateForm(username, password))
        //    {
        //        if (LogInUser(username, password))
        //        {
        //            //var user = DatabaseManager.GetUserInformation(username);
        //            //var mainForm = new MainVolunteerForm(user, this);
        //            ErrorMessageLabel.Visible = false;

        //            this.Hide();
        //            mainForm.ShowDialog();
        //        }
        //        else
        //        {
        //            ErrorMessageLabel.Text = "The email and password do not match.";
        //            ErrorMessageLabel.Visible = true;
        //        }
        //    }
        //    else
        //    {
        //        ErrorMessageLabel.Text = "The email and password cannot be blank.";
        //        ErrorMessageLabel.Visible = true;
        //    }

        //    EmailAddressTextBox.Clear();
        //    PasswordTextBox.Clear();
        //    EmailAddressTextBox.Focus();
        //}

        private void LoginButton_Click(object sender, EventArgs e)
        {
            ConnectClient();
        }

        private void ShowNewUserForm()
        {
            //var createUser = new RegistrationForm();
            //createUser.ShowDialog();
        }

        private void CreateUserButton_Click(object sender, EventArgs e)
        {
            ShowNewUserForm();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            CloseApplication();
        }

        private void CloseApplication()
        {
            this.Close();
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            CloseApplication();
        }

        private void SignUpMenuItem_Click(object sender, EventArgs e)
        {
            ShowNewUserForm();
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            //var about = new AboutBox();
            //about.ShowDialog();
        }

        private void LoginForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter | e.KeyData == Keys.Return)
            {
                //BeginUserLogin();
                e.Handled = true;
            }
        }
    }
}
