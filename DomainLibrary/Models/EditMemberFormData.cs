using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DomainLibrary.Models;

public class EditMemberFormData
{
    public string Id { get; set; } = null!;
    public IFormFile? MemberImage { get; set; }
    public string? ImagePath { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string? JobTitle { get; set; }
    public string? StreetName { get; set; } = null!;
    public string? City { get; set; } = null!;
    public string? PostalCode { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
}
