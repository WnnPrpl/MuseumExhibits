using MuseumExhibits.Core.Models;

namespace MuseumExhibits.Core.Abstractions
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(Guid id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task <Guid>CreateAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Category category);
        Task<IEnumerable<Category>> GetByPageAsync(int page, int pageSize);

    }
}
