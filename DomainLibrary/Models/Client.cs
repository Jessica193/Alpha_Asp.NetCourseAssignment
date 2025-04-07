using System.ComponentModel.DataAnnotations;

namespace DomainLibrary.Models;

public class Client
{
    public int Id { get; set; }
    public string? ImagePath { get; set; }
    public string ClientName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Location { get; set; }
    public string? Phone { get; set; }
    public virtual ICollection<Project>? Projects { get; set; } = [];
}
