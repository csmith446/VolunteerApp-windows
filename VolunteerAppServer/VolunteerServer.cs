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
using Hik.Communication.ScsServices.Service;
using VolunteerAppCommonLib;

namespace VolunteerAppServer
{
    public partial class VolunteerServer : Form
    {
        private IScsServiceApplication Server;
        private ServerService ServerSvc;

        public VolunteerServer()
        {
            InitializeComponent();
            Server = ScsServiceBuilder.CreateService(new ScsTcpEndPoint(31415));
            ServerSvc = new ServerService(this);

            Server.AddService<IVolunteerServer, ServerService>(ServerSvc);
            Server.ClientConnected += Server_ClientConnected;
            Server.ClientDisconnected += Server_ClientDisconnected;

        }

        private delegate void LogMessageCallBack(string msg);
        public void LogMessage(string msg)
        {
            if (ServerLogListBox.InvokeRequired)
            {
                LogMessageCallBack callBack = new LogMessageCallBack(LogMessage);
                Invoke(callBack, new object[] { msg });
            }
            else
            {
                ServerLogListBox.Items.Add(msg);
            }

            ClearLogButton.Enabled = ServerLogListBox.Items.Count > 0;
        }

        private void CloseServer()
        {
            //close or hide the window
        }

        private void ShowAboutWindow()
        {
            //show about window
        }

        void Server_ClientDisconnected(object sender, ServiceClientEventArgs e)
        {
            //LogMessage("Connection ended");
        }

        void Server_ClientConnected(object sender, ServiceClientEventArgs e)
        {
            //LogMessage("Connection requested...");
        }

        private void ControlButton_Click(object sender, EventArgs e)
        {
            if (ControlButton.Text == "Start")
            {
                Server.Start();
                LogMessage(">> Server Started");
                ServerStatusLabel.Text = "Server : Loading...";
                Task getLists = new Task(() =>
                {
                    LogMessage(">> Retrieving data from database...");
                    ServerSvc.GetDataFromDatabase();
                    LogMessage(">> Done. Listening for connections...");
                    ServerStatusLabel.Text = "Server : Running";
                });
                getLists.Start();

                ControlButton.Text = "Stop";
                ControlButton.BackColor = Color.LightCoral;
            }
            else
            {
                Server.Stop();
                LogMessage(">> Server Stopped");
                ServerStatusLabel.Text = "Server : Stopped";

                ControlButton.Text = "Start";
                ControlButton.BackColor = Color.LightGreen;
            }
        }

        private void ClearLogButton_Click(object sender, EventArgs e)
        {
            ServerLogListBox.Items.Clear();
            ClearLogButton.Enabled = false;
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            CloseServer();
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            ShowAboutWindow();
        }

        private void VolunteerServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            Server.Stop();
        }
    }
}
