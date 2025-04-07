using DataLibrary.Contexts;
using DataLibrary.Entities;
using DataLibrary.Interfaces;
using DomainLibrary.Models;

namespace DataLibrary.Repositories;

public class MemberAddressRepository(DataContext context) : BaseRepository<MemberAddressEntity, MemberAddress>(context), IMemberAddressRepository
{
}



