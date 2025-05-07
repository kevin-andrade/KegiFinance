using System.Text.Json.Serialization;

namespace KegiFin.Core.Responses;

public class Response<TData>
{
    private readonly int _code;

    [JsonConstructor]
    public Response() => _code = Configuration.DefaultStatusCode;

    public Response(
        TData? data,
        string? message = null, 
        int code = Configuration.DefaultStatusCode)
    {
        Data = data;
        _code = code;
        Message = message;
    }
    
    public TData? Data { get; set; }
    public string? Message { get; set; }
    [JsonIgnore]
    public bool IsSuccess => _code is >= 200 and <= 299;
}