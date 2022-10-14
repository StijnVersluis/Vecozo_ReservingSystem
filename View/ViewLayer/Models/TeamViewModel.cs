using BusinessLayer;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ViewLayer.Models
{
    public class TeamViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<User> Users { get; set; }

        public TeamViewModel(Team team)
        {
            this.Id = team.Id;
            this.Name = team.Name;
        }
        public TeamViewModel(List<User> users)
        {
            this.Users = users;
        }
    }
}
