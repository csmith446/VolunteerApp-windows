using System.Collections.Generic;
using Hik.Communication.ScsServices.Service;

namespace VolunteerAppCommonLib
{
    [ScsService(Version = "1.0.0.0")]
    public interface IVolunteerServer
    {
        //methods for the client to call on the server
        void CheckCredentials(string username, string password);
        void RegisterUserForEvent(int user_id, int event_id);
    }
}
