using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Hik.Collections;
using Hik.Communication.ScsServices.Service;
using VolunteerAppCommonLib;

namespace VolunteerAppServer
{
    internal class ServerService : ScsService, IVolunteerServer
    {
        private readonly ThreadSafeSortedList<long, VolunteerClient> ConnectedClients;
        private List<EventInfo> EventInfoList;
        private List<UserInfo> UserInfoList;
        private VolunteerServer ServerForm;

        public ServerService(VolunteerServer serverForm)
        {
            ServerForm = serverForm;
            ConnectedClients = new ThreadSafeSortedList<long, VolunteerClient>();
        }

        #region VolunteerClient class
        private sealed class VolunteerClient
        {
            public IScsServiceClient Client { get; private set; }
            public IVolunteerClient ClientProxy { get; private set; }
            public UserInfo User { get; private set; }

            public VolunteerClient(IScsServiceClient client, IVolunteerClient clientProxy, UserInfo userInfo)
            {
                Client = client;
                ClientProxy = clientProxy;
                User = userInfo;
            }
        }
        #endregion

        public void GetDataFromDatabase()
        {
            UserInfoList = DatabaseManager.GetAllUsersFromDb();
            EventInfoList = DatabaseManager.GetAllEventsFromDb();

            foreach (var evt in EventInfoList)
            {
                evt.RegisteredUsers = DatabaseManager.GetRegisteredIdsForEvent(evt.Id);
            }

            foreach (var user in UserInfoList)
            {
                user.RegisteredEvents = DatabaseManager.GetRegisteredEventsForUser(user.Id);
                user.CreatedEvents = DatabaseManager.GetCreatedEventsForUser(user.Id);
            }

            Task pushData = new Task(() =>
            {
                foreach (var client in ConnectedClients.GetAllItems())
                {
                    client.ClientProxy.PushDatabaseChanges(EventInfoList, UserInfoList);
                }
            });
            pushData.Start();
        }

        #region IVotingService methods
        public List<EventInfo> GetUpdatedEvents()
        {
            return EventInfoList;
        }

        public List<UserInfo> GetUpdatedUsers()
        {
            return UserInfoList;
        }

        public UserInfo GetLoggedOnUser()
        {
            var client = ConnectedClients[CurrentClient.ClientId];
            var userInfo = client.User;

            return userInfo;
        }

        public void DeleteSelectedEvent(int eventId)
        {
            DatabaseManager.DeleteEventFromDatabase(eventId);
            GetDataFromDatabase();
        }

        public void DeleteSelectedUser(int userId)
        {
            DatabaseManager.DeleteUserFromDatabase(userId);
            GetDataFromDatabase();
        }

        public bool CheckCredentials(string username, string password)
        {
            if (DatabaseManager.IsValidCredentials(username, password))
            {
                var client = CurrentClient;
                var clientProxy = client.GetClientProxy<IVolunteerClient>();
                var user = UserInfoList.Find(x => x.Username == username);
                var volunteerClient = new VolunteerClient(client, clientProxy, user);

                ConnectedClients[client.ClientId] = volunteerClient;
                ServerForm.LogMessage("Connection granted : " + username);
                ServerForm.CurrentUsersStatusLabel.Text = "Connected Users : " + ConnectedClients.Count.ToString();
                return true;
            }
            else
                return false;
        }

        public void UserLoggedOut()
        {
            var User = ConnectedClients[CurrentClient.ClientId].User;
            ServerForm.LogMessage("Conncetion closed : " + User.Username);

            ConnectedClients.Remove(CurrentClient.ClientId);
            ServerForm.CurrentUsersStatusLabel.Text = "Connected Users : " + ConnectedClients.Count.ToString();
        }

        public void UpdateUserInfo(int userId, string username, string firstName, string lastName,
            bool isAdmin, string phoneNumber, string hashedPassword = null)
        {
            DatabaseManager.UpdateUserInformation(userId, username, firstName,
                lastName, isAdmin, phoneNumber, hashedPassword);
            GetDataFromDatabase();
        }

        public void UpdateEventInfo(int eventId, string name, string date, string time,
            string duration, string location)
        {
            DatabaseManager.UpdateEventInformation(eventId, name, date,
                time, duration, location);
            GetDataFromDatabase();
        }

        public void CreateNewEvent(int userId, string name, string date, string time,
            string duration, string location)
        {
            DatabaseManager.CreateNewEvent(userId, name, date, time,
                duration, location);
            GetDataFromDatabase();
        }

        public void RegisterNewUser(string username, string password, string firstName,
            string lastName, string phoneNumber, bool admin)
        {
            DatabaseManager.RegisterNewUser(username, password, firstName, lastName,
                phoneNumber, admin);
            GetDataFromDatabase();
        }

        public bool IsEmailInUse(string emailAddress)
        {
            return DatabaseManager.IsEmailInUse(emailAddress);
        }

        public void UnregisterUserFromEvent(int userId, int eventId)
        {
            DatabaseManager.UnregisterUserForEvent(userId, eventId);
            GetDataFromDatabase();
        }

        public void RegisterUserForEvent(int userId, int[] eventIds)
        {
            DatabaseManager.RegisterUserForEvent(userId, eventIds);
            GetDataFromDatabase();
        }
        #endregion

        #region private methods

        #endregion
    }
}
