using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainLibrary.Models;

public class Project
{
    public int Id { get; set; }
    public string? ImagePath { get; set; }
    public string ProjectName { get; set; } = null!;
    public int ClientId { get; set; }
    public string? Description { get; set; } 

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
    public virtual ICollection<Member> Members { get; set; } = [];
    public decimal? Budget { get; set; }

    public int StatusId { get; set; }
    public virtual Client Client { get; set; } = null!;
    public virtual StatusModel Status { get; set; } = null!;
}
    