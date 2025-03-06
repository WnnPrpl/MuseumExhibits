using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using MuseumExhibits.Application.Abstractions;
using MuseumExhibits.Application.DTO;
using MuseumExhibits.Core.Abstractions;
using MuseumExhibits.Core.Filters;
using MuseumExhibits.Core.Models;


namespace MuseumExhibits.Application.Services
{
    public class ExhibitService : IExhibitService
    {
        private readonly IExhibitRepository _exhibitRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public ExhibitService(IExhibitRepository exhibitRepository, IImageService imageService, IMapper mapper)
        {
            _exhibitRepository = exhibitRepository;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<ExhibitResponse> GetById(Guid id)
        {
            var exhibit = await _exhibitRepository.GetByIdAsync(id);
            if (exhibit == null)
            {
                throw new ArgumentException("Exhibit not found.");
            }

            var exhibitDTO = _mapper.Map<ExhibitResponse>(exhibit);

            return exhibitDTO;
        }

        public async Task<PagedResult<ExhibitSummaryDTO>> Get(ExhibitQueryParameters queryParams, bool isAdmin = false)
        {
            try
            {
                var filter = _mapper.Map<ExhibitFilter>(queryParams);

                IQueryable<Exhibit> query = _exhibitRepository.Get();

                query = ApplyFiltering(query, filter);
                query = ApplySorting(query, filter);

                if (!isAdmin)
                {
                    query = query.Where(e => e.Visible);
                }

                int totalCount = await query.CountAsync();

                query = query.Skip((filter.PageNumber - 1) * filter.PageSize)
                             .Take(filter.PageSize);

                var exhibits = await query.ToListAsync();

                var exhibitDtos = _mapper.Map<IEnumerable<ExhibitSummaryDTO>>(exhibits);

                return new PagedResult<ExhibitSummaryDTO>(exhibitDtos, totalCount, filter.PageNumber, filter.PageSize);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while retrieving exhibits.", ex);
            }
        }

        private IQueryable<Exhibit> ApplyFiltering(IQueryable<Exhibit> query, ExhibitFilter filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(e => EF.Functions.Like(e.Name.ToLower(), $"%{filter.Name.ToLower()}%"));

            if (filter.CreationExactDate.HasValue)
            {
                query = query.Where(e => e.CreationExactDate == filter.CreationExactDate);
            }

            if (filter.CreationYear.HasValue)
            {
                query = query.Where(e => e.CreationYear == filter.CreationYear);
            }

            if (filter.CreationCentury.HasValue)
            {
                int centuryMin = (filter.CreationCentury.Value - 1) * 100 + 1;
                int centuryMax = filter.CreationCentury.Value * 100;
                query = query.Where(e =>
                    (e.CreationExactDate.HasValue && e.CreationExactDate.Value.Year >= centuryMin && e.CreationExactDate.Value.Year <= centuryMax) ||
                    (e.CreationYear.HasValue && e.CreationYear.Value >= centuryMin && e.CreationYear.Value <= centuryMax) ||
                    (e.CreationCentury.HasValue && e.CreationCentury.Value == filter.CreationCentury.Value)
                );
            }

            if (filter.CreationYearMin.HasValue)
            {
                query = query.Where(e =>
                    (e.CreationExactDate.HasValue ? e.CreationExactDate.Value.Year :
                     e.CreationYear.HasValue ? e.CreationYear.Value :
                     e.CreationCentury.HasValue ? ((e.CreationCentury.Value - 1) * 100 + 1) : int.MaxValue)
                    >= filter.CreationYearMin.Value);
            }
            if (filter.CreationYearMax.HasValue)
            {
                query = query.Where(e =>
                    (e.CreationExactDate.HasValue ? e.CreationExactDate.Value.Year :
                     e.CreationYear.HasValue ? e.CreationYear.Value :
                     e.CreationCentury.HasValue ? (e.CreationCentury.Value * 100) : int.MinValue)
                    <= filter.CreationYearMax.Value);
            }

            if (filter.EntryDate.HasValue)
                query = query.Where(e => e.EntryDate == filter.EntryDate);

            if (!string.IsNullOrWhiteSpace(filter.CategoryName))
                query = query.Where(e => EF.Functions.Like(e.Category.Name.ToLower(), $"%{filter.CategoryName.ToLower()}%"));

            return query;
        }

        private IQueryable<Exhibit> ApplySorting(IQueryable<Exhibit> query, ExhibitFilter filter)
        {
            switch (filter.SortBy?.ToLower())
            {
                case "name":
                    query = filter.Descending ? query.OrderByDescending(e => e.Name) : query.OrderBy(e => e.Name);
                    break;

                case "creation":
                    query = filter.Descending
                        ? query.OrderByDescending(e =>
                              e.CreationExactDate.HasValue ? e.CreationExactDate :
                              e.CreationYear.HasValue ? new DateOnly(e.CreationYear.Value, 1, 1) :
                              e.CreationCentury.HasValue ? new DateOnly((e.CreationCentury.Value - 1) * 100 + 1, 1, 1) :
                              (DateOnly?)null)
                        : query.OrderBy(e =>
                              e.CreationExactDate.HasValue ? e.CreationExactDate :
                              e.CreationYear.HasValue ? new DateOnly(e.CreationYear.Value, 1, 1) :
                              e.CreationCentury.HasValue ? new DateOnly((e.CreationCentury.Value - 1) * 100 + 1, 1, 1) :
                              (DateOnly?)null);
                    break;

                case "entrydate":
                default:
                    query = filter.Descending ? query.OrderByDescending(e => e.EntryDate) : query.OrderBy(e => e.EntryDate);
                    break;
            }
            return query;
        }

        public async Task<Guid> Create(ExhibitRequest exhibitDto)
        {
            try
            {
                var exhibit = _mapper.Map<Exhibit>(exhibitDto);

                await _exhibitRepository.CreateAsync(exhibit);

                return exhibit.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Update(Guid id, ExhibitRequest exhibitDto)
        {
            try
            {
                var existingExhibit = await _exhibitRepository.GetByIdAsync(id);
                if (existingExhibit == null)
                {
                    throw new ArgumentException("Exhibit not found.");
                }

                _mapper.Map(exhibitDto, existingExhibit);

                await _exhibitRepository.UpdateAsync(existingExhibit);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task PartialUpdate(Guid id, JsonPatchDocument<ExhibitRequest> patchDoc)
        {
            try
            {
                var exhibit = await _exhibitRepository.GetByIdAsync(id);
                if (exhibit == null)
                {
                    throw new ArgumentException("Exhibit not found.");
                }
                var exhibitPatch = _mapper.Map<JsonPatchDocument<Exhibit>>(patchDoc);

                exhibitPatch.ApplyTo(exhibit);

                await _exhibitRepository.UpdateAsync(exhibit);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task Delete(Guid id)
        {
            try
            {
                var exhibit = await _exhibitRepository.GetByIdAsync(id);
                if (exhibit == null)
                {
                    throw new ArgumentException("Exhibit not found.");
                }

                var images = await _imageService.GetByEntityId(id);

                if (images.Any())
                {
                    await _imageService.DeleteByEntityId(id);
                }

                await _exhibitRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }

}
