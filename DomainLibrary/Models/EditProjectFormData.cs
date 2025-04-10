using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DomainLibrary.Models;

public class EditProjectFormData
{
    public int Id { get; set; }
    public IFormFile? ProjectImage { get; set; }
    public string? ImagePath { get; set; }
    public string ProjectName { get; set; } = null!;
    public string ClientName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateOnly StartDate { get; set; } 
    public DateOnly EndDate { get; set; } 
    public int UserId { get; set; }
    public string Budget { get; set; } = null!;
}



