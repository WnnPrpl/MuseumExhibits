using Microsoft.AspNetCore.Http;

namespace MuseumExhibits.Application.DTO
{
    public class ImageRequest
    {
        public bool IsTitleImage { get; set; }
        public IFormFile File { get; set; }
    }
}
