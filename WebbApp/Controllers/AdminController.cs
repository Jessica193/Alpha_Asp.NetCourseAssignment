using BusinessLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebbApp.Controllers
{
    
    public class AdminController : Controller
    {

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Members()
        {
            return View();
        }


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
