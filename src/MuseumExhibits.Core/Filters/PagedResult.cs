
namespace MuseumExhibits.Core.Filters
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; }
        public int TotalCount { get; }
        public int TotalPages { get; }
        public int PageNumber { get; }
        public int PageSize { get; }

        public PagedResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
