using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using ViewLayer.Models;

namespace ViewLayer.Controllers
{
    public class AuthController : Controller
    {
        private User user = new User(new UserDAL());
        private const string AUTH_SESSION_KEY = "VECOZO_AUTH";

        [HttpGet("/Auth/Login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, [FromQuery(Name = "redirect_uri")] string uri, CancellationToken token = default)
        {
            ActionResult result = null;
            uri = String.IsNullOrEmpty(uri) ? $"{Request.Host}/" : uri;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!user.AttemptLogin(model.Email, model.Password))
            {
                ModelState.AddModelError(String.Empty, "Het e-mailadres of wachtwoord is onjuist.");
                return View(model);
            }

            try
            {
                uri = HttpUtility.UrlDecode(uri);

                HttpContext.Session.SetString(AUTH_SESSION_KEY, model.Email);
                await HttpContext.Session.CommitAsync(token);
            }
            finally
            {
                result = Redirect(Request.IsHttps ? $"https://{uri}" : $"http://{uri}");
            }

            return result;
        }

        [HttpGet]
        public async Task<ActionResult> Logout(CancellationToken token = default)
        {
            ActionResult result = null;

            try
            {
                user.Logout();

                HttpContext.Session.Remove(AUTH_SESSION_KEY);
                await HttpContext.Session.CommitAsync(token);
            } finally
            {
                result = RedirectToAction("Login");
            }

            return result;
        }
    }
}
