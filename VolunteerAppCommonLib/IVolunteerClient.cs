using Hik.Communication.ScsServices.Service;
using System.Collections.Generic;
using VolunteerAppCommonLib;

namespace VolunteerAppCommonLib
{
    public interface IVolunteerClient
    {
        //methods for the server to call on the client
        void PushDatabaseChanges(List<EventInfo> events, List<UserInfo> users);
    }
}
