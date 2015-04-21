using System;
using System.Data;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerAppCommonLib;

namespace VolunteerAppServer
{
    static public class DatabaseManager
    {
        //todo: clean up/consolidate this mess
        private const string TRUE = "1", FALSE = "0";
        //private const string CONNECTION_STRING = @"Data Source=..\..\Resources\volunteer_db.sqlite;Version=3;";
        private const string CONNECTION_STRING = @"Data Source=.\Resources\volunteer_db.sqlite;Version=3;";

        /// <summary>
        /// Gets the SQLite Connection for the database manager class methods.
        /// </summary>
        static private SQLiteConnection GetConnection()
        {
            var connection = new SQLiteConnection(CONNECTION_STRING);
            connection.Open();

            return connection;
        }

        /// <summary>
        /// Checks the user's entered data against the user's 
        /// username and password in the database.
        /// </summary>
        /// <param name="username">The users username/email address.</param>
        /// <param name="password">The MD5 hashed value of the users password.</param>
        static public bool IsValidCredentials(string username, string password)
        {
            using (var connection = GetConnection())
            {
                string query = "SELECT COUNT(Users.ID) FROM Users " +
                        "WHERE Users.Username = @username " +
                        "AND Users.Password =  @password";

                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                return (cmd.ExecuteScalar().ToString() == TRUE);
            }
        }

        /// <summary>
        /// Creates a new ContactInfo entry in the database witht the provided 
        /// phone number and email address, and returns the ID for the row created.
        /// <usage>Call from within RegisterNewUser()</usage>
        /// </summary>
        /// <param name="emailAddress">The user's email address.</param>
        /// <param name="phoneNumber">The user's phone number. Format: (###) ###-####</param>
        static private string GetNewContactInfo(string emailAddress, string phoneNumber)
        {
            using (var connection = GetConnection())
            {
                string query = "INSERT INTO ContactInfo (EmailAddress, PhoneNumber) " +
                    "VALUES (@email, @phone)";
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@email", emailAddress);
                cmd.Parameters.AddWithValue("@phone", phoneNumber);

                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                query = "SELECT ContactInfo.ID FROM ContactInfo WHERE " +
                    "ContactInfo.EmailAddress = @email";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@email", emailAddress);

                return cmd.ExecuteScalar().ToString();
            }
        }

