using IntefaceLayer;
using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class TeamContainerMock:ITeam
    {
       public List<TeamDTO> teams=new List<TeamDTO>();    
        public TeamContainerMock()
        {
            TeamDTO team1 = new TeamDTO ( 1,"Team1" );
            TeamDTO team2 = new TeamDTO(2, "Team2");
            teams.Add(team1);   
            teams.Add(team2);   

        }

        public bool AddUser(TeamDTO team, UserDTO user)
        {
         throw new NotImplementedException();
        }

        public bool Check_Accessibility(string username)
        {
         throw new NotImplementedException();
        }

        public bool CreateTeam(string name, List<int> userids)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTeam(int teamId)
        {
           var todelete= teams.Find(x => x.Id == teamId);
            teams.Remove(todelete);
            return true;    
        }

        public bool EditTeam(TeamDTO team, List<int> userids)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string name, int userId)
        {
            throw new NotImplementedException();
        }

        public List<TeamDTO> GetArchivedTeams()
        {
            throw new NotImplementedException();
        }

        public TeamDTO GetTeam(int id)
        {
           var team=teams.Find(x => x.Id == id); 
            return team;
        }

        public UserDTO GetTeamAdmin(int teamId)
        {
            throw new NotImplementedException();
        }

        public List<TeamDTO> GetTeamsOfUser(int userId)
        {
            throw new NotImplementedException();
        }

        public List<UserDTO> GetUsers(int id)
        {
            throw new NotImplementedException();    
        }

        public bool IsTeamAdmin(int teamId, int userId)
        {
            throw new NotImplementedException();
        }

        public bool LeaveTeam(int teamId, int userId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveUser(TeamDTO team, UserDTO user)
        {
            throw new NotImplementedException();
        }
    }
}
