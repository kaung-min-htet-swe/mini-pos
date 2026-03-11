namespace mini_pos.Core.Dtos;

public class PagedResult<T>
{
    public PagedResult(List<T> items, int totalCount, int pageNumber, int limit)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        Limit = limit;
    }

    public List<T> Items { get; }
    public int TotalCount { get; }
    public int PageNumber { get; }
    public int Limit { get; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)Limit);
}