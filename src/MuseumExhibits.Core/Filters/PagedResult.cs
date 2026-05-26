
namespace MuseumExhibits.Core.Filters
{
    public record PagedResult<T>(
        IEnumerable<T> Items,
        int TotalCount,
        int PageNumber,
        int PageSize)
    {
        public int TotalPages { get; } = (int)Math.Ceiling(TotalCount / (double)PageSize);
    }
}
