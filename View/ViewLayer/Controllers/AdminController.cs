using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewLayer.Models;
using ViewLayer.Util;

namespace ViewLayer.Controllers
{
    [RequireAuth("Administrator")]
    public class AdminController : Controller
    {
        WorkzoneContainer workzoneContainer = new WorkzoneContainer(new WorkzoneDAL());
        FloorContainer floorContainer = new FloorContainer(new FloorDAL()); 

        public IActionResult Index()
        {
            return View(workzoneContainer.GetAll().ConvertAll(x => new WorkzoneViewModel(x)));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var workzone = workzoneContainer.GetById(id);
            var model = new WorkzoneViewModel(workzone);
            model.Floors = floorContainer.GetAll().ConvertAll(x => new FloorViewModel(x));
            return View(model);
        }
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(WorkzoneViewModel workzoneViewModel)
        {
            if (workzoneViewModel != null)
            {
                if (workzoneViewModel.Workspaces < 0)
                {
                    return View(workzoneViewModel);
                }
                Workzone workzone = new Workzone();
                workzone.Id = workzoneViewModel.Id;
                workzone.Workspaces = workzoneViewModel.Workspaces;
                var resut = workzoneContainer.Updateworkspace(workzone);
                if (resut)
                {

                    return RedirectToAction("GetWorkspace", "Workzone");
                }
                else
                {
                    return View(workzoneViewModel);
                }
            }
            return View(workzoneViewModel);

        }

    }
}


