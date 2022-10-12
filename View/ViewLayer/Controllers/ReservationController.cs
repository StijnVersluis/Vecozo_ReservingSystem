using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using ViewLayer.Models;

namespace ViewLayer.Controllers
{
    public class ReservationController : Controller
    {
        ReservationContainer _reservationContainer=new ReservationContainer(new ReservationDAL());  
      
        [HttpGet]
        public ActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddBook(ReservationViewModel reservationmodel)
        {
            try
            {
                Reservation reservation = new Reservation(reservationmodel.User_id, reservationmodel.Workzone_id, reservationmodel.DateTime_Leaving, reservationmodel.DateTime_Arriving);
                    
                _reservationContainer.CreateReservation(reservation);
                return RedirectToAction("index", "Home");
            }
            catch
            {
                return RedirectToAction("index", "home");
            }

        }
    }
}
