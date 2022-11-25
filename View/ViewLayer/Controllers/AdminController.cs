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

    }
}


