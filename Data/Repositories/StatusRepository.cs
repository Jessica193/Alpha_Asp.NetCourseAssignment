using DataLibrary.Contexts;
using DataLibrary.Entities;
using DataLibrary.Interfaces;
using DomainLibrary.Models;

namespace DataLibrary.Repositories;

public class StatusRepository(DataContext context) : BaseRepository<StatusEntity, StatusModel>(context), IStatusRepository
{
}



