﻿using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ViewLayer.Models;
using ViewLayer.Util;

namespace ViewLayer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserContainer uCont = new(new UserDAL());
        private TeamContainer tCont = new(new TeamDAL());
        private ReservationContainer rCont = new(new ReservationDAL());
        private WorkzoneContainer wCont = new(new WorkzoneDAL());

        [HttpGet]
        public IActionResult Index()
        {
            if (uCont.IsLoggedIn()) {
                ViewData["AllUserReservations"] = rCont.GetReservationsFromUser(uCont.GetLoggedInUser().Id);
                ViewData["AllWorkzones"] = wCont.GetAll();
                ViewData["TeamsOfUser"] = tCont.GetTeamsOfUser(uCont.GetLoggedInUser().Id);
                ViewData["LoggedInUserName"] = uCont.GetLoggedInUser().Name;
                return View(); 
            }
            else { return RedirectToAction("Login"); }
        }

            ViewData["TeamsOfUser"] = teamContainer.GetTeamsOfUser(userContainer.GetLoggedInUser().Id).ConvertAll(x => new TeamViewModel(x));
            ViewData["Floors"] = floorContainer.GetAll().ConvertAll(x => new FloorViewModel(x));

            this.GetResponse();

            return View();
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("/Error")]
        public IActionResult Error(int statuscode)
        {
            int result;

            switch (statuscode)
            {
                case 401:
                    result = 404;
                    break;

                default:
                    result = 404;
                    break;
            }

        [HttpPost]
        public IActionResult Reserve(IFormCollection collection)
        {
            try
            {
                var dtStart = (string)collection["datetime-start"];
                var tleave = (string)collection["datetime-leaving"];
                var dtleave = Regex.Replace((string)collection["datetime-start"], @"T[0-9]{1,2}\:[0-9]{1,2}", "T" + tleave);
                var workzone = (string)collection["workzone-id"];
                DateTime start = DateTime.Parse(dtStart);
                DateTime leave = DateTime.Parse(dtleave);
                int workzone_id = Int32.Parse(workzone);
                rCont.CreateReservation(new Reservation(uCont.GetLoggedInUser().Id, workzone_id, start, leave));
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewData["error"] = e.ToString();
                return RedirectToAction("Index");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
