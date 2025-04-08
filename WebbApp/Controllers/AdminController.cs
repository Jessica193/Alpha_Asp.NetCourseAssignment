using BusinessLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebbApp.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Members()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Clients()
        {
            return View();
        }
        public IActionResult Projects()
        {
            return View();
        }

    }
}
