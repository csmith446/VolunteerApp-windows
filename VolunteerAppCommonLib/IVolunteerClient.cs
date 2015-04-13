using Hik.Communication.ScsServices.Service;

namespace VolunteerAppCommonLib
{
    public interface IVolunteerClient
    {
        //methods for the server to call on the client
        void ShowMainWindow(bool result);
    }
}
