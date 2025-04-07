using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLibrary.Entities;

[Index(nameof(Email), IsUnique = true)]
public class MemberEntity : IdentityUser
{
    [ProtectedPersonalData]
    public string FirstName { get; set; } = null!;

    [ProtectedPersonalData]
    public string LastName { get; set; } = null!;

    [ProtectedPersonalData]
    public string? JobTitle { get; set; }

    [ProtectedPersonalData]
    [Column(TypeName = "date")]
    public DateTime? DateOfBirth { get; set; }

    [ProtectedPersonalData]
    public string? ImagePath { get; set; }

    public virtual MemberAddressEntity? Address { get; set; }
    public virtual ICollection<ProjectEntity> Projects { get; set; } = [];
}


