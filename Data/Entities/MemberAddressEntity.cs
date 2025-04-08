using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLibrary.Entities;

public class MemberAddressEntity
{
    [Key, ForeignKey("Member")]
    [PersonalData]
    public string UserId { get; set; } = null!;

    [ProtectedPersonalData]
    public string StreetName { get; set; } = null!;

    [ProtectedPersonalData]
    public string City { get; set; } = null!;

    [ProtectedPersonalData]
    public string PostalCode { get; set; } = null!;

    public virtual ICollection<MemberEntity> Members { get; set; } = [];

}


