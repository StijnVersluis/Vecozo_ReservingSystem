using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using ViewLayer.Models;

namespace ViewLayer.Controllers
{
    public class WorkzoneController : Controller
    {
        private readonly WorkzoneContainer workzoneContainer= new(new WorkzoneDAL());

        [HttpPost]
        public ActionResult GetFloor([FromBody] FloorJson collection)
        {
            var workzones = workzoneContainer.GetAllFromFloor(collection.floorId).ConvertAll(workzone => new WorkzoneViewModel(workzone));
            return View(workzones);
        }
    }

    public class FloorJson
    {
        public int floorId { get; set; }
    }
}
