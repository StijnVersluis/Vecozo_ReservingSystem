using IntefaceLayer;
using IntefaceLayer.DTO;
using System;

namespace BusinessLayer
{
    public class User
    {
        public int Id { private set; get; }
        public string Name { private set; get; }
        public int Role { private set; get; }

        public User(int id, string name, int role)
        {
            Id = id;
            Name = name;
            Role = role;
        }

        public User(UserDTO user)
        {
            Id = user.Id;
            Name = user.Name;
            Role = user.Role;
        }
    }
}
