using Microsoft.AspNetCore.Http;
using MuseumExhibits.Application.DTO;

namespace MuseumExhibits.Application.Abstractions
{
    public interface IImageService
    {
        Task<IEnumerable<ImageResponse>> GetByEntityId(Guid entityId);
        Task<ImageResponse> UploadImage(Guid exhibitId, ImageRequest imageRequest);
        Task DeleteImage(Guid imageId);
        Task DeleteByEntityId(Guid entityId);
        Task SetTitleImage(Guid entityId, Guid imageId);
    }
}
