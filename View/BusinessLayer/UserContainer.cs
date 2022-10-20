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

        public UserContainer(IUserContainer iUCont) { uCont = iUCont; }

        public List<User> GetAll()
        {
            return uCont.GetAll().ConvertAll(x => new User(x));
        }

        public bool AttemptLogin(string email, string password)
        {
            return uCont.AttemptLogin(email.ToLower(), password);
        }
        public bool IsLoggedIn()
        {
            return uCont.IsLoggedIn();
        }
        public void Logout()
        {
            uCont.Logout();
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
