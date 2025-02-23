using System.ComponentModel.DataAnnotations;

namespace MuseumExhibits.Application.DTO
{
    public class CategoryDTO
    {
        public Guid? Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
    }
}
