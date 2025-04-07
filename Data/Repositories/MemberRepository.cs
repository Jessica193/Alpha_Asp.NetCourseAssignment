using DataLibrary.Contexts;
using DataLibrary.Entities;
using DataLibrary.Interfaces;
using DomainLibrary.Models;

namespace DataLibrary.Repositories;

public class MemberRepository(DataContext context) : BaseRepository<MemberEntity, Member>(context), IMemberRepository
{
}



