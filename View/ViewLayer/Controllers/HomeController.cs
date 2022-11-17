using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ViewLayer.Models;
using ViewLayer.Util;

namespace ViewLayer.Controllers
{
    public class HomeController : Controller
    {
        private UserContainer userContainer = new(new UserDAL());
        private TeamContainer teamContainer = new(new TeamDAL());
        private ReservationContainer reservationContainer = new(new ReservationDAL());
        private WorkzoneContainer workzoneContainer = new(new WorkzoneDAL());
        private FloorContainer floorContainer = new(new FloorDAL());

        [HttpGet]
        public IActionResult Index()
        {
            ViewData["UserReservations"] = reservationContainer.GetTodayReservationsFromUser(userContainer.GetLoggedInUser().Id).ConvertAll(reservation => new ReservationViewModel(reservation)
            {
                Workzone = workzoneContainer.GetById(reservation.Workzone_id)
            });

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

            return View(
                new ErrorViewModel { 
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    StatusCode = result
                }
            );
        }
    }
}
