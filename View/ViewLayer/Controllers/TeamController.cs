using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using ViewLayer.Models;
using ViewLayer.Util;

namespace ViewLayer.Controllers
{
    public class TeamController : Controller
    {
        private static TeamDAL tDAL = new TeamDAL();
        private TeamContainer teamContainer = new(tDAL);
        private UserContainer userContainer = new(new UserDAL());
        private RoleContainer roleContainer = new(new RoleDAL());

        [HttpGet]
        public ActionResult Index()
        {
            this.GetResponse();

            return View(
                teamContainer.GetTeamsOfUser(
                    userContainer.GetLoggedInUser().Id).ConvertAll(team => new TeamViewModel(team)
                    {
                        Users = teamContainer.GetTeam(team.Id).GetUsers(new TeamDAL())
                    }
                )
            );
        }

        [HttpGet("/Team/Details/{id}")]
        public ActionResult Details(int id)
        {
            Team team = teamContainer.GetTeam(id);
            if (team == null)
            {
                return NotFound();
            }
            return View(new TeamViewModel(team)
            {
                Owner = teamContainer.GetTeamAdmin(team.Id),
                Users = team.GetUsers(tDAL)
            });
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewData["Users"] = userContainer.GetAll().ConvertAll(user => new UserViewModel(user, roleContainer.GetRole(user.Role)));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TeamViewModel model)
        {
            List<int> teamMemberIds = new List<int>();
            bool result = false;
            string message = String.Empty;

            try
            {
                model.Owner = teamContainer.GetTeamAdmin(model.Id);
                if (!string.IsNullOrEmpty(model.AddedUserIds))
                {
                    for (int i = 0; i < model.AddedUserIds.Split(",").Length; i++)
                    {
                        teamMemberIds.Add(Int32.Parse(model.AddedUserIds.Split(",")[i]));
                    }
                }

                var checks = teamContainer.CheckEditRules(model.Name, userContainer.GetLoggedInUser().Id, teamMemberIds);
                if (checks.Count != 0)
                {
                    checks.ForEach(msg => ModelState.AddModelError(String.Empty, msg));
                    return View("Details", model);
                }

                result = teamContainer.EditTeam(new Team(model.Id, model.Name), teamMemberIds);
                message = result ?
                    $"{model.Name} is succesvol gewijzigd om {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}" :
                    $"{model.Name} kan niet worden gewijzigd, probeer het later nog eens.";
            }
            catch (Exception)
            {
                ModelState.AddModelError(String.Empty, "Er ontstonden problemen bij het wijzigen van het team!");
                return View("Details", model);
            }

            this.SendResponse(result, "Team", message);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Leave(TeamViewModel model)
        {
            bool result = false;
            string message = String.Empty;

            try
            {
                int currentUserId = userContainer.GetLoggedInUser().Id;
                model.Owner = teamContainer.GetTeamAdmin(model.Id);

                if (teamContainer.IsTeamAdmin(model.Id, currentUserId))
                {
                    ModelState.AddModelError(String.Empty, "Als teambeheerder kunt u het team niet verlaten!");
                    return View("Details", model);
                }

                result = teamContainer.LeaveTeam(model.Id, currentUserId);
                message = result ?
                    $"Uw heeft het team verlaten om {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}" :
                    $"Het team kan niet worden verlaten, probeer het later nog eens.";
            } catch(Exception)
            {
                ModelState.AddModelError(String.Empty, "Er ontstonden problemen bij het verlaten van het team!");
                return View("Details", model);
            }

            this.SendResponse(result, "Team", message);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            var teamName = (string)collection["Name"];
            var teamMemberIdsString = (string)collection["userids"];
            var teamMemberIds = new List<int>();

            if (!string.IsNullOrEmpty(teamMemberIdsString))
            {
                for (int i = 0; i < teamMemberIdsString.Split(",").Length; i++)
                {
                    teamMemberIds.Add(Int32.Parse(teamMemberIdsString.Split(",")[i]));
                }
            }

            var checks = teamContainer.CheckGeneralRules(teamName, userContainer.GetLoggedInUser().Id, teamMemberIds);
            if (checks.Count != 0)
            {
                foreach (var check in checks)
                {
                    ModelState.AddModelError("Name", check);
                }

                return View();
            }

            bool result = teamContainer.CreateTeam(teamName, teamMemberIds);
            this.SendResponse(
                result, 
                "Team",
                result ?
                $"{teamName} is succesvol aangemaakt om {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}" :
                $"{teamName} kan niet worden aangemaakt, probeer het later nog eens."
            );

            return RedirectToAction("Index");
        }

        [HttpGet("/Team/Users/{id}")]
        public ActionResult Users(int id)
        {
            ViewData["TeamUsers"] = teamContainer.GetTeam(id).GetUsers(new TeamDAL());
            return View();
        }

        [HttpGet("/Team/Info/{id}")]
        public ActionResult Info(int id)
        {
            Team team = teamContainer.GetTeam(id);
            if (team == null)
            {
                return NotFound();
            }

            return Json(new TeamViewModel(team)
            {
                Owner = teamContainer.GetTeamAdmin(team.Id),
                Users = new Team(id).GetUsers(new TeamDAL())
            });
        }
        public ActionResult Delete(int id)
        {
            if (teamContainer.GetTeam(id).GetUsers(tDAL).Count == 1)
            {
                teamContainer.DeleteTeam(id);
                return RedirectToAction(nameof(Index));
            }
            else return RedirectToAction(nameof(Details), new { id = id });
        }
    }
}
