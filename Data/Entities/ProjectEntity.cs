using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLibrary.Entities;

public class ProjectEntity
{
    [Key]
    public int Id { get; set; }
    public string? ImagePath { get; set; }
    public string ProjectName { get; set; } = null!;
    public string? Description { get; set; } 

    [Column(TypeName = "date")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime EndDate { get; set; }
    public decimal? Budget { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public virtual ICollection<MemberEntity> Members { get; set; } = [];

    [ForeignKey("ClientId")]
    public int ClientId { get; set; }
    public virtual ClientEntity Client { get; set; } = null!;

    [ForeignKey("StatusId")]
    public int StatusId { get; set; }
    public virtual StatusEntity Status { get; set; } = null!;
}
