using System;
using System.IO;
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
    internal partial class MainVolunteerForm : Form
    {
        private ClientService Client;

        //[todo] get rid of redundant lists
        private List<int> SelectedEventIds;     //list for 'saving' selected events when sorting
        private List<UserInfo> UserList;            //main User list - pulled form database
        private List<EventInfo> EventList;          //main event list - pulled from database
        private List<ListViewItem> RegisteredEventItems;

        private LoginForm LoginForm;
        private UserInfo CurrentUser;

        private BackgroundWorker RefreshListsWorker;

        private enum OrderBy
        {
            Name,
            NameDesc,
            Location,
            LocationDesc,
            Date,
            DateDesc
        };

        public MainVolunteerForm(UserInfo user, LoginForm loginScreen,
            ClientService client)
        {
            InitializeComponent();

            SetDoubleBuffered(VolunteerEventsListView);
            SetDoubleBuffered(UserCreatedEventsListView);
            SetDoubleBuffered(UserRegisteredEventsListView);
            SetDoubleBuffered(AdminEventListView);
            SetDoubleBuffered(AdminUserListView);

            CurrentUser = user;
            LoginForm = loginScreen;
            Client = client;

            Client.GetDatabaseInfo(out EventList, out UserList, this);
            SetRegisteredCountLable();
            SetLoggedInUserName();
            SetAdminStatus();

            SelectedEventIds = new List<int>();
            RefreshListViews();

            RefreshListsWorker = new BackgroundWorker();
            RefreshListsWorker.WorkerReportsProgress = true;
            RefreshListsWorker.DoWork += RefreshListsWorker_DoWork;
            RefreshListsWorker.RunWorkerCompleted += RefreshListsWorker_RunWorkerCompleted;

            if (CurrentUser.IsAdmin)
            {
                AdminTotalUsersLabel.Text = string.Format("Total Users: {0}", UserList.Count);
                AdminTotalCountLabel.Text = string.Format("Total Events: {0}", EventList.Count);
                AdminCurrentEventCountLabel.Text = string.Format("Current Events: {0}",
                    EventList.FindAll(x => x.Current).Count);
            }
        }

        internal void RefreshListsWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var status = LogInStatusLabel.Text.Replace("Connected", "Connected");
        }

        internal void RefreshListsWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var status = LogInStatusLabel.Text.Replace("Connected", "Loading...");
            var updatedUser = UserList.Find(x => x.Id == CurrentUser.Id);
            CurrentUser = updatedUser;
            RefreshListViews();
            if (CurrentUser.IsAdmin) UpdateAdminCounts();
        }

        internal void RefreshListViews()
        {
            LoadCurrentEvents();
            LoadUserCreatedEvents();
            LoadUserRegisteredEvents();
            if (CurrentUser.IsAdmin) { GetAllEventsToManage(); GetAllUsersToManage(); }
            SetRegisteredCountLable();
        }

        private UserInfo GetUserFromId(int userId)
        {
            return UserList.Find(x => x.Id == userId) as UserInfo;
        }

        private EventInfo GetEventFromId(int eventId)
        {
            return EventList.Find(x => x.Id == eventId) as EventInfo;
        }

        private static void SetDoubleBuffered(Control control)
        {
            System.Reflection.PropertyInfo aProp =
                  typeof(System.Windows.Forms.Control).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

            aProp.SetValue(control, true, null);
        }

        internal void UpdateLists(List<EventInfo> events, List<UserInfo> users)
        {
            EventList = events;
            UserList = users;
            RefreshListsWorker.RunWorkerAsync();
        }

        private void SetAdminStatus()
        {
            if (!CurrentUser.IsAdmin)
            {
                MainTabControl.TabPages.Remove(AdminTab);
            }
        }

        private void SetRegisteredCountLable()
        {
            //int events = Server.ServiceProxy.GetRegisteredCountForUser(CurrentUser.Id);
            var events = EventList.FindAll(x => CurrentUser.RegisteredEvents.Contains(x.Id));
            var count = events.FindAll(x => x.Current == true).Count;
            RegisteredEventCountLabel.Text = string.Format("Registered for {0} upcoming events", count);
        }

        private void SetLoggedInUserName()
        {
            string loginStatus = LogInStatusLabel.Text.Replace("[username]",
                String.Format("{0}, {1}", CurrentUser.FullName.Item2,
                CurrentUser.FullName.Item1));
            if (CurrentUser.IsAdmin) loginStatus += " (Administrator)";

            LogInStatusLabel.Text = loginStatus;
        }
        //private OrderBy CurrentOrder;
        private delegate void LoadCurrentCallBack();
        private void LoadCurrentEvents()
        {
            if (VolunteerEventsListView.InvokeRequired)
            {
                var callBack = new LoadCurrentCallBack(LoadCurrentEvents);
                Invoke(callBack);
            }
            else
            {
                VolunteerEventsListView.Items.Clear();

                VolunteerEventsListView.CheckBoxes = true;
                RegisteredEventItems = new List<ListViewItem>();

                var currentEvents = EventList;
                //switch (order)
                //{
                //    case OrderBy.Name:
                //        currentEvents = currentEvents.OrderBy(evt => evt.Name).ToList();
                //        break;
                //    case OrderBy.NameDesc:
                //        currentEvents = currentEvents.OrderByDescending(evt => evt.Name).ToList();
                //        break;
                //    case OrderBy.Location:
                //        currentEvents = currentEvents.OrderBy(evt => evt.Location).ToList();
                //        break;
                //    case OrderBy.LocationDesc:
                //        currentEvents = currentEvents.OrderByDescending(evt => evt.Location).ToList();
                //        break;
                //    case OrderBy.DateDesc:
                //        currentEvents = currentEvents.OrderByDescending(evt => evt.Date).ToList();
                //        break;
                //}

                //CurrentOrder = order;
                //FixHeaderColors();

                foreach (var evt in currentEvents)
                {
                    if (evt.Current)
                    {
                        var creator = GetUserFromId(evt.CreatorId);
                        //visible subitems
                        var item = new ListViewItem(evt.Name);
                        item.SubItems.Add(evt.Location);
                        item.SubItems.Add(evt.Date);

                        //'invisible' subitems
                        item.SubItems.Add(evt.StartTime.ToString("hh:mm tt") +
                            " - " + evt.EndTime.ToString("hh:mm tt"));
                        item.SubItems.Add(String.Format("{1}, {0}",
                            creator.FullName.Item1,
                            creator.FullName.Item2));
                        item.SubItems.Add(creator.PhoneNumber);
                        item.SubItems.Add(creator.Username);
                        item.SubItems.Add(evt.RegisteredUsers.Count.ToString());
                        item.SubItems.Add(evt.Id.ToString());
                        item.SubItems.Add(evt.Duration.ToString());

                        if (CurrentUser.RegisteredEvents.Contains(evt.Id) || CurrentUser.CreatedEvents.Contains(evt.Id))
                            RegisteredEventItems.Add(item);
                        else
                        {
                            if (SelectedEventIds.Contains(evt.Id)) item.Checked = true;
                            VolunteerEventsListView.Items.Add(item);
                        }
                    }
                }

                if (!HideRegisteredEventsCheckBox.Checked)
                {
                    foreach (var evt in RegisteredEventItems)
                    {
                        evt.ForeColor = Color.Red;
                        VolunteerEventsListView.Items.Add(evt);
                    }
                }

                if (VolunteerEventsListView.Items.Count == 0)
                {
                    VolunteerEventsListView.Items.Add(new ListViewItem("There are no new events"));
                    VolunteerEventsListView.CheckBoxes = false;
                    VolunteerEventsListView.Enabled = false;
                    DetailedInformationGroupBox.Visible = false;
                }
                else
                {
                    VolunteerEventsListView.Enabled = true;
                    DetailedInformationGroupBox.Visible = true;
                }

                HideRegisteredEventsCheckBox.Visible = RegisteredEventItems.Count > 0;
                RedEventEntryLabel.Visible = RegisteredEventItems.Count > 0;
                VolunteerEventsListView.Items[0].Selected = true;
                AdjustEventListViewSize();
            }
        }

        private void FixHeaderColors()
        {
            //UpcomingEventNameHeaderButton.BackColor = (CurrentOrder == OrderBy.Name) ? Color.Green : Color.PaleGreen; ;
            //UpcomingEventDateHeaderButton.BackColor = (CurrentOrder == OrderBy.Date) ? Color.Green : Color.PaleGreen;
            //UpcomingEventLocationHeaderButton.BackColor = (CurrentOrder == OrderBy.Location) ? Color.Green : Color.PaleGreen;
        }

        private void VolunteerEventsListView_ItemSelectionChanged(object sender,
            ListViewItemSelectionChangedEventArgs e)
        {
            //todo: possibly make the group box look nicer?
            var item = e.Item;
            if (item.Text != "There are no new events")
            {
                if (item.Selected)
                {
                    DetailedEventTimeLabel.Text = item.SubItems[3].Text;
                    DetailedContactNameLabel.Text = item.SubItems[4].Text;
                    DetailedContactNumberLabel.Text = item.SubItems[5].Text;
                    DetailedContactEmailLink.Text = item.SubItems[6].Text;
                    DetailedAttendeesLabel.Text = item.SubItems[7].Text;
                }
            }
            else
            {
                DetailedEventTimeLabel.Text = DetailedAttendeesLabel.Text = DetailedContactNameLabel.Text =
                    DetailedContactNumberLabel.Text = DetailedContactEmailLink.Text = "";
            }
        }

        private bool UserLoggedOut = false;
        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            UserLoggedOut = false;
            this.Close();
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            var about = new AboutBox();
            about.ShowDialog();
        }

        private void LogOutMenuItem_Click(object sender, EventArgs e)
        {
            UserLoggedOut = true;
            this.Close();
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Client.Disconnect();

            if (UserLoggedOut)
                LoginForm.Show();
            else
                Application.Exit();
        }

        private void AdjustEventListViewSize()
        {
            if (VolunteerEventsListView.Items.Count > 10)
                VolunteerEventsListView.Width = 476;
            else
                VolunteerEventsListView.Width = 459;
        }

        private void HideRegisteredEvents(bool Hidden = true)
        {
            if (Hidden)
            {
                foreach (ListViewItem item in VolunteerEventsListView.Items)
                {
                    if (RegisteredEventItems.Contains(item))
                        item.Remove();
                }
            }
            else
            {
                foreach (ListViewItem item in RegisteredEventItems)
                {
                    item.ForeColor = Color.Red;
                    VolunteerEventsListView.Items.Add(item);
                }
            }

            AdjustEventListViewSize();
        }

        private void HideRegisteredEventsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            HideRegisteredEvents(HideRegisteredEventsCheckBox.Checked);
        }

        private bool EventOverlap(EventInfo evt)
        {
            //var usersEvents = CurrentUser.RegisteredEvents
            return false;
        }

        //todo: duration check for added events to prevent overlap 
        private void VolunteerEventsListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {

            if (e.Item.ForeColor == Color.Red)
                e.Item.Checked = false;

            if (e.Item.Checked)
            {
                SelectedEventIds.Add(Int32.Parse(e.Item.SubItems[8].Text));
                SelectedEventsListBox.Items.Add(FormatSelctedItem(e.Item));
            }
            else
            {
                SelectedEventIds.Remove(Int32.Parse(e.Item.SubItems[8].Text));
                SelectedEventsListBox.Items.Remove(FormatSelctedItem(e.Item));
            }

            SetSelectedCount(SelectedEventsListBox.Items.Count);
            GenerateScheduleCheckBox.Enabled = ClearSelectedEventsButton.Enabled =
                RegisterForSelectedEventsButton.Enabled = (SelectedEventsListBox.Items.Count > 0);
        }

        private string FormatSelctedItem(ListViewItem item)
        {
            string date = item.SubItems[2].Text;
            string time = item.SubItems[3].Text;
            string name = item.SubItems[0].Text;

            return date + "  @  " + time + "\t" + name;
        }

        private void SetSelectedCount(int count)
        {
            var currentCount = string.Format("({0}) Selected ", count);
            currentCount += (count > 1 || count == 0) ? "Events" : "Event";
            SelectedEventsCountLabel.Text = currentCount;
        }

        private void ClearSelectedEvents()
        {
            SelectedEventsListBox.Items.Clear();
            SelectedEventIds.Clear();
        }

        private void ContactNameLabel_Hover(object sender, EventArgs e)
        {
            if (DetailedContactNameLabel.Text.Contains("..."))
            {
                var name = VolunteerEventsListView.SelectedItems[0].SubItems[4].Text;
                MainToolTip.Show(name, DetailedContactNameLabel);
            }
        }

        private void ContactEmailLabel_Hover(object sender, EventArgs e)
        {
            if (DetailedContactEmailLink.Text.Contains("..."))
            {
                var name = VolunteerEventsListView.SelectedItems[0].SubItems[6].Text;
                MainToolTip.Show(name, DetailedContactEmailLink);
            }
        }

        private void ClearSelectedEventsButton_Click(object sender, EventArgs e)
        {
            ClearSelectedEvents();
            foreach (ListViewItem item in VolunteerEventsListView.SelectedItems)
            {
                item.Checked = false;
            }
        }

        private void RegisterForSelectedEventsButton_Click(object sender, EventArgs e)
        {
            int[] eventIds = new int[VolunteerEventsListView.CheckedItems.Count];
            int index = 0;
            foreach (ListViewItem item in VolunteerEventsListView.CheckedItems)
            {
                eventIds[index] = Int32.Parse(item.SubItems[8].Text);
                index++;
            }

            ClearSelectedEvents();
            foreach (ListViewItem item in VolunteerEventsListView.CheckedItems)
            {
                item.Checked = false;
            }

            Client.RegisterUserForEvent(CurrentUser.Id, eventIds);
            if (GenerateScheduleCheckBox.Checked)
            {
                var events = EventList.FindAll(x => eventIds.Contains(x.Id)).ToArray();
                HtmlBuilder.GenerateRegisteredReport(events, UserList);
                var previewForm = new PreviewReportForm(0);
                previewForm.ShowDialog();
            }
            else
            {
                MessageBox.Show(string.Format("You registered for {0} events!", eventIds.Length));
            }
        }

        private void EventNameHeaderButton_Click(object sender, EventArgs e)
        {
            //if (CurrentOrder == OrderBy.Name)
            //    LoadCurrentEvents(HideRegisteredEventsCheckBox.Checked, OrderBy.NameDesc);
            //else
            //    LoadCurrentEvents(HideRegisteredEventsCheckBox.Checked, OrderBy.Name);

            VolunteerEventsListView.Focus();
        }

        private void EventLocationHeaderButton_Click(object sender, EventArgs e)
        {
            //if (CurrentOrder == OrderBy.Location)
            //    LoadCurrentEvents(HideRegisteredEventsCheckBox.Checked, OrderBy.LocationDesc);
            //else
            //    LoadCurrentEvents(HideRegisteredEventsCheckBox.Checked, OrderBy.Location);
            VolunteerEventsListView.Focus();
        }

        private void EventDateHeaderButton_Click(object sender, EventArgs e)
        {
            //if (CurrentOrder == OrderBy.Date)
            //    LoadCurrentEvents(HideRegisteredEventsCheckBox.Checked, OrderBy.DateDesc);
            //else
            //    LoadCurrentEvents(HideRegisteredEventsCheckBox.Checked);

            VolunteerEventsListView.Focus();
        }

        private void ShowCreateUserForm()
        {
            var createUserForm = new CreateUserForm(Client);
            createUserForm.ShowDialog();
        }

        private void ShowUserForm(UserInfo usr, bool adminEdit = false, bool self = false)
        {
            var viewUserForm = new ViewUserForm(usr, adminEdit, Client, self);
            viewUserForm.ShowDialog(this);
        }

        private void ShowCreateEventForm()
        {
            var createEventForm = new CreateEventForm(CurrentUser, Client);
            createEventForm.ShowDialog();
        }

        private void ShowEventForm(EventInfo evt, bool readOnly = false)
        {
            var eventForm = new ViewEventForm(evt, readOnly, Client);
            eventForm.ShowDialog();
        }

        private void UpdateInformationMenuItem_Click(object sender, EventArgs e)
        {
            ShowUserForm(CurrentUser, CurrentUser.IsAdmin, true);
        }

        private void AdminCreateUserButton_Click(object sender, EventArgs e)
        {
            ShowCreateUserForm();
        }

        private void CreateNewEventButton_Click(object sender, EventArgs e)
        {
            ShowCreateEventForm();
        }

        private delegate void AdminGetUsersCallBack();
        private void GetAllUsersToManage()
        {
            if (AdminUserListView.InvokeRequired)
            {
                var callback = new AdminGetUsersCallBack(GetAllUsersToManage);
                Invoke(callback);
            }
            else
            {
                AdminUserListView.Items.Clear();
                foreach (var user in UserList)
                {
                    var item = new ListViewItem(user.Username);
                    item.SubItems.Add(string.Format("{1}, {0}",
                        user.FullName.Item1, user.FullName.Item2));
                    item.SubItems.Add(user.CreatedEvents.Count.ToString());
                    item.SubItems.Add(user.RegisteredEvents.Count.ToString());
                    item.SubItems.Add(user.Id.ToString());
                    item.SubItems.Add(user.IsAdmin.ToString());
                    AdminUserListView.Items.Add(item);
                }

                AdminUserListView.Items[0].Selected = true;
            }
        }

        private delegate void AdminGetEventsCallBack();
        private void GetAllEventsToManage()
        {
            if (AdminEventListView.InvokeRequired)
            {
                var callback = new AdminGetEventsCallBack(GetAllEventsToManage);
                Invoke(callback);
            }
            else
            {
                List<ListViewItem> temp = new List<ListViewItem>();
                AdminEventListView.Items.Clear();
                foreach (var evt in EventList)
                {
                    var creator = GetUserFromId(evt.CreatorId);
                    var item = new ListViewItem(evt.Name);
                    item.SubItems.Add(evt.Date);
                    item.SubItems.Add(creator.Username);
                    item.SubItems.Add(evt.Id.ToString());

                    if (!evt.Current)
                        temp.Add(item);
                    else
                        AdminEventListView.Items.Add(item);
                }

                foreach(ListViewItem item in temp)
                {
                    item.ForeColor = Color.Red;
                    AdminEventListView.Items.Add(item);
                }

                AdminEventListView.Items[0].Selected = true;
            }
        }

        private void UpdateAdminCounts()
        {
            AdminTotalUsersLabel.Invoke((MethodInvoker)(() =>
                AdminTotalUsersLabel.Text = string.Format("Total Users: {0}", UserList.Count)));
            AdminTotalCountLabel.Invoke((MethodInvoker)(() =>
                AdminTotalCountLabel.Text = string.Format("Total Events: {0}", EventList.Count)));
            AdminCurrentEventCountLabel.Invoke((MethodInvoker)(() =>
                AdminCurrentEventCountLabel.Text = string.Format("Current Events: {0}",
                EventList.FindAll(x => x.Current).Count)));
        }

        private void HideOldUserCreatedEvents(bool hidden = true)
        {
            if (hidden)
            {
                foreach (ListViewItem item in OldUserCreatedEventItems)
                {
                    if (UserCreatedEventsListView.Items.Contains(item))
                        item.Remove();
                }
            }
            else
            {
                foreach (ListViewItem item in OldUserCreatedEventItems)
                {
                    item.ForeColor = Color.Red;
                    UserCreatedEventsListView.Items.Add(item);
                }
            }
        }

        private List<ListViewItem> OldUserCreatedEventItems;
        private delegate void LoadUserCreatedCallBack();
        private void LoadUserCreatedEvents()
        {
            OldUserCreatedEventItems = new List<ListViewItem>();
            if (UserCreatedEventsListView.InvokeRequired)
            {
                var callBack = new LoadUserCreatedCallBack(LoadUserCreatedEvents);
                Invoke(callBack);
            }
            else
            {
                UserCreatedEventsListView.Items.Clear();
                var createdEvents = CurrentUser.CreatedEvents;
                foreach (var id in createdEvents)
                {
                    EventInfo evt = GetEventFromId(id);
                    var item = new ListViewItem(evt.Name);
                    item.SubItems.Add(evt.Date);
                    item.SubItems.Add(evt.StartTime.ToString("hh:mm tt") +
                        " - " + evt.EndTime.ToString("hh:mm tt"));
                    item.SubItems.Add(evt.RegisteredUsers.Count.ToString());
                    item.SubItems.Add(evt.Id.ToString());

                    if (!evt.Current)
                        OldUserCreatedEventItems.Add(item);
                    else
                        UserCreatedEventsListView.Items.Add(item);
                }

                if(ShowOldCreatedEventsCheckBox.Checked)
                {
                    foreach(ListViewItem item in OldUserCreatedEventItems)
                    {
                        item.ForeColor = Color.Red;
                        UserCreatedEventsListView.Items.Add(item);
                    }
                }

                if (UserCreatedEventsListView.Items.Count == 0)
                {
                    UserCreatedEventsListView.Enabled = false;
                    UserCreatedEventsListView.Items.Add(new ListViewItem("You haven't created any events"));
                }
                else
                {
                    UserCreatedEventsListView.Enabled = true;
                }

                UserCreatedEventsListView.Items[0].Selected = true;
            }
        }

        private void HideOldUserRegisteredEvents(bool hidden = true)
        {
            if (hidden)
            {
                foreach (ListViewItem item in OldUserRegisteredEventItems)
                {
                    if (UserRegisteredEventsListView.Items.Contains(item))
                        item.Remove();
                }
            }
            else
            {
                foreach (ListViewItem item in OldUserRegisteredEventItems)
                {
                    item.ForeColor = Color.Red;
                    UserRegisteredEventsListView.Items.Add(item);
                }
            }
        }

        private List<ListViewItem> OldUserRegisteredEventItems;
        private delegate void LoadUserRegisteredCallBack();
        private void LoadUserRegisteredEvents()
        {
            OldUserRegisteredEventItems = new List<ListViewItem>();
            if (UserRegisteredEventsListView.InvokeRequired)
            {
                var callback = new LoadUserRegisteredCallBack(LoadUserRegisteredEvents);
                Invoke(callback);
            }
            else
            {
                UserRegisteredEventsListView.Items.Clear();
                var registeredEvents = CurrentUser.RegisteredEvents.FindAll(
                    x => !CurrentUser.CreatedEvents.Contains(x));
                foreach (var id in registeredEvents)
                {
                    EventInfo evt = GetEventFromId(id);
                    var creator = GetUserFromId(evt.CreatorId);
                    var item = new ListViewItem(evt.Name);
                    item.SubItems.Add(evt.Date);
                    item.SubItems.Add(creator.Username);
                    item.SubItems.Add(evt.Id.ToString());

                    if (!evt.Current)
                        OldUserRegisteredEventItems.Add(item);
                    else
                        UserRegisteredEventsListView.Items.Add(item);
                }

                if (ShowOldRegisteredEventsCheckBox.Checked)
                {
                    foreach (ListViewItem item in OldUserRegisteredEventItems)
                    {
                        item.ForeColor = Color.Red;
                        UserRegisteredEventsListView.Items.Add(item);
                    }
                }

                if (UserRegisteredEventsListView.Items.Count == 0)
                {
                    UserRegisteredEventsListView.Items.Add(new ListViewItem("You aren't registered for any events"));
                    UserRegisteredEventsListView.Enabled = false;
                }
                else
                {
                    UserRegisteredEventsListView.Enabled = true;
                }
                UserRegisteredEventsListView.Items[0].Selected = true;
            }
        }

        private void UserCreatedEventsListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            EditSelectedEventButton.Enabled = false;
            DeleteSelectedEventButton.Enabled = false;

            if (e.Item.Text == "You haven't created any events")
            {
                CreatedEventTime.Text = "";
                CreatedEventAttendees.Text = "";
                e.Item.Selected = false;
                return;
            }

            if (e.IsSelected)
            {
                CreatedEventTime.Text = e.Item.SubItems[2].Text;
                CreatedEventAttendees.Text = e.Item.SubItems[3].Text;
                EditSelectedEventButton.Enabled = true;
                DeleteSelectedEventButton.Enabled = e.Item.ForeColor != Color.Red;
            }
        }

        private void UserRegisteredEventsListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ViewSelectedEventButton.Enabled = false;
            UnregisterFromEventButton.Enabled = false;

            if (e.Item.Text == "You aren't registered for any events")
            {
                RegisteredEventContactEmail.Text = "";
                e.Item.Selected = false;
                return;
            }

            if (e.IsSelected)
            {
                RegisteredEventContactEmail.Text = e.Item.SubItems[2].Text;
                ViewSelectedEventButton.Enabled = true;
                UnregisterFromEventButton.Enabled = e.Item.ForeColor != Color.Red;
            }
        }

        private void EditSelectedEventButton_Click(object sender, EventArgs e)
        {
            var selectedId = Int32.Parse(UserCreatedEventsListView.SelectedItems[0].SubItems[4].Text);
            ShowEventForm(GetEventFromId(selectedId));
        }

        private void ViewSelectedEventButton_Click(object sender, EventArgs e)
        {
            var selectedId = Int32.Parse(UserRegisteredEventsListView.SelectedItems[0].SubItems[3].Text);
            ShowEventForm(GetEventFromId(selectedId), true);
        }

        private void AdminUserListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            AdminEditUserButton.Enabled = AdminUserListView.SelectedItems.Count > 0;
            AdminDeleteUserButton.Enabled = AdminUserListView.SelectedItems.Count > 0
                && e.Item.SubItems[5].Text != "True";

            if (e.IsSelected)
            {
                AdminUserCreatedEvents.Text = e.Item.SubItems[2].Text;
                AdminUserRegisteredEvents.Text = e.Item.SubItems[3].Text;
            }
        }

        private void AdminEventListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            AdminEditEventButton.Enabled = AdminEventListView.SelectedItems.Count > 0;
            AdminDeleteEventButton.Enabled = AdminEventListView.SelectedItems.Count > 0;
            if (e.IsSelected)
            {
                AdminEventContactEmail.Text = e.Item.SubItems[2].Text;
            }
        }

        private void DeleteSelectedEventButton_Click(object sender, EventArgs e)
        {
            var selectedId = Int32.Parse(UserCreatedEventsListView.SelectedItems[0].SubItems[4].Text);
            Client.DeleteSelectedEvent(selectedId);
        }

        private void UnregisterFromEventButton_Click(object sender, EventArgs e)
        {
            var selectedId = Int32.Parse(UserRegisteredEventsListView.SelectedItems[0].SubItems[3].Text);
            Client.UnregisterUserForEvent(CurrentUser.Id, selectedId);
        }

        private void GenerateEventScheduleButton_Click(object sender, EventArgs e)
        {
            var events = (IncludeCreatedEventsCheckBox.Checked) ?
                EventList.FindAll(x => CurrentUser.RegisteredEvents.Contains(x.Id)).ToArray() :
                EventList.FindAll(x => CurrentUser.RegisteredEvents.Contains(x.Id) && !CurrentUser.CreatedEvents.Contains(x.Id)).ToArray();

            HtmlBuilder.GenerateUserSchedule(CurrentUser, events, UserList);
            var previewForm = new PreviewReportForm(1);
            previewForm.ShowDialog();
        }

        private void AdminEditUserButton_Click(object sender, EventArgs e)
        {
            var selectedId =
                Int32.Parse(AdminUserListView.SelectedItems[0].SubItems[4].Text);
            UserInfo selectedUser = GetUserFromId(selectedId);

            ShowUserForm(selectedUser, true, selectedUser.Username == CurrentUser.Username);
        }

        private void AdminEditEventButton_Click(object sender, EventArgs e)
        {
            var selectedId =
                Int32.Parse(AdminEventListView.SelectedItems[0].SubItems[3].Text);
            ShowEventForm(GetEventFromId(selectedId));
        }

        private void AdminDeleteUserButton_Click(object sender, EventArgs e)
        {
            var selectedId = Int32.Parse(AdminUserListView.SelectedItems[0].SubItems[4].Text);
            Client.DeletedSelectedUser(selectedId);
        }

        private void AdminDeleteEventButton_Click(object sender, EventArgs e)
        {
            var selectedId = Int32.Parse(AdminEventListView.SelectedItems[0].SubItems[3].Text);
            Client.DeleteSelectedEvent(selectedId);
        }

        private void AdminGnerateReportButton_Click(object sender, EventArgs e)
        {
            HtmlBuilder.GenerateAdminReport(UserList, EventList);
            var previewForm = new PreviewReportForm(2);
            previewForm.ShowDialog();
        }

        private void ShowSendEmailForm(string recipient)
        {
            var EmailForm = new SendEmailForm(recipient, CurrentUser.Username);
            EmailForm.ShowDialog();
        }

        private void ContactEmailAddress_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var recipient = (sender as LinkLabel).Text;
            ShowSendEmailForm(recipient);
        }

        private void ShowOldCreatedEventsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            HideOldUserCreatedEvents(!ShowOldCreatedEventsCheckBox.Checked);
        }

        private void ShowOldRegisteredEventsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            HideOldUserRegisteredEvents(!ShowOldRegisteredEventsCheckBox.Checked);
        }
    }
}
