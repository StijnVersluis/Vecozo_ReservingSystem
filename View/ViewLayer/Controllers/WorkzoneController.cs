using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewLayer.Models;
using System;
//using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace ViewLayer.Controllers
{
    [AllowAnonymous]
    public class WorkzoneController : Controller
    {
        private readonly WorkzoneContainer workzoneContainer = new(new WorkzoneDAL());
        private readonly ReservationContainer reservationContainer = new(new ReservationDAL());

        [HttpGet("/AdHoc/{id}")]
        public ActionResult Index(int id)
        {
            var result = workzoneContainer.GetByDateAndId(id, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            if (result == null)
            {
                return NotFound();
            }

            return View(new WorkzoneReservationViewModel
            {
                Workzone_Name = result.Name,
                Workzone_id = result.Id,
                Workspaces = result.Workspaces
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ValidateData(WorkzoneReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(String.Empty, "Zorg ervoor dat de datum/tijden correct zijn ingevulgd!");
                return View("Index", model);
            }

            Workzone workzone = workzoneContainer.GetById(model.Workzone_id);
            if (workzone != null)
            {
                // There are still workspaces left and the workzone is not only for teams.
                if (workzone.Workspaces != 0 && !workzone.TeamOnly)
                {
                    var messages = reservationContainer.CheckGeneralRules(new Reservation(0, 0, model.DateTime_Arriving, model.DateTime_Leaving));
                    if (messages.Count != 0)
                    {
                        messages.ForEach(x => ModelState.AddModelError(String.Empty, x));
                        return View("Index", model);
                    }
                }
                else
                {
                    ModelState.AddModelError(String.Empty, $"{workzone.Name} kan niet gereserveerd worden, probeer het later nog eens.");
                }
            }

            return RedirectToAction("Reserve", "Reservation", model);
        }

        [HttpGet("/Workzone/Floor/{id}")]
        public ActionResult Floor(int id, DateTime date)
        {
            // Always return the first floor
            if (id == 0)
            {
                id = 1;
            }

            var formatted_date = string.Empty;
            var hasDefaultDate = date == default(DateTime);
            if (hasDefaultDate)
            {
                formatted_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }
            else
            {
                formatted_date = date.ToString("yyyy-MM-dd HH:mm");
            }

            var workzones = workzoneContainer.GetAllFromFloorWithDate(id, formatted_date).ConvertAll(workzone => new WorkzoneViewModel(workzone));
            return View(workzones);
        }

        public JsonResult GetWorkzonePositions(int id)
        {
            var workzones = workzoneContainer.GetAllFromFloor(id).ConvertAll(workzone => new WorkzoneViewModel(workzone));
            return new JsonResult(workzones);
        }
    }
    public class FloorJson
    {
        public int floorId;
    }
}
