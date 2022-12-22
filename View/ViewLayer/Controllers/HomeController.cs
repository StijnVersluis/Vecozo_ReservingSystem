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
using ViewLayer.Util;

namespace ViewLayer.Controllers
{
    public class HomeController : Controller
    {
        private UserContainer uCont = new(new UserDAL());
        private TeamContainer tCont = new(new TeamDAL());
        private ReservationContainer rCont = new(new ReservationDAL());
        private WorkzoneContainer wCont = new(new WorkzoneDAL());
        private FloorContainer fCont = new(new FloorDAL());

        [HttpGet]
        public IActionResult Index()
        {
            ViewData["IndividualReservations"] = rCont.GetReservationsFromUser(uCont.GetLoggedInUser().Id).Where(reservation => reservation.DateTime_Arriving.Date == DateTime.Now.Date).ToList().ConvertAll(reservation => new ReservationViewModel(reservation));
            ViewData["TeamReservations"] = rCont.GetReservationsFromTeam(uCont.GetLoggedInUser().Id).Where(reservation => reservation.TimeArriving.Date == DateTime.Now.Date).ToList().ConvertAll(reservation => new TeamReservationViewModel(reservation));

            ViewData["TeamsOfUser"] = tCont.GetTeamsOfUser(uCont.GetLoggedInUser().Id).ConvertAll(team => new TeamViewModel(team));
            ViewData["LoggedInUserName"] = uCont.GetLoggedInUser().Name;
            ViewData["Floors"] = fCont.GetAll().ConvertAll(x => new FloorViewModel(x));

            var workzones = wCont.GetAll();
            List<WorkzoneViewModel> workzoneViewModels = new();
            //workzones.ForEach(workzone =>
            //{
            //    workzoneViewModels.Add(new WorkzoneViewModel(workzone, workzone.GetAvailableWorkspaces(DateTime.Now, new WorkzoneDAL())));
            //});
            ViewData["Workzones"] = workzoneViewModels;
            this.GetResponse();
            return View();
        }

        [HttpGet("Home/BhvRegister")]
        public ActionResult BhvRegister(DateTime datetime)
        {
            if (datetime == default(DateTime) || datetime.ToString() == "01/01/0001 00:00:00") datetime = DateTime.Now;
            var users = uCont.GetAll().Where(user => user.IsBhv).ToList();

            var uDal = new UserDAL();
            var bhvUsers = new List<BhvRegisterViewModel>();
            users.ForEach(user =>
            {
                bhvUsers.Add(new BhvRegisterViewModel(user, user.IsPresent(datetime, uDal)));
            });
            bhvUsers = bhvUsers.OrderBy(user => !user.IsAvailable).ToList();
            ViewData["DateTime"] = datetime.ToString();
            return View(bhvUsers);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("/Error")]
        public IActionResult Error(int statusCode)
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = statusCode
            });
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
    }
}
