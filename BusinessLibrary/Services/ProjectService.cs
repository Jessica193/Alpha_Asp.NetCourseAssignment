using BusinessLibrary.Interfaces;
using BusinessLibrary.Models;
using DataLibrary.Entities;
using DataLibrary.Interfaces;
using DomainLibrary.Extentions;
using DomainLibrary.Models;

namespace BusinessLibrary.Services;

public class ProjectService(IProjectRepository projectRepository, IMemberRepository memberRepository) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IMemberRepository _memberRepository = memberRepository;


    public async Task<ProjectResult> CreateProjectAsync(AddProjectFormData form)
    {
        if (form == null)
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Not all required fields are supplied" };

        var projectEntity = form.MapTo<ProjectEntity>();
        projectEntity.ClientId = form.ClientId;
        projectEntity.StatusId = form.StatusId;

        var memberEntities = new List<MemberEntity>();
        foreach (var memberId in form.MemberIds)
        {
            var memberResult = await _memberRepository.GetOneEntityAsync(x => x.Id == memberId, include => include.Address!);
            if (memberResult.Succeeded && memberResult.Result != null)
            {
                memberEntities.Add(memberResult.Result);
            }
        }
        projectEntity.Members = memberEntities;

        var result = await _projectRepository.CreateAsync(projectEntity);

        return result.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 201 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }


    public async Task<ProjectResult<IEnumerable<Project>>> GetAllProjectsAsync()
    {
        var entitiesResponse = await _projectRepository.GetAllEntitiesAsync(
            orderByDescendning: true,
            sortBy: s => s.Created
        );

        if (!entitiesResponse.Succeeded)
            return new ProjectResult<IEnumerable<Project>> { Succeeded = false, StatusCode = 500, Error = "Could not load projects." };

        if (entitiesResponse.Result == null || !entitiesResponse.Result.Any())
            return new ProjectResult<IEnumerable<Project>> { Succeeded = false, StatusCode = 404, Error = "No projects found." };

        var projects = entitiesResponse.Result.Select(entity => new Project
        {
            Id = entity.Id,
            ImagePath = entity.ImagePath,
            ProjectName = entity.ProjectName,
            Description = entity.Description,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Budget = entity.Budget,
            ClientId = entity.ClientId,
            StatusId = entity.StatusId,
            Members = entity.Members.Select(m => new Member
            {
                Id = m.Id,
                ImagePath = m.ImagePath,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Email = m.Email!,
                PhoneNumber = m.PhoneNumber,
                JobTitle = m.JobTitle,
                DateOfBirth = m.DateOfBirth,
                Address = m.Address != null 
                ? new MemberAddress
                {
                    UserId = m.Address.UserId,
                    StreetName = m.Address.StreetName,
                    PostalCode = m.Address.PostalCode,
                    City = m.Address.City
                } 
                : null
            }).ToList(),
            Client = new Client
            {
                Id = entity.Client.Id,
                ImagePath = entity.Client.ImagePath,
                ClientName = entity.Client.ClientName,
                Email = entity.Client.Email,
                Phone = entity.Client.Phone,
                Location = entity.Client.Location,
            },
            Status = new StatusModel
            {
                Id = entity.Status.Id,
                Status = entity.Status.Status
            }
        });

        return new ProjectResult<IEnumerable<Project>> { Succeeded = true, StatusCode = 200, Result = projects };
    }

    public async Task<ProjectResult<Project>> GetOneProjectAsync(int id)
    {
        var entityResponse = await _projectRepository.GetOneEntityAsync(x => x.Id == id);

        if (!entityResponse.Succeeded)
            return new ProjectResult<Project> { Succeeded = false, StatusCode = 500, Error = "Could not load project." };

        if (entityResponse.Result == null)
            return new ProjectResult<Project> { Succeeded = false, StatusCode = 404, Error = "No project found." };

        var entity = entityResponse.Result;

        var project = new Project
        {
            Id = entity.Id,
            ImagePath = entity.ImagePath,
            ProjectName = entity.ProjectName,
            Description = entity.Description,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Budget = entity.Budget,
            ClientId = entity.ClientId,
            StatusId = entity.StatusId,
            Members = entity.Members.Select(m => new Member
            {
                Id = m.Id,
                ImagePath = m.ImagePath,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Email = m.Email!,
                PhoneNumber = m.PhoneNumber,
                JobTitle = m.JobTitle,
                DateOfBirth = m.DateOfBirth,
                Address = m.Address != null
                ? new MemberAddress
                {
                    UserId = m.Address.UserId,
                    StreetName = m.Address.StreetName,
                    PostalCode = m.Address.PostalCode,
                    City = m.Address.City
                }
                : null
            }).ToList(),
            Client = new Client
            {
                Id = entity.Client.Id,
                ImagePath = entity.Client.ImagePath,
                ClientName = entity.Client.ClientName,
                Email = entity.Client.Email,
                Phone = entity.Client.Phone,
                Location = entity.Client.Location,
            },
            Status = new StatusModel
            {
                Id = entity.Status.Id,
                Status = entity.Status.Status
            }
        };

        return new ProjectResult<Project> { Succeeded = true, StatusCode = 200, Result = project };
    }

    public async Task<ProjectResult> EditProjectAsync(EditProjectFormData form)
    {
        if (form == null)
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Not all required fields are supplied" };

        var projectEntityResponse = await _projectRepository.GetOneEntityAsync(x => x.Id == form.Id);
        if (!projectEntityResponse.Succeeded)
            return new ProjectResult { Succeeded = false, StatusCode = 500, Error = "Could not load project." };
        if (projectEntityResponse.Result == null)
            return new ProjectResult { Succeeded = false, StatusCode = 404, Error = "No project found." };
        var projectEntity = projectEntityResponse.Result;

        var memberEntities = new List<MemberEntity>();
        foreach (var memberId in form.MemberIds)
        {
            var memberResult = await _memberRepository.GetOneEntityAsync(x => x.Id == memberId, include => include.Address!);
            if (memberResult.Succeeded && memberResult.Result != null)
            {
                memberEntities.Add(memberResult.Result);
            }
        }
        
        projectEntity.Id = form.Id;
        projectEntity.ImagePath = form.ImagePath;
        projectEntity.ProjectName = form.ProjectName;
        projectEntity.Description = form.Description;
        projectEntity.StartDate = form.StartDate;
        projectEntity.EndDate = form.EndDate;
        projectEntity.Budget = form.Budget;
        projectEntity.ClientId = form.ClientId;
        projectEntity.StatusId = form.StatusId;
        projectEntity.Members = memberEntities;

        var result = await _projectRepository.UpdateAsync(projectEntity);
        return result.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 200 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }
}
