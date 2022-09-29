using System;
using BusinessLayer;
using DataLayer;

namespace ReservationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            UserContainer uCont = new UserContainer(new UserDAL());

            Console.WriteLine("Hello World! Please fill in your information");
            Console.Write("Username: ");
            var username = Console.ReadLine();

            Console.WriteLine();
            Console.Write("Password: ");

            var password = Console.ReadLine();

            uCont.AttemptLogin(username, password);

            Console.WriteLine(uCont.GetLoggedInUser().Name);
        }
    }
}
