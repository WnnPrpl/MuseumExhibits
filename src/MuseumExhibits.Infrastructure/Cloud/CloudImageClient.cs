using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using MuseumExhibits.Core.Abstractions;


namespace MuseumExhibits.Infrastructure.Cloud
{
    public class CloudImageClient : ICloudImageClient
    {
        private readonly Cloudinary _cloudinary;

        public CloudImageClient(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }
           
        public async Task<UploadImageResult> UploadImageAsync(IFormFile file, string folderPath)
        {
            using var fileStream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, fileStream),
                Folder = folderPath
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Failed to upload image to Cloudinary");
            }

            return new UploadImageResult
            {
                Url = uploadResult.SecureUrl.AbsoluteUri,
                PublicId = uploadResult.PublicId
            };
        }

        public async Task DeleteImageAsync(string publicId)
        {
            var deleteResult = await _cloudinary.DestroyAsync(new DeletionParams(publicId));

            if (deleteResult.Result != "ok")
            {
                throw new Exception("Failed to delete image from Cloudinary");
            }
        }
    }
}
