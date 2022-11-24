using IntefaceLayer;
using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataLayer
{
    public class TeamDAL : SqlConnect, ITeam
    {
        SqlDataReader reader;
        public TeamDAL()
        {
            InitializeDB();
        }

        public List<UserDTO> GetUsers(int id)
        {
            OpenCon(); 

            List<int> userIds = new List<int>();
            List<UserDTO> users = new List<UserDTO>();

            DbCom.CommandText = "SELECT * FROM TeamMembers WHERE Team_Id = @id";
            DbCom.Parameters.AddWithValue("id", id);

            reader = DbCom.ExecuteReader();

            while (reader.Read())
            {
                userIds.Add((int)reader["User_Id"]);
            }
            CloseCon();
            userIds.ForEach(userid =>
            {
                OpenCon();
                DbCom.CommandText = "SELECT * FROM Users WHERE Id = @id";
                DbCom.Parameters.Clear();
                DbCom.Parameters.AddWithValue("id", userid);

                reader = DbCom.ExecuteReader();

                while(reader.Read())
                {
                    users.Add(new UserDTO((int)reader["Id"], (string)reader["Name"], (int)reader["Role"]));
                }
                CloseCon();
            });
            return users;
        }

        public bool AddUser(TeamDTO team, UserDTO user)
        {
            OpenCon();

            DbCom.Parameters.Clear();
            DbCom.CommandText = "INSERT INTO TeamMembers (Team_Id, User_Id) Values (@tId, @uId)";
            DbCom.Parameters.AddWithValue("tId", team.Id);
            DbCom.Parameters.AddWithValue("uId", user.Id);

            bool success = DbCom.ExecuteNonQuery()>0;
            CloseCon();
            return success;
        }

        public bool RemoveUser(TeamDTO team, UserDTO user)
        {
            OpenCon();

            DbCom.Parameters.Clear();
            DbCom.CommandText = "DELETE FROM TeamMembers WHERE Team_Id = @tId and User_Id = @uId";
            DbCom.Parameters.AddWithValue("tId", team.Id);
            DbCom.Parameters.AddWithValue("uId", user.Id);

            bool success = DbCom.ExecuteNonQuery() > 0;
            CloseCon();
            return success;
        }

        public bool CreateTeam(string name, UserDTO aUser, List<UserDTO> users)
        {
            try
            {
                DBConnection.Open();
                DbCom.CommandText = "INSERT INTO Teams (Name) Values (@name) SELECT SCOPE_IDENTITY()";
                DbCom.Parameters.AddWithValue("name", name);
                decimal idDec = (decimal)DbCom.ExecuteScalar();

                DbCom.Parameters.Clear();
                DbCom.CommandText = "INSERT INTO TeamMembers (Team_Id, User_Id, Is_Team_Admin) Values (@tId, @uId, @isAdmin)";
                DbCom.Parameters.AddWithValue("tId", idDec);
                DbCom.Parameters.AddWithValue("uId", aUser.Id);
                DbCom.Parameters.AddWithValue("isAdmin", 1);

                DbCom.ExecuteNonQuery();

                users.ForEach(user =>
                {
                    DbCom.Parameters.Clear();
                    DbCom.CommandText = "INSERT INTO TeamMembers (Team_Id, User_Id, Is_Team_Admin) Values (@tId, @uId, @isAdmin)";
                    DbCom.Parameters.AddWithValue("tId", idDec);
                    DbCom.Parameters.AddWithValue("uId", user.Id);
                    DbCom.Parameters.AddWithValue("isAdmin", 0);

                    DbCom.ExecuteNonQuery();
                });
                return true;
            } catch (Exception e)
            {
                Console.Write(e);
                return false;
            }
        }

        public List<TeamDTO> GetTeamsOfUser(int userId)
        {
            OpenCon();
            List<int> teamIds = new List<int>();
            List<TeamDTO> teams = new List<TeamDTO>();

            DbCom.CommandText = "SELECT Team_Id FROM TeamMembers WHERE User_Id = @userId";
            DbCom.Parameters.AddWithValue("userId", userId);

            reader = DbCom.ExecuteReader();

            while (reader.Read())
            {
                teamIds.Add(reader.GetInt32(0));
            }
            CloseCon();
            teamIds.ForEach(teamId =>
            {
                OpenCon();
                reader = null;
                DbCom.CommandText = "SELECT * FROM Teams WHERE Id = @id";
                DbCom.Parameters.Clear();
                DbCom.Parameters.AddWithValue("id", teamId);

                reader = DbCom.ExecuteReader();

                while (reader.Read())
                {
                    teams.Add(new TeamDTO((int)reader["Id"], (string)reader["Name"]));
                }
                CloseCon();
            });
            CloseCon();
            return teams;
        }

        public List<TeamDTO> GetTeams()
        {
            throw new NotImplementedException();
        }
        public bool Check_Accessibility(string username)
        {
            bool Is_Team_Admin = false;
            OpenCon();
            reader = null;
            DbCom.CommandText = "select * TeamMembers Where UserName=@username  ";
            DbCom.Parameters.AddWithValue("username", username);
            reader = DbCom.ExecuteReader(); 
            while (reader.Read())
            {
                
                Is_Team_Admin = Convert.ToBoolean(reader["Is_Team_Admin"]);

            }
            CloseCon();
            if (Is_Team_Admin == true)
            {
                return true;
            }
            return false;   
        



        }

        public TeamDTO GetTeam(int id)
        {
            OpenCon();
            TeamDTO team = null;
            DbCom.CommandText = "SELECT * FROM Teams WHERE Id = @id";
            DbCom.Parameters.AddWithValue("id", id);
            reader = DbCom.ExecuteReader();
            while (reader.Read())
            {
                team = new TeamDTO((int)reader["Id"], (string)reader["Name"]);
            }
            CloseCon();
            return team;
        }
    }
}
