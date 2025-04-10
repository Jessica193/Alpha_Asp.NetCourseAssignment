using BusinessLibrary.Interfaces;
using BusinessLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebbApp.ViewModels;

namespace WebbApp.Controllers
{
    [Authorize]
    public class AdminController(IClientService clientService, IMemberService memberService) : Controller
    {
        private readonly IClientService _clientService = clientService;
        private readonly IMemberService _memberService = memberService;

        public IActionResult Dashboard()
        {
            return View();
        }

        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Members()
        {
            var viewModel = new MembersViewModel(_memberService);
            await viewModel.PopulateMembersAsync();
            return View(viewModel);
        }

        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Clients()
        {
            var viewModel = new ClientsViewModel(_clientService);
            await viewModel.PopulateClientsAsync();
            return View(viewModel);
        }
        public IActionResult Projects()
        {
            return View();
        }

    }
}
