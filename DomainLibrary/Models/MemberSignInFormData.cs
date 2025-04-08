using System.ComponentModel.DataAnnotations;

namespace DomainLibrary.Models;

public class MemberSignInFormData
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool RememberMe { get; set; } = false;
}
