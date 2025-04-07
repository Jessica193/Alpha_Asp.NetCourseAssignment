using BusinessLibrary.Services;
using DomainLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebbApp.Controllers
{
    public class AuthController(AuthService authService) : Controller
    {
        private readonly AuthService _authService = authService;

        public IActionResult CreateAccount()
        {
            ViewBag.ErrorMessage = "";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountForm form)
        {
            ViewBag.ErrorMessage = "";

            if (ModelState.IsValid)
            {
                var result = await _authService.CreateAccountAsync(form);
                if (result)
                {
                    return RedirectToAction("LogIn", "Auth");
                }
            }

            return View(form);
        }


        public IActionResult LogIn(string returnUrl = "~/")
        {
            ViewBag.ReturnUrl = returnUrl;
           
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(MemberLogInForm form, string returnUrl="~/")
        {
            ViewBag.ErrorMessage = "";

            if (ModelState.IsValid)
            {
                var result = await _authService.LogInAsync(form);
                if (result)
                {
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    return RedirectToAction("Dashboard", "Admin");
                }
            }
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.ErrorMessage = "Invalid email or password";
            return View(form);
        }

     


        public async Task<IActionResult> LogOut()
        {
            await _authService.LogOutAsync();
            return RedirectToAction("LogIn", "Auth");
        }
    }
}
