using AutoMapper;
using MuseumExhibits.Application.Abstractions;
using MuseumExhibits.Application.DTO;
using MuseumExhibits.Core.Abstractions;
using MuseumExhibits.Core.Filters;
using MuseumExhibits.Core.Models;

namespace MuseumExhibits.Application.Services
{
    public class CollectionService(
        ICollectionRepository collectionRepository,
        IExhibitRepository exhibitRepository,
        IMapper mapper) : ICollectionService
    {
        private readonly ICollectionRepository _collectionRepository = collectionRepository;
        private readonly IExhibitRepository _exhibitRepository = exhibitRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<CollectionResponse> GetById(Guid id)
        {
            var collection = await _collectionRepository.GetByIdAsync(id);
            return _mapper.Map<CollectionResponse>(collection);
        }

        public async Task<IEnumerable<CollectionSummaryDTO>> GetAll()
        {
            var collections = await _collectionRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CollectionSummaryDTO>>(collections);
        }

        public async Task<PagedResult<CollectionSummaryDTO>> GetPaged(int page, int pageSize)
        {
            var (collections, totalCount) = await _collectionRepository.GetPagedAsync(page, pageSize);
            var dtos = _mapper.Map<IEnumerable<CollectionSummaryDTO>>(collections);
            return new PagedResult<CollectionSummaryDTO>(dtos, totalCount, page, pageSize);
        }

        public async Task<Guid> Create(CollectionRequest request)
        {
            var collection = new Collection
            {
                Name = request.Name,
                Description = request.Description,
                Exhibits = await ResolveExhibits(request.ExhibitIds)
            };

            await _collectionRepository.CreateAsync(collection);
            return collection.Id;
        }

        public async Task Update(Guid id, CollectionRequest request)
        {
            var collection = await _collectionRepository.GetByIdAsync(id);
            collection.Name = request.Name;
            collection.Description = request.Description;
            collection.Exhibits = await ResolveExhibits(request.ExhibitIds);
            await _collectionRepository.UpdateAsync(collection);
        }

        public async Task Delete(Guid id)
        {
            await _collectionRepository.DeleteAsync(id);
        }

        private async Task<List<Exhibit>> ResolveExhibits(List<Guid> exhibitIds)
        {
            if (exhibitIds.Count == 0)
                return [];

            var exhibits = new List<Exhibit>();
            foreach (var exhibitId in exhibitIds)
                exhibits.Add(await _exhibitRepository.GetByIdAsync(exhibitId));

            return exhibits;
        }
    }
}
