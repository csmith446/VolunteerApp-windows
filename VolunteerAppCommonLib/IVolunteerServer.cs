using System;
using System.Collections.Generic;
using Hik.Communication.ScsServices.Service;

namespace VolunteerAppCommonLib
{
    [ScsService(Version = "1.0.0.0")]
    public interface IVolunteerServer
    {
        //methods for the client to call on the server
        bool CheckCredentials(string username, string password);
        UserInfo GetLoggedOnUser();
        List<EventInfo> GetUpdatedEvents();
        List<UserInfo> GetUpdatedUsers();
        void UserLoggedOut();
        void UpdateUserInfo(int userId, string username, string firstName, string lastName,
            bool isAdmin, string phoneNumber, string hashedPassword = null);
        void UpdateEventInfo(int eventId, string name, string date, string time,
            string duration, string location);
        void CreateNewEvent(int userId, string name, string date, string time,
            string duration, string location);


        void RegisterNewUser(string username, string password, string firstName,
            string lastName, string phoneNumber, bool admin);
        bool IsEmailInUse(string emailAddress);
        void RegisterUserForEvent(int user_id, int event_id);
        bool IsUserRegisteredForEvent(int eventId, int userId);
        bool IsUserAdmin(int id);
        int GetContactIDForUser(UserInfo user);
    }
}
