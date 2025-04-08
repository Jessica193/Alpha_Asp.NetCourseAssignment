using DomainLibrary.Models;

namespace BusinessLibrary.Models;

public class StatusResult<T> : ServiceResult
{
    public T? Result { get; set; }
}

public class StatusResult : ServiceResult
{
}
