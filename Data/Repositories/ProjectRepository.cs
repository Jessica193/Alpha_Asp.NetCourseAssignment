using DataLibrary.Contexts;
using DataLibrary.Entities;
using DataLibrary.Interfaces;
using DomainLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLibrary.Repositories;

public class ProjectRepository(DataContext context) : BaseRepository<ProjectEntity, Project>(context), IProjectRepository
{
}



