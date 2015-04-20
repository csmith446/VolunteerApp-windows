using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerAppCommonLib
{
    [Serializable]
    public class EventInfo
    {
        #region private properties
        private int _Id { get; set; }
        private double _Duration { get; set; }
        private string _Name { get; set; }
        private string _Date { get; set; }
        private DateTime _StartTime { get; set; }
        private DateTime _EndTime { get; set; }
        private string _Location { get; set; }
        private int _UserInfoId { get; set; }
        private List<int> _RegisteredUsers { get; set; }
        private bool _IsCurrent { get; set; }
        #endregion

        public EventInfo(int id, string name, string date,
            string time, string location, double duration, int userId)
        {
            this._Id = id;
            this._Name = name;
            this._Date = date;
            this._StartTime = DateTime.ParseExact(time, "h:mm tt", CultureInfo.InvariantCulture);
            this._Duration = duration;
            this._EndTime = this._StartTime.AddHours(duration);
            this._Location = location;
            this._UserInfoId = userId;
            this._RegisteredUsers = new List<int>();

            var dt = DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            this._IsCurrent = (DateTime.Now.CompareTo(dt) < 0);
        }

        #region public gets/sets
        public int Id
        {
            get { return this._Id; }
        }

        public string Name
        {
            get { return this._Name; }
        }

        public string Date
        {
            get { return this._Date; }
        }

        public DateTime StartTime
        {
            get { return this._StartTime; }
        }

        public DateTime EndTime
        {
            get { return this._EndTime; }
        }

        public double Duration
        {
            get { return this._Duration; }
        }

        public string Location
        {
            get { return this._Location; }
        }

        public int CreatorId
        {
            get { return this._UserInfoId; }
        }

        public bool Current
        {
            get { return this._IsCurrent; }
        }
        #endregion / setters

        public List<int> RegisteredUsers
        {
            get { return this._RegisteredUsers; }
            set { this._RegisteredUsers = value; }
        }
    }
}
