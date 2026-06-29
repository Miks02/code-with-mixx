namespace CodeWithMixx.Common.Results;

public class PagedResult<T>(IReadOnlyList<T> items, int pageNumber, int pageSize, int totalCount)
{
    public IReadOnlyList<T> Items { get; init; } = items;
    public int PageNumber { get; init; } = pageNumber;
    public int PageSize { get; init; } = pageSize;
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
    public int PaginatedCount => Items.Count;
    public int TotalCount { get; init; } = totalCount;
}