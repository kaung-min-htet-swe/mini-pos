namespace mini_pos.Core.Dtos;

public class PagedResult<T>
{
    public PagedResult(List<T> data, int totalCount, int pageNumber, int limit)
    {
        Data = data;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        Limit = limit;
    }

    public List<T> Data { get; }
    public int TotalCount { get; }
    public int PageNumber { get; }
    public int Limit { get; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)Limit);
}