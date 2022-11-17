using DataLayer;
using BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using ViewLayer.Models;
using System.Linq;

namespace ViewLayer.Controllers
{
    public class UserController : Controller
    {
        private UserContainer userContainer = new(new UserDAL());
        private RoleContainer roleContainer = new(new RoleDAL());

        [HttpGet("/User/Filter")]
        public ActionResult Filter(string str)
        {
            int userId = userContainer.GetLoggedInUser().Id;
            return View(userContainer.GetFilteredUsers(str).Where(x => x.Id != userId).ToList().ConvertAll(user => new UserViewModel(user, roleContainer.GetRole(user.Role))));
        }
    }
}
