using Microsoft.AspNetCore.Mvc;

namespace Majlis2Go.Controllers
{
    public class EventsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
