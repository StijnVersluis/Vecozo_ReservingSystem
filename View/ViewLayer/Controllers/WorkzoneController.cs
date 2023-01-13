using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using ViewLayer.Models;
using System;
using System.Collections.Generic;
using ViewLayer.Util;

namespace ViewLayer.Controllers
{

    public class WorkzoneController : Controller
    {
        private static readonly WorkzoneDAL wDAL = new WorkzoneDAL();
        private readonly WorkzoneContainer workzoneContainer = new(wDAL);
        private readonly ReservationContainer reservationContainer = new(new ReservationDAL());
        private readonly FloorContainer floorContainer = new FloorContainer(new FloorDAL());

        [HttpGet("/AdHoc/{id}")]
        public ActionResult Index(int id)
        {
            var result = workzoneContainer.GetById(id);
            if (result == null)
            {
                return NotFound();
            }

            this.GetResponse();
            return View(new WorkzoneReservationViewModel
            {
                Workzone_Name = result.Name,
                Workzone_id = result.Id,
                Workspaces = result.Workspaces
            });
        }

        [HttpGet]
        public JsonResult GetWorkzonePositions(int id)
        {
            var workzones = workzoneContainer.GetAllFromFloor(id).ConvertAll(workzone => new WorkzoneViewModel(workzone, workzone.GetAvailableWorkspaces(DateTime.Now, wDAL)));
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
            if (workzone == null)
            {
                return NotFound();
            }

            var model = new WorkzoneViewModel(workzone)
            {
                HasReservations = workzone.HasReservations(new WorkzoneDAL())
            };

            model.Floors = floorContainer.GetAll().ConvertAll(x => new FloorViewModel(x));
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WorkzoneViewModel workzoneViewModel)
        {
            bool success = false;
            string message = string.Empty;
            Workzone workzone = workzoneContainer.GetById(workzoneViewModel.Id);
            if (workzone == null)
            {
                return NotFound();
            }

            workzoneViewModel.Floors = floorContainer.GetAll().ConvertAll(x => new FloorViewModel(x));

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(String.Empty, "Voer alle velden in!");
                return View(workzoneViewModel);
            }

            var messages = workzoneContainer.CheckEditRules(new Workzone(workzoneViewModel.Id, workzoneViewModel.Name, workzoneViewModel.Workspaces, workzoneViewModel.Floor, workzoneViewModel.TeamOnly), new WorkzoneDAL());
            if (messages.Count > 0)
            {
                messages.ForEach(x => ModelState.AddModelError(String.Empty, x));
                return View(workzoneViewModel);
            } else
            {
                success = workzoneContainer.Edit(new Workzone(
                    workzoneViewModel.Id,
                    workzoneViewModel.Name,
                    workzoneViewModel.Workspaces,
                    workzoneViewModel.Floor,
                    workzoneViewModel.TeamOnly,
                    workzoneViewModel.Xpos,
                    workzoneViewModel.Ypos
                ));
            }

            if (!success)
            {
                ModelState.AddModelError(String.Empty, "Het wijzigen van een werkplek is mislukt, probeer het later nog eens.");
                return RedirectToAction("Edit", new { id = workzoneViewModel.Id });
            }

            this.SendResponse(
                true,
                "Werkblok Wijziging",
                $"De werkblok {workzoneViewModel.Name} is succesvol gewijzigd."
            );

            return RedirectToAction("Index", "Admin");
        }

        public class FloorJson
        {
            public int floorId;
        }
    }
}
