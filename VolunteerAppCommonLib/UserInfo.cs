﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerAppCommonLib
{
    [Serializable]
    public class UserInfo
    {
        #region private properties
        private int _Id { set; get; }
        private string _Username { set; get; }
        private string _PhoneNumber { set; get; }
        private bool _IsAdmin { get; set; }
        private Tuple<string, string> _FullName { set; get; }
        private List<EventInfo> _CreatedEvents { get; set; }
        private List<EventInfo> _RegisteredEvents { get; set; }
        #endregion

        public UserInfo(int id, string username, string firstName, string lastName,
            string phoneNumber, bool isAdmin)
        {
            this._Id = id;
            this._Username = username;
            this._PhoneNumber = phoneNumber;
            this._IsAdmin = isAdmin;
            this._FullName = new Tuple<string, string>(firstName, lastName);
            this._CreatedEvents = new List<EventInfo>();
            this._RegisteredEvents = new List<EventInfo>();
        }

        #region gets/sets
        public List<EventInfo> RegisteredEvents
        {
            get { return this._RegisteredEvents; }
            set { this._RegisteredEvents = value; }
        }

        public List<EventInfo> CreatedEvents
        {
            get { return this._CreatedEvents; }
            set { this._CreatedEvents = value; }
        }

        public int Id
        {
            get { return this._Id; }
        }

        public bool IsAdmin
        {
            get { return this._IsAdmin; }
        }

        public string Username
        {
            get { return this._Username; }
        }

        public string PhoneNumber
        {
            get { return this._PhoneNumber; }
        }

        public Tuple<string, string> FullName
        {
            get { return this._FullName; }
        }
        #endregion
    }
}
