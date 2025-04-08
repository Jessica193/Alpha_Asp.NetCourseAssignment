using System.ComponentModel.DataAnnotations;

namespace DomainLibrary.Models;

public class MemberSignInViewModel
{
    [Required(ErrorMessage = "Required")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email", Prompt = "Your email adress")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter your password")]
    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; }
}
