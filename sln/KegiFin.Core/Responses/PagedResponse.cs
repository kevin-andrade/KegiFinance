using System.Text.Json.Serialization;

namespace KegiFin.Core.Responses;

public class PagedResponse<TData> : Response<TData>
{
    [JsonConstructor]
    public PagedResponse(
        TData? data,
        int totalCount,
        int currentPage,
        int pageSize = Configuration.DefaultPageSize
    ) : base(data)
    {
        TotalCount = totalCount;
        CurrentPage = currentPage;
        PageSize = pageSize;
    }
    
    public PagedResponse(
        TData? data,
        string? message = null,
        int code = Configuration.DefaultStatusCode
    ) : base(data, message, code)
    {
    }
    
    public int CurrentPage { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public int PageSize { get; set; } = Configuration.DefaultPageSize;
    public int TotalCount { get; set; }
}