using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Net;

namespace IntefaceLayer
{
    public interface IUser
    {
        public void Logout();
        public bool IsLoggedIn();
        public bool AttemptLogin(string email, string password);
    }
}
