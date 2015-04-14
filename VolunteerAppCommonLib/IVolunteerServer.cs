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

        void RegisterNewUser(string username, string password, string firstName,
            string lastName, string phoneNumber, bool admin);
        bool IsEmailInUse(string emailAddress);
        void RegisterUserForEvent(int user_id, int event_id);
        bool IsUserRegisteredForEvent(int eventId, int userId);
        bool IsUserAdmin(int id);
        int GetContactIDForUser(UserInfo user);
    }
}
