using DataLibrary.Entities;
using DataLibrary.Models;
using DomainLibrary.Models;
using System.Linq.Expressions;

namespace DataLibrary.Interfaces;

public interface IProjectRepository : IBaseRepository<ProjectEntity, Project>
{
}
