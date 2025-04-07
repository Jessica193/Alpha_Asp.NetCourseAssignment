using BusinessLibrary.Models;
using DataLibrary.Entities;
using DataLibrary.Interfaces;
using DataLibrary.Repositories;
using DomainLibrary.Extentions;
using DomainLibrary.Models;
using Microsoft.AspNetCore.Identity;
namespace BusinessLibrary.Services;

public interface IMemberService
{
    Task<MemberResult> AddMemberToRoleAsync(string memberId, string roleName);
    Task<MemberResult> GetMemberssAsync();
}

public class MemberService(IMemberRepository memberRepository, UserManager<MemberEntity> userManager, RoleManager<IdentityRole> roleManager) : IMemberService
{
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly UserManager<MemberEntity> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    public async Task<MemberResult> GetMemberssAsync()
    {
        var result = await _memberRepository.GetAllAsync();
        return result.MapTo<MemberResult>();
    }

    public async Task<MemberResult> AddMemberToRoleAsync(string memberId, string roleName)
    {
        var role = await _roleManager.RoleExistsAsync(roleName);
        if (!role)
        {
            return new MemberResult
            {
                Succeeded = false,
                StatusCode = 404,
                Error = "Role does not exist"
            };
        }
        var member = await _userManager.FindByIdAsync(memberId);
        if (member == null)
        {
            return new MemberResult
            {
                Succeeded = false,
                StatusCode = 404,
                Error = "Member not found"
            };
        }
        var result = await _userManager.AddToRoleAsync(member, roleName);
        if (result.Succeeded)
        {
            return new MemberResult
            {
                Succeeded = true,
                StatusCode = 200,
            };
        }
        return new MemberResult
        {
            Succeeded = false,
            StatusCode = 500,
            Error = "Unable to add member to role"
        };

    }
}
