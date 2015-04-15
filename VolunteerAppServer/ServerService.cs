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
        private List<EventInfo> AllEventsList;
        private List<UserInfo> AllUsersList;
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

        #region IVotingService methods
        public void GetCurrentLists()
        {
            AllUsersList = DatabaseManager.GetAllUsers();
            AllEventsList = DatabaseManager.GetAllEvents();

            foreach (var usr in AllUsersList)
            {
                foreach (var evt in AllEventsList)
                {
                    if (evt.Creator.Id == usr.Id)
                    {
                        usr.CreatedEvents.Add(evt);
                        usr.RegisteredEvents.Add(evt);

                        evt.RegisteredUsers.Add(usr.Id);
                        evt.UpdateCreator(usr);
                    }
                    else if (DatabaseManager.IsUserRegisteredForEvent(evt.Id, usr.Id))
                    {
                        usr.RegisteredEvents.Add(evt);
                        evt.RegisteredUsers.Add(usr.Id);
                    }
                }
            }
        }

        public List<EventInfo> GetUpdatedEvents()
        {
            return AllEventsList;
        }

        public List<UserInfo> GetUpdatedUsers()
        {
            return AllUsersList;
        }

        public UserInfo GetLoggedOnUser()
        {
            var client = ConnectedClients[CurrentClient.ClientId];
            var userInfo = client.User;

            return userInfo;
        }

        public bool CheckCredentials(string username, string password)
        {
            if (DatabaseManager.IsValidCredentials(username, password))
            {
                var client = CurrentClient;
                var clientProxy = client.GetClientProxy<IVolunteerClient>();
                var user = AllUsersList.Find(x => x.Username == username);
                var volunteerClient = new VolunteerClient(client, clientProxy, user);

                ConnectedClients[client.ClientId] = volunteerClient;
                ServerForm.LogMessage("Connection granted : " + username);
                ServerForm.CurrentUsersStatusLabel.Text = "Connected Users : " + ConnectedClients.Count.ToString();
                return true;
            }
            else return false;
        }

        public void UserLoggedOut()
        {
            var User = ConnectedClients[CurrentClient.ClientId].User;
            ServerForm.LogMessage("Conncetion closed : " + User.Username);

            ConnectedClients.Remove(CurrentClient.ClientId);
            ServerForm.CurrentUsersStatusLabel.Text = "Connected Users : " + ConnectedClients.Count.ToString();
        }

        public void RegisterNewUser(string username, string password, string firstName,
            string lastName, string phoneNumber, bool admin)
        {
            DatabaseManager.RegisterNewUser(username, password, firstName, lastName,
                phoneNumber, admin);
        }

        public bool IsEmailInUse(string emailAddress)
        {
            return DatabaseManager.IsEmailInUse(emailAddress);
        }

        public void RegisterUserForEvent(int user_id, int event_id)
        {
            DatabaseManager.RegisterUserForEvent(user_id, event_id);
        }

        public bool IsUserRegisteredForEvent(int eventId, int userId)
        {
            return DatabaseManager.IsUserRegisteredForEvent(eventId, userId);
        }

        public bool IsUserAdmin(int id)
        {
            return DatabaseManager.IsUserAdmin(id);
        }

        public int GetContactIDForUser(UserInfo user)
        {
            return DatabaseManager.GetContactIDForUser(user);
        }
        #endregion

        #region private methods

        #endregion
    }
}
