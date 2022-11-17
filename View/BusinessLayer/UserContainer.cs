using IntefaceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class UserContainer
    {
        private IUserContainer uCont;
        private IUser user;

        public UserContainer(IUserContainer iUCont) { uCont = iUCont; }

        public List<User> GetAll()
        {
            return uCont.GetAll().ConvertAll(x => new User(x));
        }

        public User GetUserById(int id)
        {
            return new User(uCont.GetUserById(id));
        }


        public User FindUserByEmail(string email)
        {
            return new User(uCont.FindUserByEmail(email));
        }

        public User GetLoggedInUser()
        {
            return new User(uCont.GetLoggedInUser());
        }

        public List<User> GetFilteredUsers(string filterStr)
        {
            return uCont.GetFilteredUsers(filterStr).ConvertAll(userdto=>new User(userdto));
        }
    }
}
