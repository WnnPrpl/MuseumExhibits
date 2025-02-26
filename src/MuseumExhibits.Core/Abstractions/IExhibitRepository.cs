using MuseumExhibits.Core.Filters;
using MuseumExhibits.Core.Models;

namespace MuseumExhibits.Core.Abstractions
{
    public interface IExhibitRepository
    {
        Task<Exhibit> GetByIdAsync(Guid id);
        Task<IEnumerable<Exhibit>> GetAllAsync(bool isAdmin);
        Task<Guid> CreateAsync(Exhibit exhibit);
        Task UpdateAsync(Exhibit exhibit);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Exhibit>> GetByCategoryIdAsync(Guid categoryId, bool isAdmin);


        Task<(IEnumerable<Exhibit> Exhibits, int TotalCount)> GetExhibitsAsync(ExhibitFilter filter);
    }
}
