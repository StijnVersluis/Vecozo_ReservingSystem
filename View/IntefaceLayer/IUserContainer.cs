﻿using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntefaceLayer
{
    public interface IUserContainer
    {
        public UserDTO AttemptLogIn(string uName, string password);
    }
}
