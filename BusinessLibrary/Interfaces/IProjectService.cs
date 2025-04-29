using BusinessLibrary.Models;
using DomainLibrary.Models;

namespace BusinessLibrary.Interfaces;

public interface IProjectService
{
    Task<ProjectResult> CreateProjectAsync(AddProjectFormData form);
    Task<ProjectResult> DeleteProjectAsync(int id);
    Task<ProjectResult> EditProjectAsync(EditProjectFormData form);
    Task<ProjectResult<IEnumerable<Project>>> GetAllProjectsAsync();
    Task<ProjectResult<Project>> GetProjectByIdAsync(int id);
    Task<ProjectResult<IEnumerable<Project>>> GetProjectsByStatusAsync(string status);
}
