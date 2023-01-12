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
using IntefaceLayer;
using System.Collections;

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
            var individualReservations = new Dictionary<string, List<ReservationViewModel>>();
            var individualGroups = reservationContainer.GetReservationsFromUser(userContainer.GetLoggedInUser().Id)
                .GroupBy(x => $"{x.DateTime_Arriving.Day}-{x.DateTime_Arriving.Month}-{x.DateTime_Arriving.Year}");

            foreach (var group in individualGroups)
            {
                individualReservations.Add(group.Key, group.ToList().ConvertAll(x => new ReservationViewModel(x)));
            }

            var teamReservations = new Dictionary<string, List<TeamReservationViewModel>>();
            var teamGroups = reservationContainer.GetReservationsFromTeam(userContainer.GetLoggedInUser().Id)
                .GroupBy(x => $"{x.TimeArriving.Day}-{x.TimeArriving.Month}-{x.TimeArriving.Year}");

            foreach (var group in teamGroups)
            {
                teamReservations.Add(group.Key, group.ToList().ConvertAll(x => new TeamReservationViewModel(x)));
            }

            ViewData["IndividualReservations"] = individualReservations;
            ViewData["TeamReservations"] = teamReservations;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(int id, string redirectUrl = "/")
        {
            bool result = reservationContainer.CancelReservation(id);
            this.SendResponse(
                result,
                "Reservering",
                result ?
                $"is succesvol geannuleerd om {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}" :
                "kan niet worden geannuleerd, probeer het later nog eens."
            );

            return Redirect(redirectUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelTeam(int id, string redirectUrl = "/")
        {
            bool result = reservationContainer.CancelTeamReservation(id);
            this.SendResponse(
                result,
                "Team Reservering",
                result ?
                $"is succesvol geannuleerd om {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}" :
                "kan niet worden geannuleerd, probeer het later nog eens."
            );

            return Redirect(redirectUrl);
        }

        /// <summary>
        /// Try to reserve a workzone with certain parameters.
        /// </summary>
        /// <param name = "model"> The necessary parameters (workzone-id, datetime-arriving and datetime-leaving).</param>
        /// <returns>The index with or success message or error message.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReserveAdHoc(WorkzoneReservationViewModel model)
        {
            bool result = false;
            string message = String.Empty;

            //Get the workzone that the reservation is corresponding to
            Workzone workzone = workzoneContainer.GetById(model.Workzone_id);

            //Go through the checks required for a reservation
            var checks = reservationContainer.CheckReservationRules(new Reservation(userContainer.GetLoggedInUser().Id, workzone.Id, model.DateTime_Arriving, model.DateTime_Leaving), workzone);

            //If any checks fail checks contains errormessages
            if (checks.Count > 0) message = String.Join(',', checks);
            else
            {
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
            }

            string title = !result ? "Mislukt" : "";
            this.SendResponse(
                result,
                $"AdHoc Resevering {title} - ",
                result ?
                    $"{model.Workzone_Name} is succesvol gereserveerd om {model.DateTime_Arriving.ToString("dd/MM/yyyy HH:mm")} tot {model.DateTime_Leaving.ToString("HH:mm")}" :
                    $"{message}"
            );

            return result ? RedirectToAction("Index", "Home") : Redirect(model.RedirectUrl);
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
        [ValidateAntiForgeryToken]
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

                    Workzone workzone = workzoneContainer.GetById(workzoneId);
                    message = $"{workzone?.Name} is succesvol gereserveerd om {startTime.ToString("dd/MM/yyyy HH:mm")} tot {endTime.ToString("HH:mm")}.";
                }


            }
            catch (Exception e) { success = false; message = "Er is iets fout gegaan probeer het later nogmaals."; }

            this.SendResponse(
                success,
                "Team Reservering",
                message
            );

            return RedirectToAction("Index", "Home");
        }
    }
}
