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
            Team team = new Team(5, "team1");
            Console.WriteLine("Removed In: " + team.AddUser(new User(13, "user3", 1), new TeamDAL()));
        }
    }
}
