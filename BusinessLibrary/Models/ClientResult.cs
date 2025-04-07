using DomainLibrary.Models;

namespace BusinessLibrary.Models;

public class ClientResult : ServiceResult
{
    public IEnumerable<Client>? Result { get; set; }
}
