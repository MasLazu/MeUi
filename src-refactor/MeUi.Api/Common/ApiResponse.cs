namespace MeUi.Api.Common;

/// <summary>
/// Standard API response wrapper
/// </summary>
public class ApiResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public object? Errors { get; set; }
}

/// <summary>
/// Standard API response wrapper with data
/// </summary>
/// <typeparam name="T">The type of data being returned</typeparam>
public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }
}

/// <summary>
/// Paginated API response
/// </summary>
/// <typeparam name="T">The type of data being returned</typeparam>
public class PaginatedApiResponse<T> : ApiResponse<IEnumerable<T>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}