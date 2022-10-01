using IntefaceLayer;
using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    internal class TeamDAL : SqlConnect, ITeam
    {
        public bool AddUser(TeamDTO team, UserDTO user)
        {
            throw new NotImplementedException();
        }

        public bool CreateTeam(string name, UserDTO aUser, List<UserDTO> users)
        {
            throw new NotImplementedException();
        }

        public List<TeamDTO> GetTeams()
        {
            throw new NotImplementedException();
        }
    }
}
