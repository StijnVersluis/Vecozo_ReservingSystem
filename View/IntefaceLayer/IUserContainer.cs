using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntefaceLayer
{
    public interface IUserContainer
    {
        public List<UserDTO> GetAll();
        public UserDTO GetUserById(int id);
        public List<UserDTO> GetFilteredUsers(string filterStr);
        public UserDTO FindUserByEmail(string email);
        public UserDTO GetLoggedInUser();
        public bool AttemptLogin(string email, string password);
        public bool IsLoggedIn();
        public void Logout();
    }
}
