using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainLibrary.Models;

public class Member
{
    public string Id { get; set; } = null!;
    public string? ImagePath { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? JobTitle { get; set; }
    public DateTime? DateOfBirth { get; set; }

    [NotMapped]
    public string SelectedRole { get; set; } = null!;
    public virtual MemberAddress? Address { get; set; }
    public virtual ICollection<Project> Projects { get; set; } = [];
}

  
