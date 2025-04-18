using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DomainLibrary.Models;

public class EditProjectFormData
{
    public int Id { get; set; }
    public IFormFile? ProjectImage { get; set; }
    public string? ImagePath { get; set; }
    public string ProjectName { get; set; } = null!;
    public int ClientId { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; } 
    public DateTime EndDate { get; set; }
    public List<string> MemberIds { get; set; } = [];
    public decimal? Budget { get; set; } = null!;
    public int StatusId { get; set; }

}






