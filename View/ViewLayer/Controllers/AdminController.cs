using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using System;
using ViewLayer.Models;
using ViewLayer.Util;

namespace ViewLayer.Controllers
{
    [RequireAuth("Administrator")]
    public class AdminController : Controller
    {
        WorkzoneContainer workzoneContainer = new WorkzoneContainer(new WorkzoneDAL());
        FloorContainer floorContainer = new FloorContainer(new FloorDAL()); 

        public ActionResult Index()
        {
            this.GetResponse();

            return View(workzoneContainer.GetAll().ConvertAll(x => new WorkzoneViewModel(x)));
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
                workzone.Workspaces = workzoneViewModel.Workspaces;

                var result = workzoneContainer.Updateworkspace(workzone);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Het aantal werkplekken zijn niet gewijzigd.");
                    return View(workzoneViewModel);
                }
            }

            return View(workzoneViewModel);
        }

    }
}


