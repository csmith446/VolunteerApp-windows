#define DEBUG
using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Client;
using VolunteerAppCommonLib;

namespace VolunteerAppClient
{
    internal class ClientService : IVolunteerClient
    {
        private static IScsServiceClient<IVolunteerServer> Server;
#if DEBUG
        private const string SERVER_IP = "10.0.0.2";
#else
        private const string SERVER_IP = "76.20.234.235";
#endif
        private const int SERVER_PORT = 31415;
        private LoginForm LoginWindow;
        private MainVolunteerForm MainWindow;

        public ClientService(LoginForm Login)
        {
            LoginWindow = Login;

            Server = ScsServiceClientBuilder.CreateClient<IVolunteerServer>(
                new ScsTcpEndPoint(SERVER_IP, SERVER_PORT), this);
            Server.ConnectTimeout = 1000;
            Server.Timeout = 5000;
        }

        public bool LogInUser(string username, string password, out UserInfo user)
        {
            Server.Connect();
            user = null;

            if (Server.ServiceProxy.CheckCredentials(username, MD5Hasher.GetHashedValue(password)))
            {
                user = Server.ServiceProxy.GetLoggedOnUser();
                return true;
            }

            Server.Disconnect();
            return false;
        }

        public void Disconnect()
        {
            Server.ServiceProxy.UserLoggedOut();
            Server.Disconnect();
        }

        public void DeleteSelectedEvent(int eventId)
        {
            Server.ServiceProxy.DeleteSelectedEvent(eventId);
        }

        public void DeletedSelectedUser(int userId)
        {
            Server.ServiceProxy.DeleteSelectedUser(userId);
        }

        public void CreateNewEvent(int userId, string name, string date, string time,
            string duration, string location)
        {
            Server.ServiceProxy.CreateNewEvent(userId, name, date, time,
                duration, location);
        }

        public void UpdateUserInfo(int userId, string username, string firstName, string lastName,
            bool isAdmin, string phoneNumber, string hashedPassword = null)
        {
            Server.ServiceProxy.UpdateUserInfo(userId, username, firstName,
                lastName, isAdmin, phoneNumber, hashedPassword);
        }

        public void UpdateEventInfo(int eventId, string name, string date, string time,
            string duration, string location)
        {
            Server.ServiceProxy.UpdateEventInfo(eventId, name, date,
                time, duration, location);
        }

        public void RegisterNewUser(string username, string password, string firstName,
            string lastName, string phoneNumber, bool admin)
        {
            Server.ServiceProxy.RegisterNewUser(username, password, firstName, lastName,
                phoneNumber, admin);
        }

        public bool IsEmailInUse(string emailAddress)
        {
            return Server.ServiceProxy.IsEmailInUse(emailAddress);
        }

        public void UnregisterUserForEvent(int userId, int eventId)
        {
            Server.ServiceProxy.UnregisterUserFromEvent(userId, eventId);
        }

        public void RegisterUserForEvent(int userId, int[] eventIds)
        {
            Server.ServiceProxy.RegisterUserForEvent(userId, eventIds);
        }

        public void GetDatabaseInfo(out List<EventInfo> events, out List<UserInfo> users, MainVolunteerForm main = null)
        {
            events = Server.ServiceProxy.GetUpdatedEvents();
            users = Server.ServiceProxy.GetUpdatedUsers();
            if (main != null) MainWindow = main;
        }

        public List<EventInfo> GetUpdatedEvents()
        {
            return Server.ServiceProxy.GetUpdatedEvents();
        }

        public List<UserInfo> GetUpdatedUsers()
        {
            return Server.ServiceProxy.GetUpdatedUsers();
        }

        #region IVolunteerClient Methods
        //methods the server would call on the client
        public void PushDatabaseChanges(List<EventInfo> events, List<UserInfo> users)
        {
            MainWindow.UpdateLists(events, users);
        }

        public void ServerShutDown()
        {

        }
        #endregion

    }
}
