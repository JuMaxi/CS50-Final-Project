using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropagatingKindness.Domain.DTO;
using PropagatingKindness.Domain.Interfaces;
using PropagatingKindness.Domain.Models;
using PropagatingKindness.Models.Advert;
using PropagatingKindness.Services;

namespace PropagatingKindness.Controllers
{
    public class AdvertController : Controller
    {   
        private readonly IAdvertService _advertService;
        private readonly IReCaptchaService _reCaptchaService;
        private readonly IPhotosManagerService _photosService;
        private readonly IUserService _userService;

        public AdvertController(
            IAdvertService advertService,
            IReCaptchaService reCaptchaService,
            IPhotosManagerService photosService,
            IUserService userService) 
        { 
            _advertService = advertService;
            _reCaptchaService = reCaptchaService;
            _photosService = photosService;
            _userService = userService;
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

        private async Task<AdvertDTO> SavePhotos(CreateAdvertViewModel advert)
        {
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
            return dto;
        }

        private int GetUserId()
        {
            return Convert.ToInt32(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
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

                await _advertService.CreateAdvert(await SavePhotos(advert), GetUserId());
            }
            return RedirectToAction("MyAdverts");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyAdverts()
        {
            var userAdverts = await _advertService.GetAllUserAdverts(GetUserId());

            return View(MyAdvertsViewModel.FromAdverts(userAdverts));
        }

        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            List<AdvertStatus> allowedStatus = [AdvertStatus.Available, AdvertStatus.Promissed];
            var advert = await _advertService.GetAdvertById(id);

            if (allowedStatus.Contains(advert.Status))
            {
                return View(ViewAdvertViewModel.FromAdvert(advert));
            }
            else
            {
                if (HttpContext.User?.Identity?.IsAuthenticated ?? false)
                {
                    // User is logged in
                    var user = await _userService.GetById(GetUserId());
                    if (advert.User.Id == user.Id || user.IsAdmin)
                    {
                        return View(ViewAdvertViewModel.FromAdvert(advert));
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // User is NOT logged in
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Promisse(int id)
        {
            // This method retrieves the advert by ID, check if the current user owns it,
            //       check if the current status allows changing to Promissed, change the status, and redirect to /MyAdverts

            var result = await _advertService.PromisseAdvert(GetUserId(), id);

            if (result.Success)
            {
                return RedirectToAction("MyAdverts");
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Donate(int id)
        {
            // This method retrieves the advert by ID, check if the current user owns it,
            //       check if the current status allows changing to Donated, change the status, and redirect to /MyAdverts
            
            var result = await _advertService.DonateAdvert(GetUserId(), id);

            if (result.Success)
            {
                return RedirectToAction("MyAdverts");
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Available(int id)
        {
            //  This method retrieves the advert by ID, check if the current user owns it (Or, the current user is admin),
            //       check if the current status allows changing it back to Available, change the status, and redirect to /MyAdverts

            var result = await _advertService.ActivateAdvert(GetUserId(), id);

            if (result.Success)
            {
                return RedirectToAction("MyAdverts");
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Deactivate(int id)
        {
            // This method retrieves the advert by ID, check if the current user owns it (Or, the current user is admin),
            //       check if the current status allows changing to Deactivated, change the status, and redirect to /MyAdverts
            var result = await _advertService.DeactivateAdvert(GetUserId(), id);

            if (result.Success)
            {
                return RedirectToAction("MyAdverts");
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // This method retrieves the advert by ID, check if the current user owns it,
            //       check if the current status allows editing, and render the view for the user to edit it
            // User can't change the photos. If it is needed, must Inactive the current advert and create a new one.

            var result = await _advertService.CheckUserOwnsAdvert(GetUserId(), id);

            if (result.Success) 
            {
                return View(EditAdvertViewModel.FromAdvert(result.Content));
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditAdvertViewModel advertView, IFormCollection form)
        {
            if (string.IsNullOrWhiteSpace(form["g-recaptcha-response"]))
            {
                advertView.ErrorMessage = "Please solve the captcha challenge";
                return View(advertView);
            }

            if (ModelState.IsValid)
            {
                var recaptcha = await _reCaptchaService.ValidateRecaptcha(form["g-recaptcha-response"]);
                if (!recaptcha.Success)
                {
                    advertView.ErrorMessage = recaptcha.ErrorMessage;
                    return View(advertView);
                }

                var dto = advertView.ConvertToDTO();
                dto.Id = id;
                dto.UserId = GetUserId();
                    
                var result = await _advertService.UpdateAdvert(dto);

                if (result.Success) 
                {
                    return RedirectToAction("MyAdverts");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        [RequiresAdmin]
        [HttpGet]
        public async Task<IActionResult> Pending()
        {
            var pending = await _advertService.GetAllPendingAdverts();
            var viewModel = PendingAdvertsViewModel.FromAdverts(pending);
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> All(int page)
        {
            // This method retrieves all adverts that are Available or Promissed. There is no need to be logged in
            //      to have this view

            var adverts = await _advertService.GetAllAvailableAndPromissedAdverts(page);

            return View(AllAvailablePromissedAdvertsViewModel.FromAdverts(adverts.Content));
        }
    }
}
