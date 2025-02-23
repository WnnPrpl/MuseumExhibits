
using System.ComponentModel.DataAnnotations;

namespace MuseumExhibits.Core.Models
{
    public class Image
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Url { get; set; }
        public string PublicId { get; set; }
        public bool IsTitleImage { get; set; } = false;
        public Guid ExhibitId { get; set; }
        public Exhibit Exhibit { get; set; }
    }
}
