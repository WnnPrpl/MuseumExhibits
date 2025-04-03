using AutoMapper;
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
        private readonly IMapper _mapper;

        public ImageService(IImageRepository imageRepository, ICloudImageClient cloudImageClient, IMapper mapper)
        {
            _imageRepository = imageRepository;
            _cloudImageClient = cloudImageClient;
            _mapper = mapper;

        }

        public async Task<IEnumerable<ImageResponse>> GetByEntityId(Guid entityId)
        {
            if (entityId == Guid.Empty)
            {
                throw new ArgumentException("Entity ID cannot be empty.", nameof(entityId));
            }

            var images = await _imageRepository.GetByEntityIdAsync(entityId);

            return images.Select(img => _mapper.Map<ImageResponse>(img));
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

                return _mapper.Map<ImageResponse>(image);
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

                var deleteTasks = images.Select(async image =>
                {
                    await RetryOperationAsync(() => _cloudImageClient.DeleteImageAsync(image.PublicId), 2);
                    await _imageRepository.DeleteAsync(image.Id);
                }).ToList();

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
