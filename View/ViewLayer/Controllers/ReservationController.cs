using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System;
using ViewLayer.Models;
using Newtonsoft.Json;
using ViewLayer.Util;
using System.Collections.Generic;
using System.Linq;

namespace ViewLayer.Controllers
{
    public class ReservationController : Controller
    {
        private ReservationContainer reservationContainer = new ReservationContainer(new ReservationDAL());
        private WorkzoneContainer workzoneContainer = new WorkzoneContainer(new WorkzoneDAL());
        private UserContainer userContainer = new (new UserDAL());

        private const string AUTH_SESSION_STATE_KEY = "VECOZO_AUTH_STATE";

        [HttpGet]
        public ActionResult Index()
        {
            var reservations = new Dictionary<string, List<ReservationViewModel>>();
            var groups = reservationContainer.GetReservationsFromUser(userContainer.GetLoggedInUser().Id)
                .GroupBy(x => $"{x.DateTime_Arriving.Day}-{x.DateTime_Arriving.Month}-{x.DateTime_Arriving.Year}");

            foreach (var group in groups)
            {
                reservations.Add(group.Key, group.ToList().ConvertAll(x => new ReservationViewModel(x)
                {
                    Workzone = workzoneContainer.GetById(x.Workzone_id)
                }));
            }

            return View(reservations);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(int id)
        {
            bool result = reservationContainer.CancelReservation(id);
            this.SendResponse(
                result,
                "Reservering",
                result ? 
                $"is succesvol geannuleerd om {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}" :
                "kan niet worden geannuleerd, probeer het later nog eens."
            );

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reserve(IFormCollection collection)
        {
            ActionResult result = null;
            bool success = false;
            string message = String.Empty;

            try
            {
                var dtStart = (string)collection["datetime-start"];
                var tleave = (string)collection["datetime-leaving"];

                if (!String.IsNullOrEmpty(dtStart) && !String.IsNullOrEmpty(tleave))
                {
                    var dtleave = Regex.Replace((string)collection["datetime-start"], @"T[0-9]{1,2}\:[0-9]{1,2}", "T" + tleave);
                    DateTime start = DateTime.Parse(dtStart);
                    DateTime leave = DateTime.Parse(dtleave);
                    int workzone_id = Int32.Parse((string)collection["workzone-id"]);

                    Workzone workzone = workzoneContainer.GetById(workzone_id);
                    if (workzone != null)
                    {
                        // There are still workspaces left and the workzone is not only for teams.
                        if (workzone.Workspaces != 0 && !workzone.TeamOnly)
                        {
                            var checks = reservationContainer.CheckGeneralRules(new Reservation(0, 0, start, leave));
                            if (checks.Count != 0)
                            {
                                message = String.Join(',', checks);
                            }
                            else
                            {
                                success = reservationContainer.CreateReservation(new Reservation(userContainer.GetLoggedInUser().Id, workzone_id, start, leave));
                                message = success ?
                                    $"{workzone.Name} is succesvol gereserveerd om {start.ToString("dd/MM/yyyy HH:mm")} tot {leave.ToString("HH:mm")}" :
                                    $"{workzone.Name} kan niet gereserveerd worden, probeer het later nog eens.";
                            }
                        } else
                        {
                            message = $"{workzone.Name} kan niet gereserveerd worden, probeer het later nog eens.";
                        }
                    }
                } else
                {
                    message = "Zorg ervoor dat de datums en tijden correct zijn ingevoerd!";
                }

                this.SendResponse(
                    success,
                    "Reservering",
                    message
                );
            } finally
            {
                result = RedirectToAction("Index", "Home");
            }

            return result;
        }

        [HttpGet]
        public ActionResult Reserve(WorkzoneReservationViewModel model)
        {
            bool result = false;

            string json = HttpContext.Session.GetString(AUTH_SESSION_STATE_KEY);
            if (!String.IsNullOrEmpty(json))
            {
                var obj = JsonConvert.DeserializeObject<WorkzoneReservationViewModel>(json);
                HttpContext.Session.Remove(AUTH_SESSION_STATE_KEY);

                result = reservationContainer.CreateReservation(new Reservation(userContainer.GetLoggedInUser().Id, obj.Workzone_id, obj.DateTime_Arriving, obj.DateTime_Leaving));
            }
            else
            {
                result = reservationContainer.CreateReservation(new Reservation(userContainer.GetLoggedInUser().Id, model.Workzone_id, model.DateTime_Arriving, model.DateTime_Leaving));
            }

            this.SendResponse(
                result,
                "Reservering",
                result ?
                $"{model.Workzone_Name} is succesvol gereserveerd om {model.DateTime_Arriving.ToString("dd/MM/yyyy HH:mm")} tot {model.DateTime_Leaving.ToString("HH:mm")}" :
                $"{model.Workzone_Name} kan niet gereserveerd worden, probeer het later nog eens."
            );

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Cancel(int id)
        {
            try
            {
                _reservationContainer.CancelReservation(id);
                return RedirectToAction("index", "Home");
            }
            catch
            {
                return RedirectToAction("index", "home");
            }
        }
    }
}
