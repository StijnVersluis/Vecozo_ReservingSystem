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
            return iTeam.GetTeams().ConvertAll(x => new Team(x));
        }
        public List<Team> GetTeamsOfUser(int userId)
        {
            return iTeam.GetTeamsOfUser(userId).ConvertAll(x => new Team(x));
        }
        public bool CreateTeam(string name, User aUser, List<User> users)
        {
            return iTeam.CreateTeam(name, aUser.ToDTO(), users.ConvertAll(x=>x.ToDTO()));
        }

        public bool Check_Accessibility(string username)
        {

            return iTeam.Check_Accessibility(username);
        }
    }
}
