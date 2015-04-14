#define DEBUG
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
        private IScsServiceClient<IVolunteerServer> Server;
        private ClientService Client;

        public LoginForm()
        {
            InitializeComponent();
#if DEBUG
            EmailAddressTextBox.Text = "csmith34@spsu.edu";
            PasswordTextBox.Text = "Chaeson1";
#endif

            Client = new ClientService(this);
            Server = ScsServiceClientBuilder.CreateClient<IVolunteerServer>(
                new ScsTcpEndPoint("127.0.0.1", 31415), Client);

            Server.Connected += Server_Connected;
            Server.Disconnected += Server_Disconnected;
        }

        void Server_Disconnected(object sender, EventArgs e)
        {
            //do stuff when client disconnects from server
        }

        void Server_Connected(object sender, EventArgs e)
        {
            //do stuff when client connects to server
        }

        private bool UserLoggedIn(string username, string password)
        {
            if (Server.ServiceProxy.CheckCredentials(username, MD5Hasher.GetHashedValue(password)))
                return true;

            return false;
        }

        private bool ValidateForm(string username, string password)
        {
            if (!String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(password))
                return true;

            return false;
        }

        private void BeginUserLogin()
        {
            Server.Connect();
            string username = EmailAddressTextBox.Text.ToLower();
            string password = PasswordTextBox.Text;

            if (ValidateForm(username, password))
            {
                if (UserLoggedIn(username, password))
                {
                    var user = Server.ServiceProxy.GetLoggedOnUser();
                    var mainForm = new MainVolunteerForm(user, this, Server);
                    ErrorMessageLabel.Visible = false;

                    this.Hide();
                    mainForm.ShowDialog();
                }
                else
                {
                    ErrorMessageLabel.Text = "The email and password do not match.";
                    ErrorMessageLabel.Visible = true;
                    Server.Disconnect();
                }
            }
            else
            {
                ErrorMessageLabel.Text = "The email and password cannot be blank.";
                ErrorMessageLabel.Visible = true;
                Server.Disconnect();
            }

            EmailAddressTextBox.Clear();
            PasswordTextBox.Clear();
            EmailAddressTextBox.Focus();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            BeginUserLogin();
        }

        private void ShowNewUserForm()
        {
            var createUser = new RegistrationForm(Server);
            createUser.ShowDialog();
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
            var about = new AboutBox();
            about.ShowDialog();
        }

        private void LoginForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter | e.KeyData == Keys.Return)
            {
                BeginUserLogin();
                e.Handled = true;
            }
        }
    }
}
