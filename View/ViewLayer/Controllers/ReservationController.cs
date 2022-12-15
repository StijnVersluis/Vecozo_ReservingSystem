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
using Newtonsoft.Json.Serialization;

namespace ViewLayer.Controllers
{
    public class ReservationController : Controller
    {
        private ReservationContainer reservationContainer = new ReservationContainer(new ReservationDAL());
        private WorkzoneContainer workzoneContainer = new WorkzoneContainer(new WorkzoneDAL());
        private UserContainer userContainer = new(new UserDAL());

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

            return RedirectToAction("Index", "Reservation");
        }

        /// <summary>
        /// Try to reserve a workzone with certain parameters.
        /// </summary>
        /// <param name="collection">The necessary parameters (workzone-id, datetime-arriving and datetime-leaving).</param>
        /// <returns>The index with or success message or error message.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reserve(IFormCollection collection)
        {
            ActionResult result = null; //The view to return to
            bool success = false;
            string message = String.Empty;

            try
            {
                //Get parameters from form/collection
                var filledInStart = collection["datetime-start"];
                var filledInEnd = collection["datetime-leaving"];
                int workzone_id = Int32.Parse(collection["workzone-id"]); //Is dynamicly set, so is always filled in.

                //Check if everything is filled in.
                if (String.IsNullOrEmpty(filledInStart) ||
                    String.IsNullOrEmpty(filledInEnd))
                    throw new Exception("Zorg ervoor dat de data en tijden correct zijn ingevoerd!");

                //Set the datetime-leaving to a datetime to easily get the time
                DateTime dtFilledInLeave = DateTime.Parse(filledInEnd);

                //Create the starting datetime and ending datetime
                DateTime start = DateTime.Parse(filledInStart);
                DateTime leave = new DateTime(start.Year, start.Month, start.Day,
                    dtFilledInLeave.Hour, dtFilledInLeave.Minute, dtFilledInLeave.Second);
                
                //Get the workzone that the reservation is corresponding to
                Workzone workzone = workzoneContainer.GetById(workzone_id);

                //Go through the checks required for a reservation
                var checks = reservationContainer.CheckReservationRules(new Reservation(userContainer.GetLoggedInUser().Id, workzone.Id, start, leave), workzone);

                //If any checks fail checks contains errormessages
                if (checks.Count > 0) message = String.Join(',', checks);
                else
                {
                    //Else create the reservation
                    success = reservationContainer.CreateReservation(new Reservation(userContainer.GetLoggedInUser().Id, workzone.Id, start, leave));
                    message = success ?
                        $"{workzone.Name} is succesvol gereserveerd om {start.ToString("dd/MM/yyyy HH:mm")} tot {leave.ToString("HH:mm")}" :
                        $"{workzone.Name} kan niet gereserveerd worden, probeer het later nog eens.";
                }
            }
            catch (Exception e) { message = e.Message; }
            finally
            {
                result = RedirectToAction("Index", "Home");
            }


            this.SendResponse(
                    success,
                    "Reservering",
                    message
                );
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
                // A regular request from endpoint but AdHoc in order to reserve a workplace.
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
    }
}
