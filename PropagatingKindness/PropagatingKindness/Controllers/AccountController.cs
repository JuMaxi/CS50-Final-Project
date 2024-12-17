using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropagatingKindness.Domain.Interfaces;
using PropagatingKindness.Models;
using PropagatingKindness.Models.Account;

namespace PropagatingKindness.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
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
        public async Task<IActionResult> CreateAccount(CreateAccountViewModel account)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.CreateAccount(account.ConvertToDTO());

                if (result.Success)
                    return RedirectToAction("Index", "Home");
                else
                {
                    account.ErrorMessage = result.ErrorMessage;
                    return View(account);
                }
            }

            return View(account);
        }

        [Authorize]
        public IActionResult Edit()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}
