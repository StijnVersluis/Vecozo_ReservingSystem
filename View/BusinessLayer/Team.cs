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
        private int Id { get; set; }
        public string Name { get; set; }
        public Team(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public Team(TeamDTO team)
        {
            this.Id = team.Id;
            this.Name = team.Name;
        }

        public TeamDTO ToDTO()
        {
            return new TeamDTO(Id, Name);
        }

        public bool AddUser(User user, ITeam iTeam)
        {
            return iTeam.AddUser(this.ToDTO(), user.ToDTO());
        }

        public bool RemoveUser(User user, ITeam iTeam)
        {
            return iTeam.RemoveUser(this.ToDTO(), user.ToDTO());
        }
    }
}
