using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BusinessLibrary.Models;

public class EditMemberForm
{
    [Required]
    public int Id { get; set; }

    [Display(Name = "Member Image", Prompt = "Select an image")]
    [DataType(DataType.Upload)]
    public IFormFile? MemberImage { get; set; }

    [Display(Name = "First name", Prompt = "Enter first name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Last name", Prompt = "Enter last name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string LastName { get; set; } = null!;


    [Display(Name = "Email Address", Prompt = "Enter email address")]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = null!;


    [Display(Name = "Location", Prompt = "Enter location")]
    [DataType(DataType.Text)]
    public string? Location { get; set; }


    [Display(Name = "Phone Number", Prompt = "Enter phone number")]
    [DataType(DataType.PhoneNumber)]
    public string? Phone { get; set; }
}
