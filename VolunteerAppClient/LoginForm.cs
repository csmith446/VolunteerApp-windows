#define DEBUG
using System;
using System.IO;
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
        //private IScsServiceClient<IVolunteerServer> Server;
        private ClientService Client;

        public LoginForm()
        {
            InitializeComponent();
#if DEBUG
            EmailAddressTextBox.Text = "csmith34@spsu.edu";
            PasswordTextBox.Text = "Chaeson1";
#endif
            Client = new ClientService(this);
        }

        private bool ValidateForm(string username, string password)
        {
            if (!String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(password))
                return true;

            return false;
        }

        private void BeginUserLogin()
        {
            //try
            //{
                string username = EmailAddressTextBox.Text.Trim().ToLower();
                string password = PasswordTextBox.Text.Trim();
                UserInfo user = null;

                if (ValidateForm(username, password))
                {
                    if (Client.LogInUser(username, password,out user))
                    {
                        var mainForm = new MainVolunteerForm(user, this, Client);
                        ErrorMessageLabel.Visible = false;

                        this.Hide();
                        mainForm.ShowDialog();
                    }
                    else
                    {
                        ErrorMessageLabel.Text = "The email and password do not match.";
                        ErrorMessageLabel.Visible = true;
                    }
                }
                else
                {
                    ErrorMessageLabel.Text = "The email and password cannot be blank.";
                    ErrorMessageLabel.Visible = true;
                }

                EmailAddressTextBox.Clear();
                PasswordTextBox.Clear();
                EmailAddressTextBox.Focus();
            //}
            //catch
            //{
            //    MessageBox.Show("Oops! The server appears to be down.");
            //}
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            BeginUserLogin();
        }

        private void ShowNewUserForm()
        {
            var createUser = new RegistrationForm(Client);
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
