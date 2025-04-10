using BusinessLibrary.Models;
using DomainLibrary.Models;

namespace BusinessLibrary.Interfaces;

public interface IProjectService
{
    Task<ProjectResult> CreateProjectAsync(AddProjectFormData form);
    Task<ProjectResult<IEnumerable<Project>>> GetAllProjectsAsync();
    Task<ProjectResult<Project>> GetOneProjectAsync(int id);
}
