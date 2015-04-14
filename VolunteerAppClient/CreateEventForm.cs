using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hik.Communication.Scs.Communication.EndPoints.Tcp;
using Hik.Communication.ScsServices.Client;
using VolunteerAppCommonLib;

namespace VolunteerAppClient
{
    public partial class CreateEventForm : Form
    {        
        private IScsServiceClient<IVolunteerServer> Server;
        private UserInfo Creator;

        //todo: input validation, error providers, create event query
        //todo: dialog result

        public CreateEventForm(UserInfo currentUser,
            IScsServiceClient<IVolunteerServer> server)
        {
            InitializeComponent();
            Server = server;
            Creator = currentUser;
        }

        private void CancelNewEventButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
