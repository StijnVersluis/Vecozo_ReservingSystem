using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ViewLayer.Models;

namespace ViewLayer.Controllers
{
    public class WorkzoneController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserContainer uCont = new(new UserDAL());
        private TeamContainer tCont = new(new TeamDAL());
        private ReservationContainer rCont = new(new ReservationDAL());
        private WorkzoneContainer wCont = new(new WorkzoneDAL());

        // GET: /workzone/{id}
        [HttpGet("/workzone/{id}")]
        public ActionResult Index(int id)
        {
            var model = wCont.Get(id);
            if (model == null)
            {
                return NotFound();
            }

            var viewmodel = new WorkzoneViewModel
            {
                WorkzoneId = model.Id,
                Name = model.Name,
            };

            return View(viewmodel);
        }
    }
}
