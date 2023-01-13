using IntefaceLayer.DTO;
using System.Collections.Generic;

namespace IntefaceLayer
{
    public interface ITeam
    {
        public List<UserDTO> GetUsers(int id); 
        public bool AddUser(TeamDTO team, UserDTO user); 
        public bool RemoveUser(TeamDTO team, UserDTO user); 
        public TeamDTO GetTeam(int id);
        public List<TeamDTO> GetTeamsOfUser(int userId);
        public bool CreateTeam(string name, List<int> userids);
        public bool EditTeam(TeamDTO team, List<int> userids);
        public bool DeleteTeam(int teamId);
        public bool LeaveTeam(int teamId, int userId);
        public bool IsTeamAdmin(int teamId, int userId);
        public UserDTO GetTeamAdmin(int teamId);
        public bool Check_Accessibility(string username);
        public bool Exists(string name, int userId);
        public List<TeamDTO> GetArchivedTeams();
    }
}
