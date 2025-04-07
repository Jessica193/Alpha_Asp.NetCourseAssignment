namespace DomainLibrary.Models;

public class StatusModel
{
    public int Id { get; set; }
    public string Status { get; set; } = null!;
    public ICollection<Project> Projects { get; set; } = [];
}
