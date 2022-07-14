namespace RC.CA.SharedKernel.Result;

public class PagedResult<T> : CAResult<T>
{
    public PagedResult(PagedInfo pagedInfo, T value) : base(value)
    {
        PagedInfo = pagedInfo;
    }

    public PagedInfo PagedInfo { get; }
}
