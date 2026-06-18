using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using MuseumExhibits.Application.Abstractions;
using MuseumExhibits.Application.DTO;
using MuseumExhibits.Core.Abstractions;
using MuseumExhibits.Core.Filters;
using MuseumExhibits.Core.Models;

namespace MuseumExhibits.Application.Services
{
    public class ExhibitService(
        IExhibitRepository exhibitRepository,
        ICategoryService categoryService,
        IImageService imageService,
        IMapper mapper) : IExhibitService
    {
        private readonly IExhibitRepository _exhibitRepository = exhibitRepository;
        private readonly ICategoryService _categoryService = categoryService;
        private readonly IImageService _imageService = imageService;
        private readonly IMapper _mapper = mapper;

        public async Task<ExhibitResponse> GetById(Guid id)
        {
            var exhibit = await _exhibitRepository.GetByIdAsync(id);
            return _mapper.Map<ExhibitResponse>(exhibit);
        }

        public async Task<PagedResult<ExhibitSummaryDTO>> Get(ExhibitQueryParameters queryParams, bool isAdmin = false)
        {
            var filters = _mapper.Map<ExhibitFilter>(queryParams);
            var (exhibits, totalCount) = await _exhibitRepository.GetAsync(filters, isAdmin);
            var exhibitDtos = _mapper.Map<IEnumerable<ExhibitSummaryDTO>>(exhibits);

            return new PagedResult<ExhibitSummaryDTO>(exhibitDtos, totalCount, queryParams.PageNumber ?? 1, queryParams.PageSize ?? 10);
        }

        public async Task<Guid> Create(ExhibitRequest exhibitDto)
        {
            if (exhibitDto.CategoryId != null)
                await _categoryService.GetById((Guid)exhibitDto.CategoryId);

            var exhibit = _mapper.Map<Exhibit>(exhibitDto);
            await _exhibitRepository.CreateAsync(exhibit);

            return exhibit.Id;
        }

        public async Task Update(Guid id, ExhibitRequest exhibitDto)
        {
            var existingExhibit = await _exhibitRepository.GetByIdAsync(id);

            _mapper.Map(exhibitDto, existingExhibit);

            if (exhibitDto.CategoryId.HasValue)
            {
                var category = await _categoryService.GetById(exhibitDto.CategoryId.Value);
                existingExhibit.Category = null;
                existingExhibit.CategoryId = category.Id;
            }
            else
            {
                existingExhibit.Category = null;
                existingExhibit.CategoryId = null;
            }

            await _exhibitRepository.UpdateAsync(existingExhibit);
        }

        public async Task PartialUpdate(Guid id, JsonPatchDocument<ExhibitRequest> patchDoc)
        {
            var exhibit = await _exhibitRepository.GetByIdAsync(id);
            var exhibitDto = _mapper.Map<ExhibitRequest>(exhibit);

            patchDoc.ApplyTo(exhibitDto);

            if (exhibitDto.CategoryId.HasValue)
            {
                var category = await _categoryService.GetById(exhibitDto.CategoryId.Value);
                exhibit.Category = null;
                exhibit.CategoryId = category.Id;
            }
            else if (patchDoc.Operations.Any(op => op.path == "/categoryId" && op.OperationType == OperationType.Remove))
            {
                exhibit.Category = null;
                exhibit.CategoryId = null;
            }

            _mapper.Map(exhibitDto, exhibit);
            await _exhibitRepository.UpdateAsync(exhibit);
        }

        public async Task Delete(Guid id)
        {
            await _exhibitRepository.GetByIdAsync(id);

            var images = await _imageService.GetByEntityId(id);
            if (images.Any())
                await _imageService.DeleteByEntityId(id);

            await _exhibitRepository.DeleteAsync(id);
        }
    }
}
