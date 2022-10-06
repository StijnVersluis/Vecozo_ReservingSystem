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
            Console.WriteLine("Logged In: " + uCont.AttemptLogin("timmeh", "timmeh123"));
        }
    }
}