        /// <summary>
        /// Creates a new Users entry in the database and associates
        /// it with the corresponding contact information.
        /// </summary>
        /// <param name="username">The user's email address.</param>
        /// <param name="password">The user's MD5 hashed password.</param>
        /// <param name="firstName">The user's first name.</param>
        /// <param name="lastName">The user's last name.</param>
        /// <param name="phoneNumber">The user's phone number.</param>
        static public void RegisterNewUser(string username, string password, string firstName,
            string lastName, string phoneNumber, bool admin)
        {
            using (var connection = GetConnection())
            {
                string contactID = GetNewContactInfo(username, phoneNumber);
                string query = "INSERT INTO Users (Username, Password, FirstName, LastName, " +
                    "ContactInfoID, IsAdmin) VALUES (@username, @password, @firstName, @lastName, " +
                    "@contact, @admin)";
                var cmd = connection.CreateCommand();
                var isAdmin = (admin) ? "1" : "0";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@firstName", firstName);
                cmd.Parameters.AddWithValue("@lastName", lastName);
                cmd.Parameters.AddWithValue("@contact", contactID);
                cmd.Parameters.AddWithValue("@admin", isAdmin);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Checks to see if user entered email address doesn't already
        /// exist in the database, i.e. being used by somebody else.
        /// </summary>
        /// <param name="emailAddress">The user's email address.</param>
        static public bool IsEmailInUse(string emailAddress)
        {
            using (var connection = GetConnection())
            {
                string query = "SELECT COUNT(Users.Username) FROM Users " +
                    "WHERE Users.Username = @email";
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@email", emailAddress);

                return (cmd.ExecuteScalar().ToString() == TRUE);
            }
        }

        /// <summary>
        /// Registers a user for an event.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="eventIds"></param>
        static public void RegisterUserForEvent(int userId, int[] eventIds)
        {
            using (var connection = GetConnection())
            {
                string query = "INSERT INTO Registered_Events(EventID,UserID) VALUES(@eId,@uId)";
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                foreach (var id in eventIds)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@uId", userId);
                    cmd.Parameters.AddWithValue("@eId", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        static public void UnregisterUserForEvent(int userId, int eventId)
        {
            using (var connection = GetConnection())
            {
                string query = "DELETE FROM Registered_Events WHERE EventId = @eId and UserId = @uId";
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@uId", userId);
                cmd.Parameters.AddWithValue("@eId", eventId);

                cmd.ExecuteNonQuery();
            }
        }

        static public UserInfo GetUserInformation(string username)
        {
            var table = new DataTable();
            using (var connection = GetConnection())
            {
                string query = "SELECT Users.ID, Users.Username, Users.FirstName, " +
                    "Users.LastName, ContactInfo.PhoneNumber, Users.IsAdmin FROM Users JOIN " +
                    "ContactInfo ON Users.ContactInfoID = ContactInfo.ID WHERE " +
                    "Users.Username = @username";
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@username", username);

                var da = new SQLiteDataAdapter(cmd);
                da.Fill(table);
            }

            var userInfo = new UserInfo(
                Int32.Parse(table.Rows[0][0].ToString()),
                table.Rows[0][1].ToString(),
                table.Rows[0][2].ToString(),
                table.Rows[0][3].ToString(),
                table.Rows[0][4].ToString(),
                (table.Rows[0][5].ToString() == TRUE) ? true : false);

            return userInfo;
        }

        static public UserInfo GetUserInformation(int contactId)
        {
            var table = new DataTable();
            using (var connection = GetConnection())
            {
                string query = "SELECT Users.ID, Users.Username, Users.FirstName, " +
                    "Users.LastName, ContactInfo.PhoneNumber, Users.IsAdmin FROM Users JOIN " +
                    "ContactInfo ON Users.ContactInfoID = ContactInfo.ID WHERE " +
                    "ContactInfo.ID = @contactId";
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@contactId", contactId);

                var da = new SQLiteDataAdapter(cmd);
                da.Fill(table);
            }

            var userInfo = new UserInfo(
                Int32.Parse(table.Rows[0][0].ToString()),
                table.Rows[0][1].ToString(),
                table.Rows[0][2].ToString(),
                table.Rows[0][3].ToString(),
                table.Rows[0][4].ToString(),
                (table.Rows[0][5].ToString() == TRUE) ? true : false);


            return userInfo;
        }

        static public List<int> GetRegisteredEventsForUser(int userId)
        {
            var table = new DataTable();
            var eventIds = new List<int>();

            using (var connection = GetConnection())
            {
                string query = "SELECT Registered_Events.EventID FROM Registered_Events " +
                               "WHERE UserID = @userId";
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@userId", userId);

                var da = new SQLiteDataAdapter(cmd);
                da.Fill(table);
            }

            foreach (DataRow row in table.Rows)
                eventIds.Add(Int32.Parse(row[0].ToString()));

            return eventIds;
        }

        static public List<int> GetCreatedEventsForUser(int userId)
        {
            var table = new DataTable();
            var eventIds = new List<int>();

            using (var connection = GetConnection())
            {
                string query = "SELECT Events.ID FROM Events WHERE Events.ContactInfoID = @userId";
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@userId", userId);

                var da = new SQLiteDataAdapter(cmd);
                da.Fill(table);
            }

            foreach (DataRow row in table.Rows)
                eventIds.Add(Int32.Parse(row[0].ToString()));

            return eventIds;
        }

        static public List<int> GetRegisteredIdsForEvent(int evtId)
        {
            var table = new DataTable();
            var userIds = new List<int>();

            using (var connection = GetConnection())
            {
                string query = "SELECT UserID FROM Registered_Events WHERE EventID = @eventId";
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@eventId", evtId);

                var da = new SQLiteDataAdapter(cmd);
                da.Fill(table);
            }

            foreach (DataRow row in table.Rows)
                userIds.Add(Int32.Parse(row[0].ToString()));

            return userIds;
        }

        static public List<EventInfo> GetAllEventsFromDb()
        {
            var table = new DataTable();
            var listOfEvents = new List<EventInfo>();
            using (var connection = GetConnection())
            {
                string query = "SELECT Events.ID, Events.Name, Events.Date, " +
                    "Events.Location, Events.Time, ContactInfo.ID as 'ContactID', " +
                    "Events.Duration FROM Events JOIN ContactInfo ON Events.ContactInfoID = " +
                    "ContactInfo.ID JOIN Users ON ContactInfo.ID = Users.ContactInfoID " +
                    "ORDER BY Events.Date";
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                var da = new SQLiteDataAdapter(cmd);
                da.Fill(table);
            }

            foreach (DataRow row in table.Rows)
            {
                listOfEvents.Add(new EventInfo(
                    Int32.Parse(row[0].ToString()),
                    row[1].ToString(),
                    row[2].ToString(),
                    row[4].ToString(),
                    row[3].ToString(),
                    double.Parse(row[6].ToString()),
                    Int32.Parse(row[5].ToString())
                    ));
            }

            return listOfEvents;
        }

        static public List<UserInfo> GetAllUsersFromDb()
        {
            var table = new DataTable();
            var listOfUsers = new List<UserInfo>();
            using (var connection = GetConnection())
            {
                string query = "SELECT Users.ID, Users.Username, Users.FirstName, " +
                    "Users.LastName, ContactInfo.PhoneNumber, Users.Isadmin FROM " +
                    "Users JOIN ContactInfo ON Users.ContactInfoID = ContactInfo.ID " +
                    "ORDER BY Users.Username";
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                var da = new SQLiteDataAdapter(cmd);
                da.Fill(table);
            }

            foreach (DataRow row in table.Rows)
            {
                listOfUsers.Add(new UserInfo(
                    Int32.Parse(row[0].ToString()),
                    row[1].ToString(),
                    row[2].ToString(),
                    row[3].ToString(),
                    row[4].ToString(),
                    (row[5].ToString() == TRUE ? true : false)
                    ));
            }

            return listOfUsers;
        }

        static public bool IsUserRegisteredForEvent(int eventId, int userId)
        {
            bool result;
            using (var connection = GetConnection())
            {
                string query = "SELECT COUNT(Events.ID) FROM Events JOIN Registered_Events " +
                    "ON Events.ID = Registered_Events.EventID JOIN Users ON " +
                    "Registered_Events.UserID = Users.ID WHERE Events.ID = " +
                    "@eventId AND Users.ID = @userId";
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@eventId", eventId);
                cmd.Parameters.AddWithValue("@userId", userId);

                result = (cmd.ExecuteScalar().ToString() == TRUE);
            }

            return result;
        }

        static public bool IsUserAdmin(int id)
        {
            bool result;
            using (var connection = GetConnection())
            {
                string query = "SELECT Users.IsAdmin FROM Users WHERE " +
                    "Users.ID = @id";
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@id", id);

                result = (cmd.ExecuteScalar().ToString() == TRUE);
            }

            return result;
        }

        static public int GetEventIDFromName(string name)
        {
            int result = -1;
            using (var connection = GetConnection())
            {
                string query = "SELECT Events.ID FROM Events WHERE Events.Name = @name";
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@name", name);

                result = Int32.Parse(cmd.ExecuteScalar().ToString());
            }

            return result;
        }

        static public int GetContactIDForUser(UserInfo user)
        {
            int result = -1;
            using (var connection = GetConnection())
            {
                string query = "SELECT Users.ContactInfoID FROM Users WHERE Users.ID = @userId";
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@userId", user.Id);

                result = Int32.Parse(cmd.ExecuteScalar().ToString());
            }

            return result;
        }

        static public int GetRegisteredCountForUser(int userId)
        {
            int result = -1;
            using (var connection = GetConnection())
            {
                string query = "SELECT COUNT(Registered_Events.EventID) FROM " +
                    "Registered_Events JOIN Users ON Registered_Events.UserID = " +
                    "Users.ID WHERE Users.ID = @userId";
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@userId", userId);

                result = Int32.Parse(cmd.ExecuteScalar().ToString());
            }

            return result;
        }

        static public int GetContactIDForUser(int userId)
        {
            int result = -1;
            using (var connection = GetConnection())
            {
                string query = "SELECT Users.ContactInfoID FROM Users WHERE Users.ID = @userId";
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@userId", userId);

                result = Int32.Parse(cmd.ExecuteScalar().ToString());
            }

            return result;
        }



        static public void CreateNewEvent(int userId, string name, string date, string time,
            string duration, string location)
        {
            using (var connection = GetConnection())
            {
                string query = "INSERT INTO Events (Name, Date, Time, Duration, " +
                    "Location, ContactInfoID) VALUES (@name, @date, @time, @duration, " +
                    "@location, @contactId)";
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@time", time);
                cmd.Parameters.AddWithValue("@duration", duration);
                cmd.Parameters.AddWithValue("@location", location);
                cmd.Parameters.AddWithValue("@contactId", userId);

                cmd.ExecuteNonQuery();
                int[] id = { GetEventIDFromName(name) };
                RegisterUserForEvent(userId, id);
            }
        }

        static public void UpdateEventInformation(int eventId, string name, string date, string time,
            string duration, string location)
        {
            using (var connection = GetConnection())
            {
                string query = "update Events set Name=@name, Date=@date, Time=@time, " +
                    "Duration=@duration, Location=@location where ID = @eventId";

                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@time", time);
                cmd.Parameters.AddWithValue("@duration", duration);
                cmd.Parameters.AddWithValue("@location", location);
                cmd.Parameters.AddWithValue("@eventId", eventId);

                cmd.ExecuteNonQuery();
            }
        }

        static public void UpdateUserContactInfo(int contactId, string email, string phoneNumber)
        {
            using (var connection = GetConnection())
            {
                string query = "update ContactInfo set EmailAddress=@email, " +
                    "PhoneNumber=@phone where ID = @contactId";

                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@phone", phoneNumber);
                cmd.Parameters.AddWithValue("@contactId", contactId);

                cmd.ExecuteNonQuery();
            }
        }

        static public void UpdateUserInformation(int userId, string username, string firstName, string lastName,
            bool isAdmin, string phoneNumber, string hashedPassword)
        {
            using (var connection = GetConnection())
            {
                var cmd = connection.CreateCommand();
                var admin = (isAdmin) ? "1" : "0";

                string query = (hashedPassword != null) ?
                    "update Users set Username=@username, Password=@password, " +
                    "FirstName=@firstName, LastName=@lastName, IsAdmin=@isAdmin " +
                    "where ID = @userId" :
                    "update Users set Username=@username, " +
                    "FirstName=@firstName, LastName=@lastName, IsAdmin=@isAdmin " +
                    "where ID = @userId";

                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@firstName", firstName);
                cmd.Parameters.AddWithValue("@lastName", lastName);
                cmd.Parameters.AddWithValue("@isAdmin", admin);
                cmd.Parameters.AddWithValue("@userId", userId);

                if (hashedPassword != null) cmd.Parameters.AddWithValue("@password", hashedPassword);

                cmd.ExecuteNonQuery();
                UpdateUserContactInfo(GetContactIDForUser(userId), username, phoneNumber);
            }
        }

        static public void DeleteEventFromDatabase(int eventId)
        {
            using (var connection = GetConnection())
            {
                string fQuery = "DELETE FROM Registered_Events WHERE EventID = @id";
                var cmd = connection.CreateCommand();
                cmd.CommandText = fQuery;
                cmd.Parameters.AddWithValue("@id", eventId);
                cmd.ExecuteNonQuery();

                string sQuery = "DELETE FROM Events WHERE ID = @id";
                cmd.CommandText = sQuery;
                cmd.ExecuteNonQuery();
            }
        }

        static public void DeleteUserFromDatabase(int userId)
        {
            using (var connection = GetConnection())
            {
                string fQuery = "DELETE FROM Registered_Events WHERE UserID = @id";
                var cmd = connection.CreateCommand();
                cmd.CommandText = fQuery;
                cmd.Parameters.AddWithValue("@id", userId);
                cmd.ExecuteNonQuery();

                string sQuery = "DELETE FROM ContactInfo WHERE ID = @id";
                cmd.CommandText = sQuery;
                cmd.ExecuteNonQuery();

                string tQuery = "DELETE FROM Users WHERE ID = @id";
                cmd.CommandText = tQuery;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
