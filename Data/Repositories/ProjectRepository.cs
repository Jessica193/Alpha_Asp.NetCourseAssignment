using DataLibrary.Contexts;
using DataLibrary.Entities;
using DataLibrary.Interfaces;
using DomainLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DataLibrary.Models;

namespace DataLibrary.Repositories;

public class ProjectRepository(DataContext context) : BaseRepository<ProjectEntity, Project>(context), IProjectRepository
{
    public override async Task<RepositoryResult<IEnumerable<ProjectEntity>>> GetAllEntitiesAsync(
        bool orderByDescendning = false,
        Expression<Func<ProjectEntity, object>>? sortBy = null,
        Expression<Func<ProjectEntity, bool>>? where = null,
        params Expression<Func<ProjectEntity, object>>[] includes)
    {
        IQueryable<ProjectEntity> query = _context.Projects;

        if (where != null)
            query = query.Where(where);

        query = query
            .Include(p => p.Client)
            .Include(p => p.Status)
            .Include(p => p.Members)
                .ThenInclude(m => m.Address);

        if (sortBy != null)
            query = orderByDescendning ? query.OrderByDescending(sortBy) : query.OrderBy(sortBy);

        var entities = await query.ToListAsync();

        return new RepositoryResult<IEnumerable<ProjectEntity>> { Succeeded = true, StatusCode = 200, Result = entities };
    }

    public override async Task<RepositoryResult<ProjectEntity>> GetOneEntityAsync(
        Expression<Func<ProjectEntity, bool>>? where,
        params Expression<Func<ProjectEntity, object>>[] includes)
    {
        IQueryable<ProjectEntity> query = _context.Projects;

        query = query
            .Include(p => p.Client)
            .Include(p => p.Status)
            .Include(p => p.Members)
                .ThenInclude(m => m.Address);

        var entity = where != null
           ? await query.FirstOrDefaultAsync(where)
           : await query.FirstOrDefaultAsync();

        if (entity == null)
            return new RepositoryResult<ProjectEntity>{Succeeded = false, StatusCode = 404, Error = "Project not found."};


        return new RepositoryResult<ProjectEntity> { Succeeded = true, StatusCode = 200, Result = entity };
    }
}




