using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BusinessLayer;
using DataLayer;
using IntefaceLayer;

namespace ReservationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("2022-01-01T09:01".Replace(Regex.@T[0-9]{1,2}\:[0-9]{1,2}/g, "T"+"18:00"));
        }
    }
}
