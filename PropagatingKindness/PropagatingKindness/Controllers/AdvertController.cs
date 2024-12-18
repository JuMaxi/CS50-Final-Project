using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropagatingKindness.Domain.Interfaces;
using PropagatingKindness.Models.Advert;
using PropagatingKindness.Services;

namespace PropagatingKindness.Controllers
{
    public class AdvertController : Controller
    {   
        private readonly IAdvertService _advertService;
        private readonly IReCaptchaService _reCaptchaService;
        public AdvertController(IAdvertService advertService, IReCaptchaService reCaptchaService) 
        { 
            _advertService = advertService;
            _reCaptchaService = reCaptchaService;
        }

        public IActionResult Donation() 
        { 
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult CreateAdvert()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAdvert(CreateAdvertViewModel advert, IFormCollection form)
        {
            if (string.IsNullOrWhiteSpace(form["g-recaptcha-response"]))
            {
                advert.ErrorMessage = "Please solve the captcha challenge";
                return View(advert);
            }

            if (ModelState.IsValid)
            {
                var recaptcha = await _reCaptchaService.ValidateRecaptcha(form["g-recaptcha-response"]);
                if (!recaptcha.Success)
                {
                    advert.ErrorMessage = recaptcha.ErrorMessage;
                    return View(advert);
                }

                var userId = Convert.ToInt32(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

                await _advertService.CreateAdvert(advert.ConvertToDTO(), userId);

                
            }

            return View("CreateAdvert");
        }
    }
}
