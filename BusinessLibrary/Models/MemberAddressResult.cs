namespace BusinessLibrary.Models;

public class MemberAddressResult<T> : ServiceResult
{
    public T? Result { get; set; }
}

public class MemberAddressResult : ServiceResult
{
}

