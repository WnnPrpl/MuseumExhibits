namespace MuseumExhibits.Application.DTO
{
    public class PostResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? ShortContent { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime PublishedAt { get; set; }
        public string? Category { get; set; }
        public bool Visible { get; set; }
    }
}
