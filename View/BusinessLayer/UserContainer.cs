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

        public User AttemptLogin(string uName, string password)
        {
            return new User(uCont.AttemptLogin(uName.ToLower(), password));
        }
    }
}
