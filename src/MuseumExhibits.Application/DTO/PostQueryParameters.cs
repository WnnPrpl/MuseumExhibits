using System.ComponentModel.DataAnnotations;

namespace MuseumExhibits.Application.DTO
{
    public class PostQueryParameters
    {
        public string? Title { get; set; }
        public string? Category { get; set; }

        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, int.MaxValue)]
        public int PageSize { get; set; } = 10;
    }
}
