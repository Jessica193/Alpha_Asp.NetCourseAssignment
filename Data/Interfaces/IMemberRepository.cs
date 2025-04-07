using DataLibrary.Entities;
using DomainLibrary.Models;

namespace DataLibrary.Interfaces;

public interface IMemberRepository : IBaseRepository<MemberEntity, Member>
{

}