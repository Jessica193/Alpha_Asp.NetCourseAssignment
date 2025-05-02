using BusinessLibrary.Interfaces;
using DataLibrary.Entities;
using DomainLibrary.Extentions;
using DomainLibrary.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using WebbApp.ViewModels;

namespace WebbApp.Controllers
{
    public class AuthController(IAuthService authService, SignInManager<MemberEntity> signInManager, UserManager<MemberEntity> userManager) : Controller
    {
        private readonly IAuthService _authService = authService;
        private readonly SignInManager<MemberEntity> _signInManager = signInManager;
        private readonly UserManager<MemberEntity> _userManager = userManager;


        #region Local Identity

        #region SignUp
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
        #endregion

        #region AdminSignIn

        public IActionResult AdminSignIn(string returnUrl = "~/")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AdminSignIn(MemberSignInViewModel model, string returnUrl = "~/")
        {
            ViewBag.ErrorMessage = "";
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Invalid email or password";
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.IsInRoleAsync(user, "Admin"))
            {
                ViewBag.ErrorMessage = "Only admins can log in here";
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


        #endregion

        #region SignIn
        public IActionResult SignIn(string returnUrl = "~/")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(MemberSignInViewModel model, string returnUrl = "~/")
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
        #endregion

        #region SignOut
        public new async Task<IActionResult> SignOut()
        {
            await _authService.SignOutAsync();
            return RedirectToAction("SignIn", "Auth");
        }
        #endregion

        #region AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }
        #endregion

        #endregion

        #region External Authentication

        [HttpPost]
        public IActionResult ExternalSignIn(string provider, string returnUrl = null!)
        {
            if (string.IsNullOrEmpty(provider))
            {
                ModelState.AddModelError("", "Invalid provider");
                return View("SignIn");
            }

            var redirectUrl = Url.Action("ExternalSignInCallback", "Auth", new { returnUrl })!;
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);

        }

        public async Task<IActionResult> ExternalSignInCallback(string returnUrl = null!, string remoteError = null!)
        {
            returnUrl ??= Url.Content("~/");
            if (!string.IsNullOrEmpty(remoteError))
            {
                ModelState.AddModelError("", $"Error from external provider: {remoteError}");
                return View("SignIn");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction("SignIn");

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                string firstName = string.Empty;
                string lastName = string.Empty;

                try
                {
                    firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "unknown";
                    lastName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "unknown";
                }
                catch { }

                string email = info.Principal.FindFirstValue(ClaimTypes.Email)!;
                string userName = $"ext_{info.LoginProvider.ToLower()}_{email}";

                var user = new MemberEntity
                {
                    UserName = userName,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                };

                var identityResult = await _userManager.CreateAsync(user);
                if (identityResult.Succeeded)
                {
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }

                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("SignIn");
            }
        }

        #endregion
    }
}
