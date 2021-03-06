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
    internal partial class CreateEventForm : Form
    {
        private ClientService Client;
        private UserInfo Creator;

        private const double TIME_CONST = 1.66666667;
        private const string EVENT_NAME_ERROR = "The event name cannot be left blank.";
        private const string EVENT_LOC_ERROR = "The event location cannot be left blank.";

        public CreateEventForm(UserInfo currentUser,
            ClientService client)
        {
            InitializeComponent();
            Client = client;
            Creator = currentUser;

            var currentDate = DateTime.Now;
            EventDatePicker.MinDate = currentDate.AddDays(1);
            EventDatePicker.MaxDate = currentDate.AddMonths(6).AddDays(1);

            StartTimePicker.MinDate = new DateTime(currentDate.Year, currentDate.Month, 
                currentDate.Day, 8, 0, 0);
            EndTimePicker.MinDate = StartTimePicker.MinDate.AddHours(1);

            EndTimePicker.MaxDate = new DateTime(currentDate.Year, currentDate.Month,
                currentDate.Day, 22, 0, 0);
            StartTimePicker.MaxDate = EndTimePicker.Value.AddHours(-1);
        }

        private void CreateNewEvent()
        {
            string eventName = NameTextBox.Text.Trim(), location = LocationTextBox.Text.Trim(),
                startTime = StartTimePicker.Value.ToString("h:mm tt"),
                endTime = EndTimePicker.Value.ToString("h:mm tt"),
                eventDate = EventDatePicker.Value.ToString("MM/dd/yyyy");

            string timeDuration = EndTimePicker.Value.Subtract(StartTimePicker.Value).ToString();
            string[] timeParts = timeDuration.Split(':');

            double hour = double.Parse(timeParts[0]);
            double minutes = (double.Parse(timeParts[1]) * TIME_CONST) / 100.0;
            string eventDuration = string.Format("{0:F2}", hour + minutes);

           Client.CreateNewEvent(Creator.Id, eventName, eventDate, startTime, eventDuration, location);
        }

        private void ProcessNewEventCreation()
        {
            if (ValidateForm())
            {
                CreateNewEvent();
                MessageBox.Show("Event has been created and registered.", "Event Created Successfully");
                this.Close();
            }
        }

        private bool ValidateForm()
        {
            if (NameIsValid && LocationIsValid)
            {
                return true;
            }

            ShowAllErrors();
            MessageBox.Show("The event was not created. Errors exist on the page.",
                "Errors");
            return false;
        }

        private void ShowAllErrors()
        {
            if (!NameIsValid) SetErrorForControl(NameTextBox, EVENT_NAME_ERROR);
            if (!LocationIsValid) SetErrorForControl(LocationTextBox, EVENT_LOC_ERROR);
        }

        private bool NameIsValid = false;
        private bool ValidateName()
        {
            NameIsValid = false;
            if (!string.IsNullOrEmpty(NameTextBox.Text))
                NameIsValid = true;

            return NameIsValid;
        }

        private bool LocationIsValid = false;
        private bool ValidateLocation()
        {
            LocationIsValid = false;
            if (!string.IsNullOrEmpty(LocationTextBox.Text))
                LocationIsValid = true;

            return LocationIsValid;
        }

        private void SetErrorForControl(Control control, string error = "")
        {
            CreateEventErrorProvider.SetError(control, error);
            CreateEventErrorProvider.SetIconPadding(control, 10);
        }

        private void CancelNewEventButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EndTimePicker_ValueChanged(object sender, EventArgs e)
        {
            StartTimePicker.MaxDate = EndTimePicker.Value.AddHours(-1);
        }

        private void CreateEventButton_Click(object sender, EventArgs e)
        {
            ProcessNewEventCreation();
        }

        private void NameTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidateName())
                SetErrorForControl(NameTextBox, EVENT_NAME_ERROR);
            else
                SetErrorForControl(NameTextBox);
        }

        private void LocationTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidateLocation())
                SetErrorForControl(LocationTextBox, EVENT_LOC_ERROR);
            else
                SetErrorForControl(LocationTextBox);
        }
    }
}
