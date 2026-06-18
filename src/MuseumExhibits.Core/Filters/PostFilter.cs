namespace MuseumExhibits.Core.Filters
{
    public class PostFilter
    {
        public string? Title { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
