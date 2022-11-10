using BusinessLayer;
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

namespace ViewLayer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserContainer uCont = new(new UserDAL());
        private TeamContainer tCont = new(new TeamDAL());
        private ReservationContainer rCont = new(new ReservationDAL());
        private WorkzoneContainer wCont = new(new WorkzoneDAL());

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (uCont.IsLoggedIn()) {
                ViewData["AllUserReservations"] = rCont.GetReservationsFromUser(uCont.GetLoggedInUser().Id).ConvertAll(reservation => new ReservationViewModel(reservation));
                ViewData["AllWorkzones"] = wCont.GetAllFromFloor(1).ConvertAll(workzone => new WorkzoneViewModel(workzone));
                ViewData["TeamsOfUser"] = tCont.GetTeamsOfUser(uCont.GetLoggedInUser().Id).ConvertAll(x => new TeamViewModel(x));
                ViewData["LoggedInUserName"] = uCont.GetLoggedInUser().Name;
                return View(); 
            }
            else { return RedirectToAction("Login"); }
        }

        public ActionResult Login()
        {
            if (uCont.IsLoggedIn()) return RedirectToAction(nameof(Index));
            return View();
        }

        public ActionResult Logout()
        {
            uCont.Logout();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult Login(IFormCollection collection)
        {
            try
            {
                string name = (string)collection["name"];
                string pass = (string)collection["password"];
                if (name == "" || pass == "") { ViewData["error"] = "Please enter Name and Password!"; return RedirectToAction(""); }

                if (uCont.AttemptLogin(name, pass))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Login");
                }
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult Outsourcing()
        {
            if (uCont.IsLoggedIn())
            {
                List<ReservationViewModel> reservations = rCont.GetReservationsFromUser(uCont.GetLoggedInUser().Id).ConvertAll(reservation => new ReservationViewModel(reservation));
                List<WorkzoneViewModel> workzones = wCont.GetAll().ConvertAll(workzone => new WorkzoneViewModel(workzone));
                List<TeamViewModel> TeamsOfUsers = tCont.GetTeamsOfUser(uCont.GetLoggedInUser().Id).ConvertAll(x => new TeamViewModel(x));
                UserViewModel LoggedInUser = new( uCont.GetLoggedInUser());
                OutsourcingReservationViewModel model = new(reservations, workzones, TeamsOfUsers, LoggedInUser);

                return View(model);
            }
            else { return RedirectToAction("Login"); }
        }
        [HttpPost]
        public IActionResult OutsourcingFilter(OutsourcingReservationViewModel model)
        {
            WorkZoneFinder Finder = new();
            List<Workzone> workzones = wCont.GetAllFromFloor(model.SelectedFloor.GetValueOrDefault());
            List<Reservation> reservations = rCont.GetAllReservations();
            model.TeamsOfUser = tCont.GetTeamsOfUser(uCont.GetLoggedInUser().Id).ConvertAll(x => new TeamViewModel(x));
            List<User> users = new(); //this list will be filled with the selected users 
            users.Add(uCont.GetLoggedInUser());// list van Users word standaard gevuld met de ingelogde user 
            users.Add(uCont.GetAll().Where(user => user.Id == 7).Single());// comment this to test for a single users and not a team
            model.AllWorkzones = Finder.AvailableWorkzones(workzones, reservations, users,
                                                model.dateTime_Planned_Start.GetValueOrDefault(), model.dateTime_Planned_Leaving.GetValueOrDefault()
                                                ,model.IsTeam
                                                ).ConvertAll(workzone => new WorkzoneViewModel(workzone));
            model.SelectedUsers = users;
            return View("Outsourcing", model);
        }
    }
}
