using System.ComponentModel.DataAnnotations;

namespace DomainLibrary.Models;

public class SignUpFormData
{

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string StreetName { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Password { get; set; } = null!;

}
