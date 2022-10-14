using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntefaceLayer
{
    public interface ITeam
    {
        public bool AddUser(TeamDTO team, UserDTO user); 
        public bool RemoveUser(TeamDTO team, UserDTO user); 
        public List<TeamDTO> GetTeams();
        public List<TeamDTO> GetTeamsOfUser(int userId);
        public bool CreateTeam(string name, UserDTO aUser, List<UserDTO> users);
        public bool DeleteUserOutTeam(int userID, int teamID);
        public int ChangeFromTeam(int teamID, int userID);
    }
}
