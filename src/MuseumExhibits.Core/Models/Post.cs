namespace MuseumExhibits.Core.Models
{
    public class Post
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? ShortContent { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
        public string? Category { get; set; }
        public bool Visible { get; set; } = false;
    }
}
