using System.ComponentModel.DataAnnotations;

namespace WebbApp.ViewModels;

public class EditProjectFormViewModel
{
    [Required]
    public int Id { get; set; }


    [Display(Name = "Project Image", Prompt = "Select an image")]
    [DataType(DataType.Upload)]
    public IFormFile? ProjectImage { get; set; }


    [Display(Name = "Project name", Prompt = "Enter project name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string ProjectName { get; set; } = null!;


    [Display(Name = "Client name", Prompt = "Enter client name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string ClientName { get; set; } = null!;


    [Display(Name = "Description", Prompt = "Type something")]
    [DataType(DataType.Text)]
    public string? Description { get; set; } 


    [Display(Name = "Start date")]
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Required")]
    public DateTime StartDate { get; set; }



    [Display(Name = "End date")]
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Required")]
    public DateTime EndDate { get; set; }


    public int UserId { get; set; }

    [Display(Name = "Budget", Prompt = "0")]
    [DataType(DataType.Text)]
    public decimal? Budget { get; set; }
}

//lägg till statusid
