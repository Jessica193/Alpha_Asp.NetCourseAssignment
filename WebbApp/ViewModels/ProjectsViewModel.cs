using DomainLibrary.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebbApp.ViewModels;

public class ProjectsViewModel
{
    public AddProjectViewModel AddProjectForm { get; set; } = new AddProjectViewModel();
    public EditProjectViewModel EditProjectForm { get; set; } = new EditProjectViewModel();
    public IEnumerable<Project> Projects { get; set; } = [];
    public int CountAllProjects => Projects.Count();
    public int CountStartedProjects => Projects.Count(p => p.Status.Status == "Started");
    public int CountNotStartedProjects => Projects.Count(p => p.Status.Status == "Not started");
    public int CountCompletedProjects => Projects.Count(p => p.Status.Status == "Completed");
    public IEnumerable<SelectListItem> AvailableStatuses { get; set; } = [];
    public IEnumerable<SelectListItem> AvailableMembers { get; set; } = [];
    public IEnumerable<SelectListItem> AvailableClients { get; set; } = [];
}
