using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DataLibrary.Entities;

[Index(nameof(Status), IsUnique = true)]
public class StatusEntity
{
    [Key]
    public int Id { get; set; }
    public string Status { get; set; } = null!;
    public ICollection<ProjectEntity> Projects { get; set; } = [];
}
