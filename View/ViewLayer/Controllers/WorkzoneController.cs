﻿using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewLayer.Models;
using System;
using System.Collections.Generic;
using ViewLayer.Util;
using System.Linq;
using InterfaceLayer;

namespace ViewLayer.Controllers
{
    [AllowAnonymous]
    public class WorkzoneController : Controller
    {
        private readonly WorkzoneContainer workzoneContainer = new(new WorkzoneDAL());
        private readonly ReservationContainer reservationContainer = new(new ReservationDAL());
        FloorContainer floorContainer = new FloorContainer(new FloorDAL());

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
                    var messages = reservationContainer.CheckReservationRules(new Reservation(0, 0, model.DateTime_Arriving, model.DateTime_Leaving), workzone);
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
        public ActionResult Floor(int id, DateTime date, bool teamOnly = false)
        {
            // Always return the first floor
            if (id == 0) id = 1;

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

            var workzones = workzoneContainer.GetAllFromFloorWithDate(id, formatted_date).Where(workzone => workzone.TeamOnly == teamOnly).ToList().ConvertAll(workzone => new WorkzoneViewModel(workzone));
            return View(workzones);
        }

        public JsonResult GetWorkzonePositions(int id)
        {
            var workzones = workzoneContainer.GetAllFromFloor(id).ConvertAll(workzone => new WorkzoneViewModel(workzone));
            return new JsonResult(workzones);
        }

        [HttpGet]
        public IActionResult GetWorkspace()
        {
            List<WorkzoneViewModel> workzoneViewModels = new List<WorkzoneViewModel>();
            List<Workzone> workzones = workzoneContainer.GetAll();
            foreach (Workzone w in workzones)
            {

                workzoneViewModels.Add(new WorkzoneViewModel(w));
            }

            return View(workzoneViewModels);


        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var workzone = workzoneContainer.GetById(id);
            var model = new WorkzoneViewModel(workzone);
            model.Floors = floorContainer.GetAll().ConvertAll(x => new FloorViewModel(x));
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WorkzoneViewModel workzoneViewModel)
        {
            if (workzoneViewModel != null)
            {
                if (workzoneViewModel.Workspaces < 0)
                {
                    ModelState.AddModelError(String.Empty, "Het aantal werplekken moet groter zijn dan 0");
                    return View(workzoneViewModel);
                }

                Workzone workzone = new Workzone();
                workzone.Id = workzoneViewModel.Id;
                workzone.Name = workzoneViewModel.Name;
                workzone.Xpos = workzoneViewModel.Xpos;
                workzone.Ypos = workzoneViewModel.Ypos;
                workzone.Workspaces = workzoneViewModel.Workspaces;

                var result = workzoneContainer.Edit(workzone);
                if (result)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Het aantal werkplekken zijn niet gewijzigd.");
                    return RedirectToAction("Edit", "Admin", new { id = workzoneViewModel.Id });
                }
            }

            return RedirectToAction("Edit", "Admin", new { id = workzoneViewModel.Id });
        }

        public class FloorJson
        {
            public int floorId;
        }
    }
}
