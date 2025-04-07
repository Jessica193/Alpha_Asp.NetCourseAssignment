using DomainLibrary.Models;

namespace BusinessLibrary.Models;

public class StatusResult : ServiceResult
{
    public IEnumerable<StatusModel>? Result { get; set; }
}
