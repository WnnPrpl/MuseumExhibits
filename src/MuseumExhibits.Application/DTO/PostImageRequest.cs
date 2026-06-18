using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MuseumExhibits.Application.DTO
{
    public class PostImageRequest
    {
        [Required]
        public IFormFile File { get; set; } = default!;
    }
}
