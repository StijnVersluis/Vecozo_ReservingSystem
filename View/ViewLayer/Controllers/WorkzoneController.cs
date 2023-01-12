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
            var model = new WorkzoneViewModel(workzone);
            model.Floors = floorContainer.GetAll().ConvertAll(x => new FloorViewModel(x));
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WorkzoneViewModel workzoneViewModel)
        {
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

            if (workzoneViewModel.Workspaces < 0)
            {
                ModelState.AddModelError(String.Empty, "Het aantal werplekken moet groter zijn dan 0");
                return View(workzoneViewModel);
            }

            bool sucess = workzoneContainer.Edit(new Workzone(                
                workzoneViewModel.Id,
                workzoneViewModel.Name,
                workzoneViewModel.Workspaces,
                workzoneViewModel.Floor,
                workzoneViewModel.TeamOnly,
                workzoneViewModel.Xpos,
                workzoneViewModel.Ypos
            ));

            if (!sucess)
            {
                ModelState.AddModelError(String.Empty, "Het wijzigen van een werkplek is mislukt, probeer het later nog eens.");
                return RedirectToAction("Edit", new { id = workzoneViewModel.Id });
            }

            this.SendResponse(
                sucess,
                "Werkblok Wijziging",
                $"De werkblok {workzoneViewModel.Name} is succesvol gewijzigd."
            );

            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public IActionResult DeleteWorkspace(WorkzoneViewModel model)
        {
            try
            {
                workzoneContainer.DeleteWorkzone(model.Id);

                return RedirectToAction("index","Workzone");
            }
            catch
            {
                return RedirectToAction("Error", "Shared");
            }
        }

        [HttpGet]
        public IActionResult DeleteWorkspace(int id)
        {
            try
            {
                Workzone workzone = workzoneContainer.GetById(id);

                WorkzoneViewModel model = new WorkzoneViewModel(workzone);

                return View(model);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
    }
}
