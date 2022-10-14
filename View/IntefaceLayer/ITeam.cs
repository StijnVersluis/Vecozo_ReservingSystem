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
        public List<UserDTO> GetUsers(int id); 
        public bool AddUser(TeamDTO team, UserDTO user); 
        public bool RemoveUser(TeamDTO team, UserDTO user); 
        public List<TeamDTO> GetTeams();
        public TeamDTO GetTeam(int id);
        public List<TeamDTO> GetTeamsOfUser(int userId);
        public bool CreateTeam(string name, List<int> userids);
    }
}
