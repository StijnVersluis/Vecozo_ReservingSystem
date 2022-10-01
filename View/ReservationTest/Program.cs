using System;
using System.Collections.Generic;
using BusinessLayer;
using DataLayer;
using IntefaceLayer;

namespace ReservationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TeamContainer tCont = new TeamContainer(new TeamDAL());
            UserContainer uCont = new UserContainer(new UserDAL());

            tCont.CreateTeam("team1", new User(3, "timmeh", 1), new List<User> { new User(7,"Kim", 1)});

            Console.WriteLine("Hello World! Please fill in your information");            
        }
    }
}
