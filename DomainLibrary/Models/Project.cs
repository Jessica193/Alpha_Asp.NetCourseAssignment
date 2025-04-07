using System.ComponentModel.DataAnnotations.Schema;

namespace DomainLibrary.Models;

public class Project
{
    public int Id { get; set; }
    public string? ImagePath { get; set; }
    public string ProjectName { get; set; } = null!;
    public string? Description { get; set; } 

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
    public decimal? Budget { get; set; }

    public string UserId { get; set; } = null!;                     //Hans tog bort denna
    public virtual ICollection<Member> Members { get; set; } = [];


    public int ClientId { get; set; }                               //Hans tog bort denna
    public virtual Client Client { get; set; } = null!;

    public int StatusId { get; set; }                                //Hans tog bort denna
    public virtual StatusModel Status { get; set; } = null!;
}
