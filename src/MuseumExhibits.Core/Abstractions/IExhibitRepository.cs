using MuseumExhibits.Core.Filters;
using MuseumExhibits.Core.Models;

namespace MuseumExhibits.Core.Abstractions
{
    public interface IExhibitRepository
    {
        Task<Exhibit> GetByIdAsync(Guid id);
        IQueryable<Exhibit> Get();
        Task<Guid> CreateAsync(Exhibit exhibit);
        Task UpdateAsync(Exhibit exhibit);
        Task DeleteAsync(Guid id);

    }
}
