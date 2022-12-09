using IntefaceLayer;
using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Xml.Linq;

namespace DataLayer
{
    public class UserDAL : SqlConnect, IUser, IUserContainer
    {
        private SqlDataReader reader;
        public UserDAL() { InitializeDB(); }

        #region IUserContainer functions
        //Done
        public List<UserDTO> GetAll()
        {
            List<UserDTO> list = new List<UserDTO>();

            try
            {
                OpenCon();
                DbCom.CommandText = "SELECT * FROM Users";
                reader = DbCom.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new((int)reader["Id"], (string)reader["Name"], (int)reader["Role"], (bool)reader["IsBHV"]));
                }
            } finally
            {
                CloseCon();
            }

            return list;
        }
        //Done
        public bool AttemptLogin(string email, string password)
        {
            UserDTO DBuser = FindUserByEmail(email);
            UserDTO user = null;

            if (DBuser == null) { return false; }

            var userPass = GetUserPassword(DBuser);
            var filledIn = HashString(password);

            if (userPass == filledIn) { user = DBuser; }

            GlobalVariables.LoggedInUser = user;
            if (user == null) { return false; }
            else return true;
        }
        //Done
        public UserDTO FindUserByEmail(string email)
        {
            UserDTO user = null;

            try
            {
                OpenCon();
                DbCom.CommandText = "SELECT Id, Name, Role, IsBHV FROM Users WHERE Email = @email";
                DbCom.Parameters.AddWithValue("@email", email);
                reader = DbCom.ExecuteReader();
                while (reader.Read())
                {
                    user = new UserDTO((int)reader["Id"], (string)reader["Name"], (int)reader["Role"], (bool)reader["IsBHV"]);
                }
            } catch (Exception e)
            {
                Debug.WriteLine(e.Message); 
            }
            finally
            {
                CloseCon();
            }

            return user;
        }
        //Done
        public string GetUserPassword(UserDTO user)
        {
            string password = String.Empty;

            try
            {
                OpenCon();
                DbCom.CommandText = "SELECT Password FROM Users WHERE Id = @id";
                DbCom.Parameters.AddWithValue("@id", user.Id);
                reader = DbCom.ExecuteReader();
                while (reader.Read()) { password = (string)reader["Password"]; }
            }
            finally
            {
                CloseCon();
            }

            return password;
        }
        //Done
        static string HashString(string text, string salt = "")
        {
            if (String.IsNullOrEmpty(text)) return String.Empty;

            // Uses SHA256 to create the hash
            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                // Convert the string to a byte array first, to be processed
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text + salt);
                byte[] hashBytes = sha.ComputeHash(textBytes);

                // Convert back to a string, removing the '-' that BitConverter adds
                string hash = BitConverter
                    .ToString(hashBytes)
                    .Replace("-", String.Empty);
                Console.WriteLine(text + " hashed = " + hash);
                return hash;
            }
        }

        public List<UserDTO> GetFilteredUsers(string filterStr)
        {
            List<UserDTO> users = new List<UserDTO>();
            var str = !String.IsNullOrEmpty(filterStr) ? filterStr : "DONT SELECT ALL";

            try
            {
                OpenCon();

                DbCom.CommandText = "SELECT * FROM Users WHERE Name LIKE @name";
                var name = "%" + str + "%";
                DbCom.Parameters.AddWithValue("name", name);

                reader = DbCom.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(new((int)reader["Id"], (string)reader["Name"], (int)reader["Role"], (bool)reader["IsBHV"]));
                }
            }
            finally
            {
                CloseCon();
            }

            return users;
        }

        public UserDTO GetUserById(int id)
        {
            UserDTO user = null;

            try
            {
                OpenCon();
                DbCom.CommandText = "SELECT Id, Name, Role FROM Users WHERE Id = @id";
                DbCom.Parameters.AddWithValue("@id", id);
                reader = DbCom.ExecuteReader();
                while (reader.Read())
                {
                    user = new UserDTO((int)reader["Id"], (string)reader["Name"], (int)reader["Role"], (bool)reader["IsBHV"]);
                }
            }
            finally
            {
                CloseCon();
            }

            return user;
        }

        public bool IsLoggedIn()
        {
            return GlobalVariables.LoggedInUser != null;
        }

        public UserDTO GetLoggedInUser()
        {
            return GlobalVariables.LoggedInUser;
        }

        public void Logout()
        {
            GlobalVariables.LoggedInUser = null;
        }

        #endregion

        #region IUser functions
        public bool IsPresent(int id, DateTime datetime)
        {
            bool isPresent = false;
            DateTime sqlDatetime = new DateTime(datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, 0);
            try
            {
                OpenCon();
                DbCom.CommandText = "SELECT User_Id, DateTime_Arriving, DateTime_Leaving FROM Reservations WHERE " +
                    "@datetime BETWEEN DateTime_Arriving AND DateTime_Leaving AND " +
                    "User_Id = @id";
                DbCom.Parameters.AddWithValue("@datetime", sqlDatetime);
                DbCom.Parameters.AddWithValue("@id", id);

                reader = DbCom.ExecuteReader();
                while (reader.Read())
                {
                    isPresent = true;
                }

            } catch(Exception e) { throw new Exception(e.Message, e); }
            finally { CloseCon(); }
            return isPresent;
        }
        #endregion

    }
}
