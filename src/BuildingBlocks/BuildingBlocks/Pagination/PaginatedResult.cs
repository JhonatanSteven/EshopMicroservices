namespace BuildingBlocks.Pagination;
public class PaginatedResult<TEntity>
    (int pages , int pageIndex, int pageSize, long count, IEnumerable<TEntity> data) 
    where TEntity : class
{
    public int Pages { get; } = pages;
    public int PageIndex { get; } = pageIndex;
    public int PageSize { get; } = pageSize;
    public long Count { get; } = count;
    public IEnumerable<TEntity> Data { get; } = data;
}
