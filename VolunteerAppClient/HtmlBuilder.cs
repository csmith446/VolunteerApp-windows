using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerAppCommonLib;

namespace VolunteerAppClient
{
    public static class HtmlBuilder
    {
        private const string REGISTERED_EVENTS_PATH = @"..\..\Reports\UserRegisteredReport_";
        private const string USER_SCHEDULE_PATH = @"..\..\Reports\UserEventSchedule_";
        private const string ADMIN_USER_PATH = @"..\..\Reports\AdminUsersReport_";
        private const string ADMIN_EVENT_PATH = @"..\..\Reports\AdminEventsReport_";

        private const string REPORTS_CSS = "<style type='text/css'>table{margin-left:auto;" +
            "margin-right:auto;width:100%;font-family:sans-serif;}td, th{padding:5px;}" +
            ".right{text-align:right;}.bottom{font-size:small;border-bottom:1px solid Salmon;}.title{background" +
            "-color:Salmon;color:Black;height:30px;}.row{text-align:center;border-right:1px solid Salmon;border-" +
            "bottom:1px solid Salmon;}</style>";

        private static UserInfo GetUserFromId(int userId, List<UserInfo> userList)
        {
            return userList.Find(x => x.Id == userId) as UserInfo;
        }

        private static EventInfo GetEventFromId(int eventId, List<EventInfo> eventList)
        {
            return eventList.Find(x => x.Id == eventId) as EventInfo;
        }

        public static void GenerateRegisteredReport(EventInfo[] events, List<UserInfo> userList)
        {
            string path = REGISTERED_EVENTS_PATH + DateTime.Now.ToString("MMddyyy") + ".html";
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (StreamWriter stream = File.CreateText(path))
            {
                stream.Write("<!DOCTYPE html><html>");
                stream.Write(REPORTS_CSS);
                stream.Write("<body><table cellspacing='0' cellpadding='3'><tr class='title'>" +
                             "<th colspan='4'>Recently Registered Event Information</th></tr>");
                foreach (var evt in events)
                {
                    var creator = GetUserFromId(evt.CreatorId, userList);
                    var time = evt.StartTime.ToString("hh:mm tt") + " - " + evt.EndTime.ToString("hh:mm tt");
                    var name = string.Format("{0} {1}", creator.FullName.Item1, creator.FullName.Item2);

                    stream.Write("<tr><td class='row' rowspan='2' style='width:10%'>" + evt.Date + "</td>" +
                                 "<td style='width:40%'>" + evt.Name + "</td>" +
                                 "<td class='right' style='width:25%'>" + evt.Location + "</td>" +
                                 "<td class='right' style='width:25%'>" + time + "</td></tr>" +
                                 "<tr><td class='bottom'>Contact: " + name + "</td>" +
                                 "<td class='bottom right'>P: " + creator.PhoneNumber + "</td>" +
                                 "<td class='bottom right'>E: " + creator.Username + "</td></tr>");
                }
                stream.Write("</table></body></html>");
            }
        }

        public static void GenerateUsersReport(List<UserInfo> userList, List<EventInfo> eventList)
        {
            string path = ADMIN_USER_PATH + DateTime.Now.ToString("MMddyyy") + ".html";
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (StreamWriter stream = File.CreateText(path))
            {
                stream.Write("<!DOCTYPE html><html>");
                stream.Write(REPORTS_CSS);
                stream.Write("<body><table cellspacing='0' cellpadding='3'><tr class='title'>" +
                             "<th colspan='4'>User Information</th></tr>");
                foreach (var usr in userList)
                {
                    var name = string.Format("{0}, {1}", usr.FullName.Item2, usr.FullName.Item1);

                    stream.Write("<tr><td class='row' rowspan='2' style='width:20%'>" + name + "</td>" +
                                 "<td colspan='2' colspan='2' style='width:60%'>" + usr.Username + "</td>" +
                                 "<td class='right' style='width:20%'>Phone: " + usr.PhoneNumber + "</td></tr>" +
                                 "<tr><td class='bottom' style='width:15%'>Events Created: " + usr.CreatedEvents.Count.ToString() + "</td>" +
                                 "<td class='bottom'>Events Registered: " + usr.RegisteredEvents.Count.ToString() + "</td>" +
                                 "<td class='bottom right'>Administrator: " + ((usr.IsAdmin) ? "Yes" : "No") + "</td></tr>");
                }
                stream.Write("</table></body></html>");
            }
        }

