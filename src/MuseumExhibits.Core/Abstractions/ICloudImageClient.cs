using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
