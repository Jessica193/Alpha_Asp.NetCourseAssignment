using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WebbApp.ViewModels;

public class AddProjectViewModel
{
    [Display(Name = "Project Image", Prompt = "Select an image")]
    [DataType(DataType.Upload)]
    public IFormFile? ProjectImage { get; set; }


    [Display(Name = "Project Name", Prompt = "Enter project name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string ProjectName { get; set; } = null!;


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


    [Display(Name = "Budget", Prompt = "0")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public decimal Budget { get; set; }


    [Display(Name = "Client Name", Prompt = "Select a client")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public int ClientId { get; set; } 


    [Display(Name = "Members", Prompt = "Select a member")]
    [Required(ErrorMessage = "You must select a member")]
    public string UserId { get; set; } = null!;


    [Display(Name = "Status", Prompt = "Select a status")]
    [Required(ErrorMessage = "You must select a status")]
    public int StatusId { get; set; }



}
