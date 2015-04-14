﻿using System;
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
    public partial class ViewUserForm : Form
    {
        //todo: input validation, error provider, and save changes functionality
        //todo: dialog result
        private IScsServiceClient<IVolunteerServer> Server;

        private UserInfo UserToEdit;
        public ViewUserForm(UserInfo user, bool adminEdit,
            IScsServiceClient<IVolunteerServer> server)
        {
            InitializeComponent();
            Server = server;

            string title = string.Format("{1}, {0} : ", user.FullName.Item1, user.FullName.Item2);
            this.Text = title + ((adminEdit) ? "Edit Information" : "Update Information");
            FirstNameTextBox.Enabled = LastNameTextBox.Enabled = adminEdit;
            IsAdminCheckBox.Visible = adminEdit;
            IsAdminCheckBox.Enabled = !user.IsAdmin;
            UserToEdit = user;
            SetEditValues();
        }

        private void SetEditValues()
        {
            FirstNameTextBox.Text = UserToEdit.FullName.Item1;
            LastNameTextBox.Text = UserToEdit.FullName.Item2;
            EmailAddressTextBox.Text = UserToEdit.Username;
            PhoneNumberTextBox.Text = UserToEdit.PhoneNumber;
            IsAdminCheckBox.Checked = UserToEdit.IsAdmin;
        }

        private void ChangePasswordCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(ChangePasswordCheckBox.Checked)
            {
                var size = new Size(430, 278);
                this.Size = size;
                PasswordLabel.Visible = ConfirmPasswordLabel.Visible =
                    PasswordTextBox.Visible = ConfirmPasswordTextBox.Visible = true;
            }
            else
            {
                var size = new Size(430, 208);
                this.Size = size;
                PasswordLabel.Visible = ConfirmPasswordLabel.Visible =
                    PasswordTextBox.Visible = ConfirmPasswordTextBox.Visible = false;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void IsAdminCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (UserToEdit.IsAdmin)
                IsAdminCheckBox.Checked = true;
        }
    }
}
