using BusinessLibrary.Models;
using DataLibrary.Entities;
using DataLibrary.Interfaces;
using DomainLibrary.Extentions;
using DomainLibrary.Models;

namespace BusinessLibrary.Services;

public class ProjectService(IProjectRepository projectRepository)
{
    private readonly IProjectRepository _projectRepository = projectRepository;


    public async Task<ProjectResult> CreateProjectAsync(AddProjectsForm form)
    {
        var projectEntity = form.MapTo<ProjectEntity>();




        return new ProjectResult { Succeeded = true, StatusCode = 201 };
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

        return new ProjectResult<IEnumerable<Project>> {
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
