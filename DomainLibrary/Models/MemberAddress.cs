namespace DomainLibrary.Models;

public class MemberAddress
{
    public string UserId { get; set; } = null!;
    public string StreetName { get; set; } = null!;
    public string City { get; set; } = null!;
    public string PostalCode { get; set; } = null!;

    public virtual ICollection<Member> Members { get; set; } = [];
}
