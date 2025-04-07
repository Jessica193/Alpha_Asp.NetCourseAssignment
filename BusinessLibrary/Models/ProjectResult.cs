using DomainLibrary.Models;

namespace BusinessLibrary.Models;

public class ProjectResult<T> : ServiceResult
{
    public T? Result { get; set; }
}

public class ProjectResult : ServiceResult
{
}
