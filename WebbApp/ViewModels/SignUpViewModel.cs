using System.ComponentModel.DataAnnotations;

namespace WebbApp.ViewModels;

public class SignUpViewModel
{
    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Text)]
    [Display(Name = "First Name", Prompt = "Your first name")]
    public string FirstName { get; set; } = null!;


    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Text)]
    [Display(Name = "Last Name", Prompt = "Your last name")]
    public string LastName { get; set; } = null!;


    [Required(ErrorMessage = "Required")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email", Prompt = "Your email adress")]
    [RegularExpression(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Text)]
    [Display(Name = "Street name", Prompt = "Your street name")]
    public string StreetName { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Text)]
    [Display(Name = "Postal Code", Prompt = "Your postal code")]
    public string PostalCode { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Text)]
    [Display(Name = "City", Prompt = "Your city")]
    public string City { get; set; } = null!;


    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter your password")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, and one number.")]
    public string Password { get; set; } = null!;


    [Required (ErrorMessage = "Required")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password", Prompt = "Confirm your password")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = null!;


    [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the terms")]
    public bool TermsAccepted { get; set; }
}
