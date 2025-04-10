using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DomainLibrary.Models;

public class AddProjectFormData
{
  
    public IFormFile? ProjectImage { get; set; }
    public string? ImagePath { get; set; }
    public string ProjectName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal Budget { get; set; }
    public int ClientId { get; set; } 
    public string UserId { get; set; } = null!;
    public int StatusId { get; set; }



}
