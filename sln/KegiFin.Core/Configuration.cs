namespace KegiFin.Core;

public static class Configuration
{
    public const int DefaultPageNumber = 1;
    public const int DefaultPageSize = 10;
    public const int DefaultStatusCode = 200;
    
    public static string ConnectionString = string.Empty;
    public static string FrontendUrl { get; set; } = string.Empty;
    public static string BackendUrl { get; set; } = string.Empty;
}