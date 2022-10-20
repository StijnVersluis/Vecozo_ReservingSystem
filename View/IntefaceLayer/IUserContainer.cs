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
        public bool AttemptLogin(string email, string password);
        public bool IsLoggedIn();
        public void Logout();
        public UserDTO GetLoggedInUser();
        public List<UserDTO> GetFilteredUsers(string filterStr);
    }
}
