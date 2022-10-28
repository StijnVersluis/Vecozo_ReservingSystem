using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Text.RegularExpressions;
using System;
using ViewLayer.Models;

namespace ViewLayer.Controllers
{
    public class ReservationController : Controller
    {
        ReservationContainer rCont = new ReservationContainer(new ReservationDAL());  
        UserContainer uCont = new (new UserDAL());  

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
                    
                rCont.CreateReservation(reservation);
                return RedirectToAction("index", "Home");
            }
            catch
            {
                return RedirectToAction("index", "home");
            }

        }

        public ActionResult Cancel(int id)
        {
            try
            {
                rCont.CancelReservation(id);
                return RedirectToAction("index", "Home");
            }
            catch
            {
                return RedirectToAction("index", "home");
            }
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
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
