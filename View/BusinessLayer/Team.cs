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
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Team(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Team(int id)
        {
            Id = id;
        }

        public Team(TeamDTO team)
        {
            if (team == null) return;

            this.Id = team.Id;
            this.Name = team.Name;
            this.DeletedAt = team.DeletedAt;
        }

        public TeamDTO ToDTO()
        {
            return new TeamDTO(Id, Name);
        }

        public List<User> GetUsers(ITeam iTeam)
        {
            return iTeam.GetUsers(this.Id).ConvertAll(x => new User(x));
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
