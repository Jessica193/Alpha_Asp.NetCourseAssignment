using BusinessLibrary.Interfaces;
using BusinessLibrary.Models;
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
    public class AdminController(IClientService clientService, IMemberService memberService, IStatusService statusService, IProjectService projectService, RoleManager<IdentityRole> roleManager) : Controller
    {
        private readonly IClientService _clientService = clientService;
        private readonly IMemberService _memberService = memberService;
        private readonly IStatusService _statusService = statusService;
        private readonly IProjectService _projectService = projectService;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public IActionResult Dashboard()
        {
            return View();
        }

        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Members()
        {
            var availableRoles = await PopulateAvailableRolesAsync();

            var addMemberViewModel = new AddMemberViewModel
            {
                AvailableRoles = availableRoles
            };

            var editMemberViewModel = new EditMemberViewModel
            {
                AvailableRoles = availableRoles
            };

            var viewModel = new MembersViewModel
            {
                Members = await PopulateMembersAsync(),
                AddMemberForm = addMemberViewModel,
                AvailableRoles = availableRoles,
                EditMemberForm = editMemberViewModel
            };

            return View(viewModel);
        }

        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Clients()
        {
            var viewModel = new ClientsViewModel(_clientService);
            await viewModel.PopulateClientsAsync();
            return View(viewModel);
        }

        public async Task<IActionResult> Projects()
        {
            var availableStatuses = await PopulateAvailableStatusesAsync();
            var availableMembers = await PopulateAvailableMembersAsync();
            var availableClients = await PopulateAvailableClientsAsync();

            var addprojectViewModel = new AddProjectViewModel
            {
                AvailableStatuses = availableStatuses,
                AvailableMembers = availableMembers,
                AvailableClients = availableClients
            };

            var editProjectViewModel = new EditProjectViewModel
            {
                AvailableStatuses = availableStatuses,
                AvailableMembers = availableMembers,
                AvailableClients = availableClients
            };

            var viewModel = new ProjectsViewModel
            {
                Projects = await PopulateProjectsAsync(),
                AddProjectForm = addprojectViewModel,
                EditProjectForm = editProjectViewModel,
                AvailableStatuses = availableStatuses,
                AvailableMembers = availableMembers,
                AvailableClients = availableClients
            };

            return View(viewModel);
        }




        public async Task<IEnumerable<SelectListItem>> PopulateAvailableStatusesAsync()
        {
            var statusResult = await _statusService.GetAllStatusesAsync();
            if (!statusResult.Succeeded)
            {
                return new List<SelectListItem>();
            }
            var statuses = statusResult.Result;
            if (statuses == null || !statuses.Any())
            {
                return new List<SelectListItem>();
            }
            return statuses.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Status
            });
        }

        public async Task<IEnumerable<SelectListItem>> PopulateAvailableMembersAsync()
        {
            var memberResult = await _memberService.GetMembersAsync();
            if (!memberResult.Succeeded)
            {
                return new List<SelectListItem>();
            }
            var members = memberResult.Result;
            if (members == null || !members.Any())
            {
                return new List<SelectListItem>();
            }
            return members.Select(m => new SelectListItem
            {
                Value = m.Id,
                Text = $"{m.FirstName} {m.LastName}"
            });
        }

        public async Task<IEnumerable<SelectListItem>> PopulateAvailableClientsAsync()
        {
            var clientResult = await _clientService.GetAllClientsAsync();
            if (!clientResult.Succeeded)
            {
                return new List<SelectListItem>();
            }
            var clients = clientResult.Result;
            if (clients == null || !clients.Any())
            {
                return new List<SelectListItem>();
            }
            return clients.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.ClientName
            });
        }


        public async Task<IEnumerable<Project>> PopulateProjectsAsync()
        {
            var result = await _projectService.GetAllProjectsAsync();
            if (result.Succeeded)
            {
                return result.Result?.ToList() ?? [];
            }
            return [];
        }


        public async Task<IEnumerable<Member>> PopulateMembersAsync()
        {
            var result = await _memberService.GetMembersAsync();
            if (result.Succeeded)
            {
                return result.Result?.ToList() ?? [];
            }
            return [];
        }

        public async Task<IEnumerable<SelectListItem>> PopulateAvailableRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            });
        }
    }
}
