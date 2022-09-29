using IntefaceLayer;
using IntefaceLayer.DTO;
using System;

namespace DataLayer
{
    public class UserDAL : SqlConnect, IUser, IUserContainer
    {

        public bool IsLoggedIn()
        {
            if (GlobalVariables.LoggedInUser != null) return true;
            return false;
        }
        public void Logout()
        {
            GlobalVariables.LoggedInUser = null;
        }


        public UserDTO AttemptLogin(string uName, string password)
        {
            UserDTO DBuser = FindUserByUserName(uName);
            UserDTO user = null;

            if (DBuser.GetPassword() == HashString(password))
            {
                user = DBuser;
            }

            GlobalVariables.LoggedInUser = user;
            return user;
        }

        public UserDTO FindUserByUserName(string uName)
        {

            DbCom.CommandText = "SELECT Id, Name, Role FROM Users WHERE UserName = @name";
            DbCom.Parameters.AddWithValue("@name", uName);
            var reader = DbCom.ExecuteReader();
            var user = new UserDTO(0, "NONEX", 0);
            while (reader.Read())
            {
                user = new UserDTO((int)reader["Id"], (string)reader["Name"], (int)reader["Role"]);
            }
            return user;
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
    }
}
