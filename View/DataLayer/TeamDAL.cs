using IntefaceLayer;
using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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

        public bool DeleteUserOutTeam(int userID, int teamID)
        {
            try
            {
            OpenCon();
            DbCom.CommandText = "DELETE FROM TeamMembers WHERE Team_id = @Team_id AND User_id = @User_id";
                DbCom.Parameters.AddWithValue("User_id", userID);
                DbCom.Parameters.AddWithValue("Team_id", teamID);
                var result = DbCom.ExecuteNonQuery() > 0;
                return result;
            }
            catch (Exception exception)
            {
               Console.WriteLine(exception);
                return false;
            }
            finally
            {
                CloseCon();
            }
        }

        public int ChangeFromTeam(int teamID, int userID)
        {
            try
            {
                OpenCon();
                DbCom.CommandText = ("UPDATE TeamMembers SET Team_id = @Team_id WHERE User_id = @User_id");
                DbCom.Parameters.AddWithValue("Team_id", teamID);
                DbCom.Parameters.AddWithValue("User_id", userID);
                return DbCom.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return DbCom.ExecuteNonQuery();
                //????????
            }
            finally
            {
                CloseCon();
            }
        }
    }
}
