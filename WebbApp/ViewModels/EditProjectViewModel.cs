using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebbApp.ViewModels;

public class EditProjectViewModel
{
    [Required]
    public int Id { get; set; }


    [Display(Name = "Project Image", Prompt = "Select an image")]
    [DataType(DataType.Upload)]
    public IFormFile? ProjectImage { get; set; }


    [Display(Name = "Project Name", Prompt = "Enter project name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string ProjectName { get; set; } = null!;


    [Display(Name = "Client Name", Prompt = "Select a client")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public int ClientId { get; set; }

    public IEnumerable<SelectListItem> AvailableClients { get; set; } = [];


    [Display(Name = "Description", Prompt = "Type something")]
    [DataType(DataType.Text)]
    public string? Description { get; set; } = null!;


    [Display(Name = "Start Date")]
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Required")]
    public DateTime StartDate { get; set; }


    [Display(Name = "End Date")]
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Required")]
    public DateTime EndDate { get; set; }


    [Display(Name = "Members", Prompt = "Select a member")]
    [Required(ErrorMessage = "You must select a member")]
    public List<string> MemberIds { get; set; } = [];

    public IEnumerable<SelectListItem> AvailableMembers { get; set; } = [];



    [Display(Name = "Buget", Prompt = "0")]
    [DataType(DataType.Text)]
    public decimal? Budget { get; set; }


    [Display(Name = "Status", Prompt = "Select a status")]
    [Required(ErrorMessage = "You must select a status")]
    public int StatusId { get; set; }

    public IEnumerable<SelectListItem> AvailableStatuses { get; set; } = [];
}

