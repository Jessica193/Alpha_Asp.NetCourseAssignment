namespace DomainLibrary.Models;

public class MemberAddress
{
    public string UserId { get; set; } = null!;
    public string? StreetName { get; set; } 
    public string? City { get; set; }
    public string? PostalCode { get; set; } 

    public virtual ICollection<Member> Members { get; set; } = [];
}
