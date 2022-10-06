using IntefaceLayer;
using IntefaceLayer.DTO;
using System;

namespace DataLayer
{
    public class UserDAL : SqlConnect, IUser, IUserContainer
    {
        public UserDAL() { InitializeDB(); }

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

        public UserDTO FindUserByUserName(string uName)
        {
            DBConnection.Open();
            DbCom.CommandText = "SELECT Id, Name, Role FROM Users WHERE UserName = @name";
            DbCom.Parameters.AddWithValue("@name", uName);
            var reader = DbCom.ExecuteReader();
            UserDTO user = null;
            while (reader.Read())
            {
                user = new UserDTO((int)reader["Id"], (string)reader["Name"], (int)reader["Role"]);
            }
            DBConnection.Close();
            return user;
        }

        public string GetUserPassword(UserDTO user)
        {
            DBConnection.Open();
            DbCom.CommandText = "SELECT Password FROM Users WHERE Id = @id";
            DbCom.Parameters.AddWithValue("@id", user.Id);
            var reader = DbCom.ExecuteReader();
            string password = String.Empty;
            while (reader.Read()) { password = (string)reader["Password"]; }
            DBConnection.Close();
            return password;
        }

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

        public UserDTO GetLoggedInUser()
        {
            return GlobalVariables.LoggedInUser;
        }

        public bool IsLoggedIn()
        {
            return GlobalVariables.LoggedInUser != null;
        }

        public void Logout()
        {
            GlobalVariables.LoggedInUser = null;
        }
    }
}
