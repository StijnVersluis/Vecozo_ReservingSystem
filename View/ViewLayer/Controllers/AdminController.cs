using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ViewLayer.Models;
using ViewLayer.Util;

namespace ViewLayer.Controllers
{
    [RequireAuth("Administrator")]
    public class AdminController : Controller
    {
        WorkzoneContainer workzoneContainer = new WorkzoneContainer(new WorkzoneDAL());
        TeamContainer teamContainer = new TeamContainer(new TeamDAL());
        UserContainer userContainer = new UserContainer(new UserDAL());

        [HttpGet]
        public ActionResult Index()
        {
            this.GetResponse();

            return View(workzoneContainer.GetAll().ConvertAll(x => new WorkzoneViewModel(x)).OrderBy(workzone=> workzone.Floor));
        }

        [HttpGet]
        public ActionResult Teams()
        {
            return View(
                teamContainer.GetArchivedTeams().ConvertAll(team => new TeamViewModel(team)
                {
                    Owner = teamContainer.GetTeamAdmin(team.Id)
                })
            );
        }
    }
}


