using IntefaceLayer;
using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Team
    {
        public string Name { get; set; }
        public Team(string name)
        {
            Name = name;
        }
        public Team(TeamDTO team)
        {
            this.Name = team.Name;
        }

        public TeamDTO ToDTO()
        {
            return new TeamDTO(Name);
        }

        public bool AddUser(User user, ITeam iTeam)
        {
            return iTeam.AddUser(this.ToDTO(), user.ToDTO());
        }
    }
}
