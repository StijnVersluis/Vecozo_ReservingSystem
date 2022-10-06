using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ViewLayer.Models;

namespace ViewLayer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserContainer uCont = new UserContainer(new UserDAL());

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (uCont.IsLoggedIn()) return View();
            else { return RedirectToAction("Login"); }
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            uCont.Logout();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult Login(IFormCollection collection)
        {
            try
            {
                string name = (string)collection["name"];
                string pass = (string)collection["password"];
                if (name == "" || pass == "") { ViewData["error"] = "Please enter Name and Password!"; return RedirectToAction(""); }

                if (uCont.AttemptLogin(name, pass))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            catch (Exception e)
            {
                ViewData["error"] = e.ToString();
                return RedirectToAction("Index");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
