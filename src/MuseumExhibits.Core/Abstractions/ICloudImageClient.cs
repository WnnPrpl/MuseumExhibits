using Microsoft.AspNetCore.Http;

namespace MuseumExhibits.Core.Abstractions
{
    public interface ICloudImageClient
    {
        Task<UploadImageResult> UploadImageAsync(IFormFile file, string folderPath);
        Task DeleteImageAsync(string publicId);
    }

    public class UploadImageResult
    {
        public string Url { get; set; } = default!;
        public string PublicId { get; set; } = default!;
    }
}
