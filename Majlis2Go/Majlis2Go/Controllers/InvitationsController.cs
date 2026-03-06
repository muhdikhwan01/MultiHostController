using Microsoft.AspNetCore.Mvc;

namespace Majlis2Go.Controllers
{
    public class InvitationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
