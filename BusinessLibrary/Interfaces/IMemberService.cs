using BusinessLibrary.Models;
using DomainLibrary.Models;
namespace BusinessLibrary.Interfaces;

public interface IMemberService
{
    Task<MemberResult> AddMemberToRoleAsync(string memberId, string roleName);
    Task<MemberResult> CreateMemberFromSignUpAsync(SignUpFormData form, string roleName = "User");
    Task<MemberResult> GetMemberssAsync();
    Task<bool> MemberExists(string email);
}
