using Microsoft.AspNetCore.Mvc;

namespace PropagatingKindness.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
