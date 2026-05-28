using MuseumExhibits.Application.DTO;
using MuseumExhibits.Core.Filters;

namespace MuseumExhibits.Application.Abstractions
{
    public interface ICollectionService
    {
        Task<CollectionResponse> GetById(Guid id);
        Task<IEnumerable<CollectionSummaryDTO>> GetAll();
        Task<PagedResult<CollectionSummaryDTO>> GetPaged(int page, int pageSize);
        Task<Guid> Create(CollectionRequest request);
        Task Update(Guid id, CollectionRequest request);
        Task Delete(Guid id);
    }
}
