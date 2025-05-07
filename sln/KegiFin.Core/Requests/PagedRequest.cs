namespace KegiFin.Core.Requests;

public abstract class PagedRequest : BaseRequest
{
    public int PageNumber { get; set; } = Configuration.DefaultPageNumber;
    public int PageSize { get; set; } = Configuration.DefaultPageSize;
}