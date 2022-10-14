using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ViewLayer.Controllers
{
    public class TeamController : Controller
    {
        TeamContainer tCont = new TeamContainer(new TeamDAL());
        UserContainer uCont = new (new UserDAL());
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewData["Users"] = uCont.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult Create(IFormCollection collection)
        {
            var teamName = (string)collection["Name"];
            var teamMemberIdsString = (string)collection["userids"];
            var teamMemberIds = new List<int>();
            for (int i = 0; i < teamMemberIdsString.Split(", ").Length; i++)
            {
                teamMemberIds.Add(Int32.Parse(teamMemberIdsString.Split(", ")[i]));
            }
            tCont.CreateTeam(teamName, teamMemberIds);
            return View();
        }

        public ActionResult Getusers(int id)
        {
            ViewData["TeamUsers"] = tCont.GetTeam(id).GetUsers(new TeamDAL());
            return View();
        }
    }
}
