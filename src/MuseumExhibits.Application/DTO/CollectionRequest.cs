using System.ComponentModel.DataAnnotations;

namespace MuseumExhibits.Application.DTO
{
    public class CollectionRequest
    {
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public List<Guid> ExhibitIds { get; set; } = [];
    }
}
