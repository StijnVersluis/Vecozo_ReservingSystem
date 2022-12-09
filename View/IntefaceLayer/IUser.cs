using IntefaceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Net;

namespace IntefaceLayer
{
    public interface IUser
    {
        public bool IsPresent(int id, DateTime datetime);
    }
}
