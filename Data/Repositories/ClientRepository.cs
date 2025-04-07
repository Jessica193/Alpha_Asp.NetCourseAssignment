using DataLibrary.Contexts;
using DataLibrary.Entities;
using DataLibrary.Interfaces;
using DomainLibrary.Models;

namespace DataLibrary.Repositories;

public class ClientRepository(DataContext context) : BaseRepository<ClientEntity, Client>(context), IClientRepository
{
}



