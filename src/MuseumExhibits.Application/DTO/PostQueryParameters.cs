using System.ComponentModel.DataAnnotations;

namespace MuseumExhibits.Application.DTO
{
    public class PostQueryParameters
    {
        public string? Title { get; set; }

        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
