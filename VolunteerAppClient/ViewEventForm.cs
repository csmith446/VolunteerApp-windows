using System;
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
    internal partial class ViewEventForm : Form
    {
        private ClientService Client;
        private EventInfo EventToEdit;

        private const double TIME_CONST = 1.66666667;
        private const string EVENT_NAME_ERROR = "The event name cannot be left blank.";
        private const string EVENT_LOC_ERROR = "The event location cannot be left blank.";


        public ViewEventForm(EventInfo evt, bool readOnly,
            ClientService client)
        {
            InitializeComponent();
            EventToEdit = evt;
            Client = client;

            SetControls();
            SetPermissions(readOnly);
            string title = evt.Name;
            this.Text = title + ((readOnly) ? " : View Detailed Event" : " : Edit Event");

            var eventDate = DateTime.ParseExact(EventToEdit.Date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
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

        private void SetPermissions(bool readOnly)
        {
            NameTextBox.ReadOnly = readOnly;
            LocationTextBox.ReadOnly = readOnly;
            EventDatePicker.Enabled = !readOnly;
            StartTimePicker.Enabled = !readOnly;
            EndTimePicker.Enabled = !readOnly;
            SaveChangesButton.Enabled = !readOnly;
        }

        private void SetControls()
        {
            NameTextBox.Text = EventToEdit.Name;
            LocationTextBox.Text = EventToEdit.Location;
            EventDatePicker.Value = DateTime.ParseExact(EventToEdit.Date, 
                "MM/dd/yyyy", CultureInfo.InvariantCulture);
            StartTimePicker.Value = EventToEdit.StartTime;
            EndTimePicker.Value = EventToEdit.EndTime;
        }

        private void CloseEditEventForm()
        {
            if (CheckForChanges())
            {
                if (MessageBox.Show("Are you sure you want to cancel the changes made?", "Cancel Changes?",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                    this.Close();
            }
            else
            {
                this.Close();
            }
        }

        private void CancelEditButton_Click(object sender, EventArgs e)
        {
            CloseEditEventForm();
        }

        private void SaveChangesButton_Click(object sender, EventArgs e)
        {
            ProcessChangesMade();
        }

        private bool CheckForChanges()
        {
            if (NameTextBox.Text != EventToEdit.Name ||
                LocationTextBox.Text != EventToEdit.Location ||
                EventDatePicker.Value != DateTime.ParseExact(EventToEdit.Date,
                    "MM/dd/yyyy", CultureInfo.InvariantCulture) ||
                StartTimePicker.Value != EventToEdit.StartTime ||
                EndTimePicker.Value != EventToEdit.EndTime)
            {
                return true;
            }

            return false;
        }

        private void EditEventInformation()
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

            Client.UpdateEventInfo(EventToEdit.Id, eventName, eventDate, startTime, eventDuration, location);
        }

        private void ProcessChangesMade()
        {
            if (CheckForChanges())
            {
                if (ValidateForm())
                {
                    EditEventInformation();
                    MessageBox.Show("Event information has been updated.", "Update Successful");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("There were no changes made.", "Error");
            }
        }

        private bool ValidateForm()
        {
            if (NameIsValid && LocationIsValid)
            {
                return true;
            }

            ShowAllErrors();
            MessageBox.Show("The event was not updated. Errors exist on the page.",
                "Error");
            return false;
        }

        private void ShowAllErrors()
        {
            if (!NameIsValid) SetErrorForControl(NameTextBox, EVENT_NAME_ERROR);
            if (!LocationIsValid) SetErrorForControl(LocationTextBox, EVENT_LOC_ERROR);
        }

        private bool NameIsValid = true;
        private bool ValidateName()
        {
            NameIsValid = false;
            if (!string.IsNullOrEmpty(NameTextBox.Text))
                NameIsValid = true;

            return NameIsValid;
        }

        private bool LocationIsValid = true;
        private bool ValidateLocation()
        {
            LocationIsValid = false;
            if (!string.IsNullOrEmpty(LocationTextBox.Text))
                LocationIsValid = true;

            return LocationIsValid;
        }

        private void SetErrorForControl(Control control, string error = "")
        {
            EditEventErrorProvider.SetError(control, error);
            EditEventErrorProvider.SetIconPadding(control, 10);
        }

        private void EndTimePicker_ValueChanged(object sender, EventArgs e)
        {
            StartTimePicker.MaxDate = EndTimePicker.Value.AddHours(-1);
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
