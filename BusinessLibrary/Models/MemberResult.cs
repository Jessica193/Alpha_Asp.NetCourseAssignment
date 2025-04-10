using DomainLibrary.Models;

namespace BusinessLibrary.Models;

public class MemberResult<T> : ServiceResult
{
    public T? Result { get; set; }
}

public class MemberResult : ServiceResult
{
}
