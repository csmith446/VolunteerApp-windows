﻿using System;
using System.Globalization;
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
    public partial class ViewEventForm : Form
    {
        //todo: input validation and error providers for edits, db update query
        //todo: dialog result
        private IScsServiceClient<IVolunteerServer> Server;
        private EventInfo EventToEdit;

        public ViewEventForm(EventInfo evt, bool readOnly,
            IScsServiceClient<IVolunteerServer> server)
        {
            InitializeComponent();
            EventToEdit = evt;
            Server = server;

            SetControls(readOnly);
            string title = evt.Name;
            this.Text = title + ((readOnly) ? " : View Detailed Event" : " : Edit Event");
        }

        private void SetControls(bool readOnly)
        {
            NameTextBox.Text = EventToEdit.Name;
            LocationTextBox.Text = EventToEdit.Location;
            EventDatePicker.Value = DateTime.ParseExact(EventToEdit.Date, 
                "MM/dd/yyyy", CultureInfo.InvariantCulture);
            StartTimePicker.Value = EventToEdit.StartTime;
            EndTimePicker.Value = EventToEdit.EndTime;

            NameTextBox.ReadOnly = readOnly;
            LocationTextBox.ReadOnly = readOnly;
            EventDatePicker.Enabled = !readOnly;
            StartTimePicker.Enabled = !readOnly;
            EndTimePicker.Enabled = !readOnly;
        }

        private void CancelEditButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
