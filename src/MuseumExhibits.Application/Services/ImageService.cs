using MuseumExhibits.Application.Abstractions;
using MuseumExhibits.Application.DTO;
using MuseumExhibits.Core.Abstractions;
using MuseumExhibits.Core.Models;


namespace MuseumExhibits.Application.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly ICloudImageClient _cloudImageClient;

        public ImageService(IImageRepository imageRepository, ICloudImageClient cloudImageClient)
        {
            _imageRepository = imageRepository;
            _cloudImageClient = cloudImageClient;
        }

        public async Task<IEnumerable<ImageResponse>> GetByEntityId(Guid entityId)
        {
            if (entityId == Guid.Empty)
            {
                throw new ArgumentException("Entity ID cannot be empty.", nameof(entityId));
            }

            var images = await _imageRepository.GetByEntityIdAsync(entityId);

            var imageResponses = images?.Select(image => new ImageResponse
            {
                Id = image.Id,
                IsTitleImage = image.IsTitleImage,
                Url = image.Url
            }) ?? Enumerable.Empty<ImageResponse>();

            return imageResponses;
        }

        public async Task<ImageResponse> UploadImage(Guid exhibitId, ImageRequest imageRequest)
        {
            await using var transaction = await _imageRepository.BeginTransactionAsync();
            string? publicId = null;

            try
            {
                var uploadResult = await _cloudImageClient.UploadImageAsync(imageRequest.File, $"exhibit_images/{exhibitId}");
                publicId = uploadResult.PublicId;

                var image = new Image
                {
                    Url = uploadResult.Url,
                    PublicId = uploadResult.PublicId,
                    IsTitleImage = imageRequest.IsTitleImage,
                    ExhibitId = exhibitId
                };

                await _imageRepository.CreateAsync(image);

                if (imageRequest.IsTitleImage)
                {
                    await _imageRepository.SetTitleImageAsync(exhibitId, image.Id);
                }

                await transaction.CommitAsync();

                return new ImageResponse
                {
                    Id = image.Id,
                    IsTitleImage = image.IsTitleImage,
                    Url = image.Url
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                if (publicId != null)
                {
                    await RetryOperationAsync(() => _cloudImageClient.DeleteImageAsync(publicId), 3);
                }
                throw;
            }
        }

        public async Task DeleteImage(Guid imageId)
        {
            await using var transaction = await _imageRepository.BeginTransactionAsync();

            try
            {
                var image = await _imageRepository.GetByIdAsync(imageId);
                if (image == null)
                {
                    throw new Exception("Image not found");
                }

                await RetryOperationAsync(() => _cloudImageClient.DeleteImageAsync(image.PublicId), 3);
                await _imageRepository.DeleteAsync(image.Id);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteByEntityId(Guid entityId)
        {
            await using var transaction = await _imageRepository.BeginTransactionAsync();

            try
            {
                var images = await _imageRepository.GetByEntityIdAsync(entityId);

                var deleteTasks = images.Select(image =>
                    RetryOperationAsync(() => _cloudImageClient.DeleteImageAsync(image.PublicId), 2)
                        .ContinueWith(_ => _imageRepository.DeleteAsync(image.Id))
                ).ToList();

                await Task.WhenAll(deleteTasks);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task RetryOperationAsync(Func<Task> operation, int retryCount)
        {
            int attempts = 0;
            while (attempts < retryCount)
            {
                try
                {
                    await operation();
                    return;
                }
                catch (Exception ex)
                {
                    attempts++;
                    if (attempts >= retryCount)
                    {
                        throw;
                    }
                    await Task.Delay(1000);
                }
            }
        }

        public async Task SetTitleImage(Guid entityId, Guid imageId)
        {
            await _imageRepository.SetTitleImageAsync(entityId, imageId);
        }
    }
}
