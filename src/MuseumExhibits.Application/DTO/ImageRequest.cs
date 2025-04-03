using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MuseumExhibits.Application.DTO
{
    public class ImageRequest
    {
        [Required]
        public bool IsTitleImage { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}
