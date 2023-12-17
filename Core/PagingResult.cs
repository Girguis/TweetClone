namespace Core;

public class PagingResult<TData>
{
    public IEnumerable<TData> Data { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int NumberOfPages
    {
        get
        {
            if (TotalCount == 0) return 0;
            return (int)Math.Ceiling(1.0 * PageSize / TotalCount);
        }
    }
}
