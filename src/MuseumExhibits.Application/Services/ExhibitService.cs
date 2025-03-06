using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
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
                var filters = _mapper.Map<ExhibitFilter>(queryParams);

                var (exhibits, totalCount) = await _exhibitRepository.GetAsync(filters, isAdmin);

                var exhibitDtos = _mapper.Map<IEnumerable<ExhibitSummaryDTO>>(exhibits);

                var pagedResult = new PagedResult<ExhibitSummaryDTO>(exhibitDtos, totalCount, queryParams.PageNumber, queryParams.PageSize);
                return pagedResult;
            }

            catch (Exception ex)
            {
                throw new ApplicationException("Error while retrieving exhibits.", ex);
            }
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
