using BusinessLibrary.Interfaces;
using DomainLibrary.Extentions;
using DomainLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebbApp.ViewModels;

namespace WebbApp.Controllers
{
    public class AuthController(IAuthService authService) : Controller
    {
        private readonly IAuthService _authService = authService;

        public IActionResult SignUp()
        {
            ViewBag.ErrorMessage = "";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            ViewBag.ErrorMessage = "";

            if (!ModelState.IsValid)
                return View(model);

            var signUpFormData = model.MapTo<SignUpFormData>();
            var result = await _authService.SignUpAsync(signUpFormData);
            if (result.Succeeded)
            {
                return RedirectToAction("SignIn", "Auth");
            }

            ViewBag.ErrorMessage = result.Error;
            return View(model);
        }


        public IActionResult SignIn(string returnUrl = "~/")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(MemberSignInViewModel model, string returnUrl="~/")
        {
            ViewBag.ErrorMessage = "";
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Invalid email or password";
                return View(model);
            }
    
            var memberSignInFormData = model.MapTo<MemberSignInFormData>();
            var result = await _authService.SignInAsync(memberSignInFormData);
            if (result.Succeeded)
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }
                return RedirectToAction("Dashboard", "Admin");
            }

            ViewBag.ErrorMessage = result.Error;
            return View(model);
        }


        public new async Task<IActionResult> SignOut()
        {
            await _authService.SignOutAsync();
            return RedirectToAction("SignIn", "Auth");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
