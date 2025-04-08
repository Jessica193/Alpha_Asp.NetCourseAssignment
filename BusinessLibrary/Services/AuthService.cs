using BusinessLibrary.Models;
using DataLibrary.Entities;
using DomainLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BusinessLibrary.Services;

public interface IAuthService
{
    Task<AuthResult> SignUpAsync(SignUpFormData form);
    Task<AuthResult> SignInAsync(MemberSignInFormData form);
    Task<AuthResult> SignOutAsync();
}

public class AuthService(UserManager<MemberEntity> userManager, SignInManager<MemberEntity> signInManager, IMemberService memberService) : IAuthService
{
    private readonly UserManager<MemberEntity> _userManager = userManager;
    private readonly SignInManager<MemberEntity> _signInManager = signInManager;
    private readonly IMemberService _memberService = memberService;



    public async Task<AuthResult> SignInAsync(MemberSignInFormData form)
    {
        if (form == null)
            return new AuthResult { Succeeded = false, StatusCode = 400, Error = "Not all required fileds are supplied" };


        var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, form.RememberMe, false);
        return result.Succeeded
            ? new AuthResult { Succeeded = true, StatusCode = 200 }
            : new AuthResult { Succeeded = false, StatusCode = 401, Error = "Invalid email or password" };
    }


    public async Task<AuthResult> SignUpAsync(SignUpFormData form)
    {
        if (form == null)
            return new AuthResult { Succeeded = false, StatusCode = 400, Error = "Not all required fileds are supplied" };

        var result = await _memberService.CreateMemberAsync(form);
        return result.Succeeded
            ? new AuthResult { Succeeded = true, StatusCode = 201 }
            : new AuthResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }



    public async Task<AuthResult> SignOutAsync()
    {
        await _signInManager.SignOutAsync();
        return new AuthResult { Succeeded = true, StatusCode = 200 };
    }


}
