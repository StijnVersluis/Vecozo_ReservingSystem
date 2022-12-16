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
using Microsoft.AspNetCore.Identity;

namespace ViewLayer.Controllers
{
    public class ReservationController : Controller
    {
        private ReservationContainer reservationContainer = new ReservationContainer(new ReservationDAL());
        private WorkzoneContainer workzoneContainer = new WorkzoneContainer(new WorkzoneDAL());
        private UserContainer userContainer = new(new UserDAL());
        private TeamContainer teamContainer = new(new TeamDAL());

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
        /// <param name = "collection" > The necessary parameters (workzone-id, datetime-arriving and datetime-leaving).</param>
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

        [HttpPost]
        public ActionResult ReserveTeam(IFormCollection collection)
        {
            bool success = false;
            string message = string.Empty;
            try
            {
                //Get parameters from form/collection
                string filledInStartStr = collection["datetime-start"];
                string filledInEndStr = collection["datetime-leaving"];
                string userIdsStr = collection["team-member-ids"];
                string teamId = collection["team-id"];
                int workzoneId = Int32.Parse(collection["workzone-id"]); //Is dynamicly set, so is always filled in.

                List<User> users = new List<User>();
                DateTime startTime = DateTime.Parse(filledInStartStr);
                DateTime endTimeTemp = DateTime.Parse(filledInEndStr);
                DateTime endTime = new DateTime(startTime.Year, startTime.Month, startTime.Day,
                    endTimeTemp.Hour, endTimeTemp.Minute, 0);

                //foreach(string id in userIdsStr.Split(", "))
                //{
                //    users.Add(userContainer.GetUserById(Convert.ToInt32(id)));
                //}

                Team team = teamContainer.GetTeam(Convert.ToInt32(teamId));
                users = team.GetUsers(new TeamDAL());

                var teamReservation = new TeamReservation(team.Id, startTime, endTime,
                    new List<int>() { workzoneId }, users.Select(user => user.Id).ToList());

                var errorMessages = reservationContainer.CheckTeamReservationRules(teamReservation);

                if (errorMessages.Count > 0) message = String.Join(",", errorMessages);
                else
                {
                    reservationContainer.CreateTeamReservation(teamReservation);
                    success = true;
                }


            }
            catch (Exception e) { success = false; message = "Er is iets fout gegaan probeer het laten normaals"; }

            this.SendResponse(
                    success,
                    "Reservering",
                    message
                );

            return RedirectToAction("Index", "Home");
        }
    }
}
