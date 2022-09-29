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

        public bool AttemptLogin(string uName, string password)
        {
            return uCont.AttemptLogin(uName.ToLower(), password);
        }

        public User GetLoggedInUser()
        {
            return new User(uCont.GetLoggedInUser());
        }
    }
}
