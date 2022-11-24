using IntefaceLayer;
using IntefaceLayer.DTO;
using System;

namespace BusinessLayer
{
    public class User
    {
        public int Id { private set; get; } = 0;
        public string Name { private set; get; }
        public int Role { private set; get; }

        public IUser IUser { get; private set; }

        public User(int id, string name, int role)
        {
            Id = id;
            Name = name;
            Role = role;
        }

        public User(UserDTO user)
        {
            if (user == null) return;

            Id = user.Id;
            Name = user.Name;
            Role = user.Role;
        }

        public User(IUser iuser)
        {
            IUser = iuser;
        }

        public UserDTO ToDTO()
        {
            return new UserDTO(Id, Name, Role);
        }

        public bool AttemptLogin(string email, string password)
        {
            return IUser.AttemptLogin(email.ToLower(), password);
        }

        public void Logout()
        {
            IUser.Logout();
        }

        public bool IsLoggedIn()
        {
            return IUser.IsLoggedIn();
        }
    }
}
