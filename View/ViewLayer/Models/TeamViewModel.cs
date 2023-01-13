using BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ViewLayer.Models
{
    public class TeamViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public string AddedUserIds { get; set; }
        public string Name { get; set; }

        public DateTime? DeletedAt { get; set; }

        public User Owner { get; set; }

        public List<User> Users { get; set; }

        public TeamViewModel() { }

        public TeamViewModel(Team team)
        {
            this.Id = team.Id;
            this.Name = team.Name;
            this.DeletedAt = team.DeletedAt;
        }

        public TeamViewModel(List<User> users)
        {
            this.Users = users;
        }
    }
}
