using BusinessLibrary.Interfaces;
using BusinessLibrary.Models;
using BusinessLibrary.Services;
using DomainLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebbApp.ViewModels;

namespace WebbApp.Controllers
{
    [Authorize]
    public class AdminController(IClientService clientService, IMemberService memberService, RoleManager<IdentityRole> roleManager) : Controller
    {
        private readonly IClientService _clientService = clientService;
        private readonly IMemberService _memberService = memberService;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public IActionResult Dashboard()
        {
            return View();
        }

        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Members()
        {
            var viewModel = new MembersViewModel();
            viewModel.Members = await PopulateMembersAsync();
            viewModel.AvailableRoles = await PopulateAvailableRolesAsync();
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


        public async Task<IEnumerable<Member>> PopulateMembersAsync()
        {
            var result = await _memberService.GetMembersAsync();
            if (result.Succeeded)
            {
                if (result.Result == null)
                    return [];

                var members = result.Result.ToList();
                return members;
            }
            return [];
        }

        public async Task<IEnumerable<SelectListItem>> PopulateAvailableRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var availableRoles = roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            });
            return availableRoles;
        }

    }

    }


