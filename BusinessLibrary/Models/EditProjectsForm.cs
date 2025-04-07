using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BusinessLibrary.Models;

public class EditProjectsForm
{
    public int Id { get; set; }


    [Display(Name = "Project Image", Prompt = "Select an image")]
    [DataType(DataType.Upload)]
    public IFormFile? ProjectImage { get; set; }


    [Display(Name = "Project Name", Prompt = "Enter project name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string ProjectName { get; set; } = null!;


    [Display(Name = "Client Name", Prompt = "Enter client name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string ClientName { get; set; } = null!;


    [Display(Name = "Description", Prompt = "Type something")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string Description { get; set; } = null!;


    [Display(Name = "Start Date")]
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Required")]
    public DateOnly StartDate { get; set; } 


    [Display(Name = "End Date")]
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Required")]
    public DateOnly EndDate { get; set; } 


    [Display(Name = "Members", Prompt = "Select a member")]
    [Required(ErrorMessage = "You must select a member")]
    public int UserId { get; set; }


    [Display(Name = "Budget", Prompt = "0")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string Budget { get; set; } = null!;
}



