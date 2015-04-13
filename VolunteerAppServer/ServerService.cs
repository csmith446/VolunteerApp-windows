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

        public ServerService()
        {
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
        public void CheckCredentials(string username, string password)
        {
            var client = CurrentClient;
            var clientProxy = client.GetClientProxy<IVolunteerClient>();

            if (DatabaseManager.IsValidCredentials(username, password))
            {
                var user = DatabaseManager.GetUserInformation(username);
                var volunteerClient = new VolunteerClient(client, clientProxy, user);
                ConnectedClients[client.ClientId] = volunteerClient;
                clientProxy.ShowMainWindow(true);
            }
            else
            {
                clientProxy.ShowMainWindow(false);
            }
        }

        public void RegisterUserForEvent(int user_id, int event_id)
        {
            ;
        }
        #endregion

        #region private methods

        #endregion
    }
}
