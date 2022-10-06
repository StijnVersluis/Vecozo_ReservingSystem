using IntefaceLayer;
using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class TeamDAL : SqlConnect, ITeam
    {
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

        public List<TeamDTO> GetTeams()
        {
            throw new NotImplementedException();
        }
    }
}
