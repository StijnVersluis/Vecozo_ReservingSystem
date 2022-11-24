using IntefaceLayer;
using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;

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
            List<int> userIds = new List<int>();
            List<UserDTO> users = new List<UserDTO>();

            try
            {
                if (DbCom.Connection.State == ConnectionState.Closed)
                {
                    OpenCon();
                }

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

                    DbCom.Parameters.Clear();
                    DbCom.CommandText = "SELECT * FROM Users WHERE Id = @id";
                    DbCom.Parameters.AddWithValue("id", userid);

                    reader = DbCom.ExecuteReader();

                    while (reader.Read())
                    {
                        users.Add(new UserDTO((int)reader["Id"], (string)reader["Name"], (int)reader["Role"]));
                    }

                    CloseCon();
                });
            }
            catch (Exception e) { 
                Debug.WriteLine(e.Message); 
            }

            return users;
        }

        public bool AddUser(TeamDTO team, UserDTO user)
        {
            bool success = false;

            try
            {
                OpenCon();

                DbCom.Parameters.Clear();
                DbCom.CommandText = "INSERT INTO TeamMembers (Team_Id, User_Id) Values (@tId, @uId)";
                DbCom.Parameters.AddWithValue("tId", team.Id);
                DbCom.Parameters.AddWithValue("uId", user.Id);

                success = DbCom.ExecuteNonQuery() > 0;
            } finally
            {
                CloseCon();
            }

            return success;
        }

        public bool RemoveUser(TeamDTO team, UserDTO user)
        {
            bool success = false;

            try
            {
                OpenCon();

                DbCom.Parameters.Clear();
                DbCom.CommandText = "DELETE FROM TeamMembers WHERE Team_Id = @tId and User_Id = @uId";
                DbCom.Parameters.AddWithValue("tId", team.Id);
                DbCom.Parameters.AddWithValue("uId", user.Id);

                success = DbCom.ExecuteNonQuery() > 0;
            } finally
            {
                CloseCon();
            }

            return success;
        }

        public bool CreateTeam(string name, List<int> userids)
        {
            try
            {
                OpenCon();
                DbCom.CommandText = "INSERT INTO Teams (Name) Values (@name) SELECT SCOPE_IDENTITY()";
                DbCom.Parameters.AddWithValue("name", name);
                decimal idDec = (decimal)DbCom.ExecuteScalar();

                DbCom.Parameters.Clear();
                DbCom.CommandText = "INSERT INTO TeamMembers (Team_Id, User_Id, Is_Team_Admin) Values (@tId, @uId, @isAdmin)";
                DbCom.Parameters.AddWithValue("tId", idDec);
                DbCom.Parameters.AddWithValue("uId", GlobalVariables.LoggedInUser.Id);
                DbCom.Parameters.AddWithValue("isAdmin", 1);

                DbCom.ExecuteNonQuery();

                userids.ForEach(userid =>
                {
                    if (GlobalVariables.LoggedInUser.Id == userid) return;
                    DbCom.Parameters.Clear();
                    DbCom.CommandText = "INSERT INTO TeamMembers (Team_Id, User_Id, Is_Team_Admin) Values (@tId, @uId, @isAdmin)";
                    DbCom.Parameters.AddWithValue("tId", idDec);
                    DbCom.Parameters.AddWithValue("uId", userid);
                    DbCom.Parameters.AddWithValue("isAdmin", 0);

                    DbCom.ExecuteNonQuery();
                });

                return true;
            } catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            } finally
            {
                CloseCon();
            }
        }

        public bool EditTeam(TeamDTO team, List<int> userids)
        {
            try
            {
                OpenCon();
                DbCom.CommandText = "" +
                    "UPDATE Teams SET Name = @a WHERE Id = @b " +
                    "DELETE FROM TeamMembers WHERE Team_Id = @c AND Id IN (SELECT Id FROM TeamMembers WHERE Is_Team_Admin != 1)";

                DbCom.Parameters.AddWithValue("a", team.Name);
                DbCom.Parameters.AddWithValue("b", team.Id);
                DbCom.Parameters.AddWithValue("c", team.Id);
                DbCom.ExecuteNonQuery();
                DbCom.Parameters.Clear();

                userids.ForEach(userid =>
                {
                    if (userid == GlobalVariables.LoggedInUser.Id) return;

                    DbCom.Parameters.Clear();
                    DbCom.CommandText = "INSERT INTO TeamMembers (Team_Id, User_Id, Is_Team_Admin) Values (@tId, @uId, @isAdmin)";
                    DbCom.Parameters.AddWithValue("tId", team.Id);
                    DbCom.Parameters.AddWithValue("uId", userid);
                    DbCom.Parameters.AddWithValue("isAdmin", 0);
                    DbCom.ExecuteNonQuery();
                });

                return true;
            }
            catch (Exception e) 
            { 
                Debug.WriteLine(e.Message);
                return false;
            }
            finally
            {
                CloseCon();
            }
        }

        public List<TeamDTO> GetTeamsOfUser(int userId)
        {
            List<int> teamIds = new List<int>();
            List<TeamDTO> teams = new List<TeamDTO>();

            try
            {
                OpenCon();

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
            } catch(Exception e) { Debug.WriteLine(e.Message); }


            return teams;
        }

        public bool DeleteTeam(int id)
        {
            bool result = false;

            try
            {
                OpenCon();
                DbCom.Parameters.Clear();
                DbCom.CommandText =
                    "DELETE FROM TeamMembers WHERE Team_Id = @id " +
                    "DELETE FROM Teams WHERE Id = @id";
                DbCom.Parameters.AddWithValue("id", id);

                result = DbCom.ExecuteNonQuery() > 0 ? true : false;
            }
            finally
            {
                CloseCon();
            }

            return result;
        }

        public TeamDTO GetTeam(int id)
        {
            TeamDTO team = null;

            try
            {
                OpenCon();
                DbCom.Parameters.Clear();
                DbCom.CommandText = "SELECT * FROM Teams WHERE Id = @id";
                DbCom.Parameters.AddWithValue("id", id);
                reader = DbCom.ExecuteReader();
                while (reader.Read())
                {
                    team = new TeamDTO((int)reader["Id"], (string)reader["Name"]);
                }
            } finally
            {
                CloseCon();
            }

            return team;
        }

        public bool LeaveTeam(int teamId, int userId)
        {
            bool result = false;

            try
            {
                OpenCon();
                DbCom.Parameters.Clear();
                DbCom.CommandText = "DELETE FROM TeamMembers WHERE Team_Id = @TeamId AND User_Id = @UserId AND Is_Team_Admin = 0";
                DbCom.Parameters.AddWithValue("TeamId", teamId);
                DbCom.Parameters.AddWithValue("UserId", userId);
                int rowsAffected = DbCom.ExecuteNonQuery();

                result =  rowsAffected > 0;
                if (result)
                {
                    // The team admin is the only one left.
                    var users = GetUsers(teamId);
                    if (users.Count == 1)
                    {
                        // The team has to get deleted.
                        CloseCon();
                        result = DeleteTeam(teamId);
                    }
                }
            } 
            finally
            {
                if (DbCom.Connection.State == ConnectionState.Open)
                {
                    CloseCon();
                }
            }

            return result;
        }

        public bool IsTeamAdmin(int teamId, int userId)
        {
            bool result = false;

            try
            {
                OpenCon();
                DbCom.CommandText = "SELECT * FROM TeamMembers WHERE Team_Id = @TeamId AND User_Id = @UserId AND Is_Team_Admin = 1";
                DbCom.Parameters.AddWithValue("TeamId", teamId);
                DbCom.Parameters.AddWithValue("UserId", userId);
                reader = DbCom.ExecuteReader();
                DbCom.Parameters.Clear();

                result = reader.HasRows ? true : false;

            }
            finally
            {
                CloseCon();
            }

            return result;
        }

        public UserDTO GetTeamAdmin(int teamId)
        {
            UserDTO user = null;

            try
            {
                OpenCon();
                DbCom.CommandText = 
                    "SELECT Users.* FROM Users " +
                    "INNER JOIN TeamMembers " +
                    "ON TeamMembers.User_Id = Users.Id " +
                    "WHERE TeamMembers.Is_Team_Admin = 1";
                reader = DbCom.ExecuteReader();

                while (reader.Read())
                {
                    user = new((int)reader["Id"], (string)reader["Name"], (int)reader["Role"]);
                }
            }
            finally
            {
                CloseCon();
            }

            return user;
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
