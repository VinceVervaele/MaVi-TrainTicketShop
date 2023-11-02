using Microsoft.AspNetCore.Mvc;
using Trains_FSD.Extensions;
using Trains_FSD.ViewModels;

namespace Trains_FSD.Controllers
{
    public class SessionController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.SetObject("mySession", new SessionVM { Date = DateTime.Now, Company = "VIVES" });

            return View();
        }
    }
}
