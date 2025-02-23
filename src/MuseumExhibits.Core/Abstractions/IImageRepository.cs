using MuseumExhibits.Core.Models;

namespace MuseumExhibits.Core.Abstractions
{
    public interface IImageRepository
    {
        Task<Image> GetByIdAsync(Guid id);
        Task<IEnumerable<Image>> GetByEntityIdAsync(Guid entitytId);
        Task <Guid> CreateAsync(Image image);
        Task SetTitleImageAsync(Guid entityId, Guid imageId);
        Task DeleteAsync(Guid id);
        Task<Image> GetTitleImageByEntityIdAsync(Guid entityId);
        Task<ITransaction> BeginTransactionAsync();
    }
}
