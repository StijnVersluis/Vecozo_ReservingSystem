using IntefaceLayer;
using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class TeamContainer
    {
        public ITeam iTeam;
        public TeamContainer(ITeam iteam)
        {
            iTeam = iteam;
        }
        public List<Team> GetTeams()
        {
            return iTeam.GetTeams().ConvertAll(teamdto => new Team(teamdto));
        }
        public Team GetTeam(int id)
        {
            return new Team(iTeam.GetTeam(id));
        }
        public List<Team> GetTeamsOfUser(int userId)
        {
            return iTeam.GetTeamsOfUser(userId).ConvertAll(teamdto => new Team(teamdto));
        }
        public bool CreateTeam(string name, List<int> userids)
        {
            return iTeam.CreateTeam(name, userids);
        }
    }
}
