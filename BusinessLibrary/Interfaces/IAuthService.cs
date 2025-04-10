using BusinessLibrary.Models;
using DomainLibrary.Models;

namespace BusinessLibrary.Interfaces;

public interface IAuthService
{
    Task<AuthResult> SignUpAsync(SignUpFormData form);
    Task<AuthResult> SignInAsync(MemberSignInFormData form);
    Task<AuthResult> SignOutAsync();
}
