using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewLayer.Util;

namespace ViewLayer.Controllers
{
    [RequireAuth("Administrator")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
