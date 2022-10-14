using IntefaceLayer;
using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            OpenCon();
            List<UserDTO> list = new List<UserDTO>();
            DbCom.CommandText = "SELECT * FROM Users";
            reader = DbCom.ExecuteReader();

            while(reader.Read())
            {
                list.Add(new((int)reader["Id"], (string)reader["Name"], (int)reader["Role"]));
            }
            return list;
        }
        //Done
        public bool AttemptLogin(string uName, string password)
        {
            UserDTO DBuser = FindUserByUserName(uName);
            UserDTO user = null;

            if (DBuser == null) { return false; }

            var userPass = GetUserPassword(DBuser);
            var filledIn = HashString(password);

            Console.WriteLine("UserPass = " + userPass);
            Console.WriteLine("filledIn = " + filledIn);
            Console.WriteLine("Compared = " + filledIn == userPass);

            if (userPass == filledIn) { user = DBuser; }

            GlobalVariables.LoggedInUser = user;
            if (user == null) { return false; }
            else return true;
        }
        //Done
        public UserDTO FindUserByUserName(string uName)
        {
            DBConnection.Open();
            DbCom.CommandText = "SELECT Id, Name, Role FROM Users WHERE UserName = @name";
            DbCom.Parameters.AddWithValue("@name", uName);
            reader = DbCom.ExecuteReader();
            UserDTO user = null;
            while (reader.Read())
            {
                user = new UserDTO((int)reader["Id"], (string)reader["Name"], (int)reader["Role"]);
            }
            DBConnection.Close();
            return user;
        }
        //Done
        public string GetUserPassword(UserDTO user)
        {
            DBConnection.Open();
            DbCom.CommandText = "SELECT Password FROM Users WHERE Id = @id";
            DbCom.Parameters.AddWithValue("@id", user.Id);
            reader = DbCom.ExecuteReader();
            string password = String.Empty;
            while (reader.Read()) { password = (string)reader["Password"]; }
            DBConnection.Close();
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
        //Done
        public UserDTO GetLoggedInUser()
        {
            return GlobalVariables.LoggedInUser;
        }
        //Done
        public bool IsLoggedIn()
        {
            return GlobalVariables.LoggedInUser != null;
        }
        //Done
        public void Logout()
        {
            GlobalVariables.LoggedInUser = null;
        }

        public List<UserDTO> GetFilteredUsers(string filterStr)
        {
            List<UserDTO> users = new List<UserDTO>();
            OpenCon();

            DbCom.CommandText = "SELECT * FROM Users WHERE Name LIKE @str";
            var str = "%" + filterStr + "%";
            DbCom.Parameters.AddWithValue("str", str);

            reader = DbCom.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new((int)reader["Id"], (string)reader["Name"], (int)reader["Role"]));
            }

            CloseCon();
            return users;
        }
        #endregion

    }
}
