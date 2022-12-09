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
        public bool IsBhv { private set; get; }

        public IUser IUser { get; private set; }

        public User(int id, string name, int role, bool isBhv)
        {
            Id = id;
            Name = name;
            Role = role;
            IsBhv = isBhv;
        }

        public User(UserDTO user)
        {
            if (user == null) return;

            Id = user.Id;
            Name = user.Name;
            Role = user.Role;
            IsBhv = user.IsBhv;
        }

        public User(IUser iuser)
        {
            IUser = iuser;
        }

        public UserDTO ToDTO()
        {
            return new UserDTO(Id, Name, Role, IsBhv);
        }

        public bool IsPresent(DateTime datetime, IUser iuser)
        {
            return iuser.IsPresent(this.Id, datetime);
        }
    }
}
