using DomainLibrary.Models;

namespace BusinessLibrary.Models;

public class MemberResult : ServiceResult
{
    public IEnumerable<Member>? Result { get; set; }
}
