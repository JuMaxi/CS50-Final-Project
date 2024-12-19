using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropagatingKindness.Domain.Interfaces;
using PropagatingKindness.Models.Account;
using PropagatingKindness.Services;

namespace PropagatingKindness.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IReCaptchaService _reCaptchaService;
        private readonly IPhotosManagerService _photosService;

        public AccountController(
            IUserService userService, 
            IReCaptchaService reCaptchaService,
            IPhotosManagerService photosService)
        {
            _userService = userService;
            _reCaptchaService = reCaptchaService;
            _photosService = photosService;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if (HttpContext.User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(string InputEmail, string InputPassword)
        {
            var authenticated = await _userService.Authenticate(InputEmail, InputPassword);

            if (authenticated.Success)
            {
                // Create the C# Claims and SignIn the User
                List<Claim> claims = 
                [
                    new(ClaimTypes.Email, authenticated.User.Email), 
                    new(ClaimTypes.Name, authenticated.User.FirstName), 
                    new(ClaimTypes.NameIdentifier, authenticated.User.Id.ToString())
                ];

                var claimsId = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsId));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(new LoginViewModel() { ErrorMessage = authenticated.ErrorMessage });
            }
        }

        [HttpGet]
        public IActionResult CreateAccount()
        {
            if (HttpContext.User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountViewModel account, IFormCollection form)
        {
            if (string.IsNullOrWhiteSpace(form["g-recaptcha-response"]))
            {
                account.ErrorMessage = "Please solve the captcha challenge";
                return View(account);
            }

            if (ModelState.IsValid)
            {
                var recaptcha = await _reCaptchaService.ValidateRecaptcha(form["g-recaptcha-response"]);
                if (!recaptcha.Success)
                {
                    account.ErrorMessage = recaptcha.ErrorMessage;
                    return View(account);
                }

                var imagePath = await _photosService.ResizeAndUpload(account.Photo, maxWidth: 500, maxHeight: 500, blobContainer: "users");
                var dto = account.ConvertToDTO();
                dto.Photo = imagePath;

                var result = await _userService.CreateAccount(dto);

                if (result.Success)
                    return RedirectToAction("Login", "Account");
                else
                {
                    account.ErrorMessage = result.ErrorMessage;
                    return View(account);
                }
            }

            return View(account);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ManageAccount()
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            
            var user = await _userService.GetById(userId);

            var account = new ManageAccountViewModel();
            
            return View(account.FromUser(user));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var user = await _userService.GetById(userId);

            return View(EditProfileViewModel.FromUser(user));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel accountView, IFormCollection form)
        {
            if (string.IsNullOrWhiteSpace(form["g-recaptcha-response"]))
            {
                accountView.ErrorMessage = "Please solve the captcha challenge";
                return View(accountView);
            }

            if (ModelState.IsValid)
            {
                var recaptcha = await _reCaptchaService.ValidateRecaptcha(form["g-recaptcha-response"]);
                if (!recaptcha.Success)
                {
                    accountView.ErrorMessage = recaptcha.ErrorMessage;
                    return View(accountView);
                }

                var result = await _userService.Update(accountView.ConvertToDTO());

                if (result.Success)
                    return RedirectToAction("Login", "Account");
                else
                {
                    accountView.ErrorMessage = result.ErrorMessage;
                    return View(accountView);
                }
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

    }
}
