using System;
using System.Collections.Generic;
using Hik.Communication.ScsServices.Client;
using VolunteerAppCommonLib;

namespace VolunteerAppClient
{
    internal class ClientService : IVolunteerClient
    {
        private IScsServiceClient<IVolunteerServer> _Server;
        private LoginForm MainLogin;
        private UserInfo _CurrentUser;

        public ClientService(LoginForm Login)
        {
            MainLogin = Login;
        }

        #region IVolunteerClient Methods
        public void ShowMainWindow(bool result)
        {
            MainLogin.ShowLoginResult(result);
        }
        #endregion
    }
}
