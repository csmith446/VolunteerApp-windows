using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VolunteerAppClient
{
    public partial class PreviewReportForm : Form
    {
        private const string REGISTERED_EVENTS_PATH = @"..\..\Reports\UserRegisteredReport_";
        private const string USER_SCHEDULE_PATH = @"..\..\Reports\UserEventSchedule_";
        private const string ADMIN_USER_PATH = @"..\..\Reports\AdminUsersReport_";
        private const string ADMIN_EVENT_PATH = @"..\..\Reports\AdminEventsReport_";
        private const int URR = 0, UER = 1, AUR = 2, AER = 3;

        public PreviewReportForm(int mode)
        {
            InitializeComponent();

            var path = "";
            switch(mode)
            {
                case URR:
                    path = REGISTERED_EVENTS_PATH + DateTime.Now.ToString("MMddyyy.html");
                    break;
                case UER:
                    path = USER_SCHEDULE_PATH + DateTime.Now.ToString("MMddyyy.html");
                    break;
                case AUR:
                    path = ADMIN_USER_PATH + DateTime.Now.ToString("MMddyyy.html");
                    break;
                case AER:
                    path = ADMIN_EVENT_PATH + DateTime.Now.ToString("MMddyyy.html");
                    break;
            }

            var file = new FileInfo(path);
            ReportViewerBrowser.Url = new Uri(string.Format("file:///{0}", path));
        }

        private void ClosePreviewButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PrintButton_Click(object sender, EventArgs e)
        {

        }
    }
}
