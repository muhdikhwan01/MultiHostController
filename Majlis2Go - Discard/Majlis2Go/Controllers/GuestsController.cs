using Microsoft.AspNetCore.Mvc;

namespace Majlis2Go.Controllers
{
    public class GuestsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
