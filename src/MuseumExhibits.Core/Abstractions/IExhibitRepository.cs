using MuseumExhibits.Core.Filters;
using MuseumExhibits.Core.Models;

namespace MuseumExhibits.Core.Abstractions
{
    public interface IExhibitRepository
    {
        Task<Exhibit> GetByIdAsync(Guid id);
        Task<List<Exhibit>> GetByIdsTrackedAsync(IEnumerable<Guid> ids);
        Task<(IEnumerable<Exhibit> Exhibits, int TotalCount)> GetAsync(ExhibitFilter filter, bool isAdmin);
        Task<Guid> CreateAsync(Exhibit exhibit);
        Task UpdateAsync(Exhibit exhibit);
        Task DeleteAsync(Guid id);
    }
}
