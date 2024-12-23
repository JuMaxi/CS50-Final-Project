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
        private readonly IPhotosManagerService _photosService;
        public AdvertController(
            IAdvertService advertService,
            IReCaptchaService reCaptchaService,
            IPhotosManagerService photosService) 
        { 
            _advertService = advertService;
            _reCaptchaService = reCaptchaService;
            _photosService = photosService;
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

                var dto = advert.ConvertToDTO();

                if (advert.Photo1 != null) 
                {
                    var imagePath = await _photosService.ResizeAndUpload(advert.Photo1, maxWidth: 1000, maxHeight: 1000, blobContainer: "adverts");
                    dto.Photos.Add(imagePath);
                }
                if (advert.Photo2 != null)
                {
                    var imagePath = await _photosService.ResizeAndUpload(advert.Photo2, maxWidth: 1000, maxHeight: 1000, blobContainer: "adverts");
                    dto.Photos.Add(imagePath);
                }
                if (advert.Photo3 != null)
                {
                    var imagePath = await _photosService.ResizeAndUpload(advert.Photo3, maxWidth: 1000, maxHeight: 1000, blobContainer: "adverts");
                    dto.Photos.Add(imagePath);
                }
                if (advert.Photo4 != null)
                {
                    var imagePath = await _photosService.ResizeAndUpload(advert.Photo4, maxWidth: 1000, maxHeight: 1000, blobContainer: "adverts");
                    dto.Photos.Add(imagePath);
                }

                var userId = Convert.ToInt32(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

                await _advertService.CreateAdvert(dto, userId);
            }

            return View("CreateAdvert");
        }
    }
}
