using System.ComponentModel.DataAnnotations;

namespace DomainLibrary.Models;

public class CreateAccountForm
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
    
    public string Password { get; set; } = null!;


    [Required (ErrorMessage = "Required")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password", Prompt = "Confirm your password")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = null!;


    [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the terms")]
    public bool TermsAccepted { get; set; }
}
