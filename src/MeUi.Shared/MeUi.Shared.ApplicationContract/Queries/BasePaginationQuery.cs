using FastEndpoints;

namespace MeUi.Shared.ApplicationContract.Queries;

public abstract class BasePaginationQuery<T> : ICommand<BasePaginationQueryResult<T>>
{
    public int Page = 1;
    public int PageSize = 10;
    public string? search;
    public ICollection<FilterPaginationQuery> Filters { get; set; } = new List<FilterPaginationQuery>();
    public ICollection<SortPaginationQuery> Sorts { get; set; } = new List<SortPaginationQuery>();
}

public class FilterPaginationQuery
{
    public string Field { get; set; } = default!;
    public string Op { get; set; } = "eq"; // eq, gte, contains, etc.
    public object? Value { get; set; }
}

public class SortPaginationQuery
{
    public string Field { get; set; } = default!;
    public string Direction { get; set; } = "asc"; // "asc" or "desc"
}

public class BasePaginationQueryResult<T>
{
    public List<T> Items { get; set; } = new();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
}
