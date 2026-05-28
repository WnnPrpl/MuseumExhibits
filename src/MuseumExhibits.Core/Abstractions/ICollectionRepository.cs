using MuseumExhibits.Core.Models;

namespace MuseumExhibits.Core.Abstractions
{
    public interface ICollectionRepository
    {
        Task<Collection> GetByIdAsync(Guid id);
        Task<IEnumerable<Collection>> GetAllAsync();
        Task<(IEnumerable<Collection> Collections, int TotalCount)> GetPagedAsync(int page, int pageSize);
        Task<Guid> CreateAsync(Collection collection);
        Task UpdateAsync(Collection collection);
        Task DeleteAsync(Guid id);
    }
}
