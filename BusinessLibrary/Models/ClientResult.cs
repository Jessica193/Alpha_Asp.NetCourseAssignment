using DomainLibrary.Models;

namespace BusinessLibrary.Models;

public class ClientResult<T> : ServiceResult
{
    public T? Result { get; set; }
}

public class ClientResult : ServiceResult
{
}