        public static void GenerateEventsReport(List<UserInfo> userList, List<EventInfo> eventList)
        {
            string path = ADMIN_EVENT_PATH + DateTime.Now.ToString("MMddyyy") + ".html";
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (StreamWriter stream = File.CreateText(path))
            {
                stream.Write("<!DOCTYPE html><html>");
                stream.Write(REPORTS_CSS);
                stream.Write("<body><table cellspacing='0' cellpadding='3'><tr class='title'>" +
                             "<th colspan='5'>Event Information</th></tr>");
                foreach (var evt in eventList)
                {
                    var creator = GetUserFromId(evt.CreatorId, userList);
                    var time = evt.StartTime.ToString("hh:mm tt") + " - " + evt.EndTime.ToString("hh:mm tt");
                    var name = string.Format("{0} {1}", creator.FullName.Item1, creator.FullName.Item2);

                    stream.Write("<tr><td class='row' rowspan='2' style='width:15%'>" + evt.Date + "</td>" +
                                 "<td style='width:40%' colspan='2'>" + evt.Name + "</td>" +
                                 "<td class='right' style='width:25%'>" + evt.Location + "</td>" +
                                 "<td class='right' style='width:20%'>" + time + "</td></tr>" +
                                 "<tr><td class='bottom'>Creator: " + name + "</td>" +
                                 "<td class='bottom' style='width:20%'>Users Registered: " + evt.RegisteredUsers.Count.ToString() + "</td>" +
                                 "<td class='bottom right'>P: " + creator.PhoneNumber + "</td>" +
                                 "<td class='bottom right'>E: " + creator.Username + "</td></tr>");
                }
                stream.Write("</table></body></html>");
            }
        }

        public static void GenerateUserSchedule(UserInfo user, EventInfo[] events, List<UserInfo> userList)
        {
            string path = USER_SCHEDULE_PATH + DateTime.Now.ToString("MMddyyy") + ".html";
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (StreamWriter stream = File.CreateText(path))
            {
                var name = string.Format("{0} {1}", user.FullName.Item1, user.FullName.Item2);
                stream.Write("<!DOCTYPE html><html>");
                stream.Write(REPORTS_CSS);
                stream.Write("<body><table cellspacing='0' cellpadding='3'><tr class='title'>" +
                             "<th colspan='5'>Event Schedule for " + name + "</th></tr>");
                foreach (var evt in events)
                {
                    var creator = GetUserFromId(evt.CreatorId, userList);
                    var time = evt.StartTime.ToString("hh:mm tt") + " - " + evt.EndTime.ToString("hh:mm tt");

                    stream.Write("<tr><td class='row' rowspan='2' style='width:15%'>" + evt.Date + "</td>" +
                                 "<td style='width:40%' colspan='2'>" + evt.Name + "</td>" +
                                 "<td class='right' style='width:25%'>" + evt.Location + "</td>" +
                                 "<td class='right' style='width:20%'>" + time + "</td></tr>" +
                                 "<tr><td class='bottom'>Creator: " + name + "</td>" +
                                 "<td class='bottom' style='width:20%'>Users Registered: " + evt.RegisteredUsers.Count.ToString() + "</td>" +
                                 "<td class='bottom right'>P: " + creator.PhoneNumber + "</td>" +
                                 "<td class='bottom right'>E: " + creator.Username + "</td></tr>");
                }
                stream.Write("</table></body></html>");
            }
        }
    }
}
