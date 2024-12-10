using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropagatingKindness.Models;
using PropagatingKindness.Models.Account;

namespace PropagatingKindness.Controllers
{
    public class AccountController : Controller
    {
        private static string user = "test@test.com";
        private static string pass = "secret";

        [HttpGet]
        public IActionResult Login()
        {
            // E se o usuario ja estiver logado?
            //   Redirecionar para Home
            // E se o usuario errar a senha?
            //   Mostrar a view de novo, mas dessa vez com uma mensagem.
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(string InputEmail, string InputPassword)
        {
            if (InputEmail == user && InputPassword == pass)
            {
                // Create the C# Claims and SignIn the User
                List<Claim> claims = [new(ClaimTypes.Name, InputEmail)];
                var claimsId = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsId));

                HttpContext.Session.SetString(Constants.LoggedUserNameSessionKey, InputEmail); // this will be better with the actual name

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(new LoginViewModel() { ErrorMessage = "Invalid email or password provided. Please try again." });
            }
        }

        public IActionResult CreateAccount()
        {
            return View();
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
