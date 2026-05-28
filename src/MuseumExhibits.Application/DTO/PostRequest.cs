using System.ComponentModel.DataAnnotations;

namespace MuseumExhibits.Application.DTO
{
    public class PostRequest
    {
        [Required]
        [StringLength(300, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [StringLength(500)]
        public string? ShortContent { get; set; }

        public string? ImageUrl { get; set; }

        [StringLength(50)]
        public string? Category { get; set; }

        public bool Visible { get; set; } = false;
    }
}
