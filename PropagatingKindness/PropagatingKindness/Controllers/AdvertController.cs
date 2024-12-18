using Microsoft.AspNetCore.Mvc;
using PropagatingKindness.Models.Advert;

namespace PropagatingKindness.Controllers
{
    public class AdvertController : Controller
    {
        public IActionResult Donation() 
        { 
            return View();
        }

        [HttpGet]
        public IActionResult CreateAdvert()
        {
            return View("CreateAdvert");
        }

        [HttpPost]
        public IActionResult CreateAdvert(CreateAdvertViewModel model)
        {
            return View("CreateAdvert");
        }
    }
}
