using BusinessLibrary.Models;
using DomainLibrary.Models;
namespace BusinessLibrary.Interfaces;

public interface IMemberService
{
    Task<MemberResult> AddMemberToRoleAsync(string memberId, string roleName);
    Task<MemberResult> CreateMemberFromAdminAsync(AddMemberFormData form);
    Task<MemberResult> CreateMemberFromSignUpAsync(SignUpFormData form, string roleName = "User");
    Task<MemberResult<Member>> GetMemberByIdAsync(string id);
    Task<MemberResult<IEnumerable<Member>>> GetMembersAsync();
    Task<bool> MemberExists(string email);
    Task<MemberResult> EditMemberAsync(Member member);
}
