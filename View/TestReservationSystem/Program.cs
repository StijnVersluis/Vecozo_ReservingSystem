using BusinessLayer;
using DataLayer;
using System;

namespace TestReservationSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            UserContainer uCont = new UserContainer(new UserDAL());
            uCont.AttemptLogin("Timmeh", "Tim123");
        }
    }
}
