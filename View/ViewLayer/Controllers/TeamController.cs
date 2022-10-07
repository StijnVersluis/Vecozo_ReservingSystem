using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Mvc;

namespace ViewLayer.Controllers
{
    public class TeamController : Controller
    {
        TeamContainer tCont = new TeamContainer(new TeamDAL());
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Getusers(int id)
        {
            ViewData["TeamUsers"] = tCont.GetTeam(id).GetUsers(new TeamDAL());
            return View();
        }
    }
}
