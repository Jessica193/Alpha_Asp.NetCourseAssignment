using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebbApp.ViewModels;

public class AddMemberViewModel
{
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


    [Display(Name = "Email", Prompt = "Enter email address")]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = null!;


    [Display(Name = "Phone", Prompt = "Enter phone number")]
    [DataType(DataType.PhoneNumber)]
    public string? Phone { get; set; }


    [Display(Name = "Job Title", Prompt = "Enter job title")]
    [DataType(DataType.Text)]
    public string? JobTitle { get; set; }


    [Display(Name = "Street Address", Prompt = "Enter street address")]
    [DataType(DataType.Text)]
    public string? StreetName { get; set; }


    [Display(Name = "City", Prompt = "Enter city")]
    [DataType(DataType.Text)]
    public string? City { get; set; } 

    [Display(Name = "Postal Code", Prompt = "Enter postal code")]
    [DataType(DataType.Text)]
    public string? PostalCode { get; set; } 


    [Display(Name = "Date of Birth", Prompt = "Enter date of birth")]
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }


    [Display(Name = "Role")]
    [Required(ErrorMessage = "Please select a role")]
    public string SelectedRole { get; set; } = null!;


    public IEnumerable<SelectListItem> AvailableRoles { get; set; } = [];
}







