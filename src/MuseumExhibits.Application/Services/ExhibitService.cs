using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MuseumExhibits.Application.Abstractions;
using MuseumExhibits.Application.DTO;
using MuseumExhibits.Core.Abstractions;
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

        public async Task<ExhibitDTO> GetById(Guid id)
        {
            var exhibit = await _exhibitRepository.GetByIdAsync(id);
            if (exhibit == null)
            {
                throw new ArgumentException("Exhibit not found.");
            }

            var exhibitDTO = _mapper.Map<ExhibitDTO>(exhibit);

            return exhibitDTO;
        }

        public async Task<IEnumerable<ExhibitDTO>> GetAll(bool isAdmin)
        {
            var exhibits = await _exhibitRepository.GetAllAsync(isAdmin);
            return exhibits.Select(exhibit => _mapper.Map<ExhibitDTO>(exhibit));
        }

        public async Task<Guid> Create(ExhibitDTO exhibitDto)
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

        public async Task Update(Guid id, ExhibitDTO exhibitDto)
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

        public async Task<IEnumerable<ExhibitDTO>> GetByCategoryId(Guid categoryId, bool isAdmin)
        {
            var exhibits = await _exhibitRepository.GetByCategoryIdAsync(categoryId, isAdmin);
            return exhibits.Select(exhibit => _mapper.Map<ExhibitDTO>(exhibit));
        }

        public async Task<IEnumerable<ExhibitDTO>> GetByPage(int page, int pageSize, bool isAdmin)
        {
            var exhibits = await _exhibitRepository.GetByPageAsync(page, pageSize, isAdmin);
            return exhibits.Select(exhibit => _mapper.Map<ExhibitDTO>(exhibit));
        }
    }

}
