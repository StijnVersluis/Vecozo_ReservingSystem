using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ViewLayer.Controllers
{
    public class TeamController : Controller
    {
        TeamContainer tCont = new TeamContainer(new TeamDAL());
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(IFormCollection collection)
        {
            //Create a team with members
            throw new NotImplementedException();
        }

        public ActionResult Getusers(int id)
        {
            ViewData["TeamUsers"] = tCont.GetTeam(id).GetUsers(new TeamDAL());
            return View();
        }
    }
}
