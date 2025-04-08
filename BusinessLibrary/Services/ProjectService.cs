using BusinessLibrary.Models;
using DataLibrary.Entities;
using DataLibrary.Interfaces;
using DomainLibrary.Extentions;
using DomainLibrary.Models;

namespace BusinessLibrary.Services;

public interface IProjectService
{
    Task<ProjectResult> CreateProjectAsync(AddProjectFormData form);
    Task<ProjectResult<IEnumerable<Project>>> GetAllProjectsAsync();
    Task<ProjectResult<Project>> GetOneProjectAsync(int id);
}

public class ProjectService(IProjectRepository projectRepository, IStatusService statusService) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IStatusService _statusService = statusService;


    public async Task<ProjectResult> CreateProjectAsync(AddProjectFormData form)
    {
        if (form == null)
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Not all required fields are supplied" };

        var projectEntity = form.MapTo<ProjectEntity>();
        var statusResult = await _statusService.GetOneStatusByIdAsync(form.StatusId); //Hans skrev bara 1 ist för form.StatusId. Varför?
        var statusModel = statusResult.Result;

        projectEntity.StatusId = statusModel!.Id;

        var result = await _projectRepository.CreateAsync(projectEntity);

        return result.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 201 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }


    public async Task<ProjectResult<IEnumerable<Project>>> GetAllProjectsAsync()
    {
        var response = await _projectRepository.GetAllAsync
            (
                orderByDescendning: true,
                sortBy: s => s.Created,
                where: null,
                include => include.Members,
                include => include.Client,
                include => include.Status
            );

        return new ProjectResult<IEnumerable<Project>>
        {
            Succeeded = true,
            StatusCode = 200,
            Result = response.Result
        };
    }

    public async Task<ProjectResult<Project>> GetOneProjectAsync(int id)
    {
        var response = await _projectRepository.GetOneAsync
            (
                where: x => x.Id == id,
                include => include.Members,
                include => include.Client,
                include => include.Status
            );

        return response.Succeeded
            ? new ProjectResult<Project> { Succeeded = true, StatusCode = 200, Result = response.Result }
            : new ProjectResult<Project> { Succeeded = false, StatusCode = 404, Error = "Project not found" };
    }
}
