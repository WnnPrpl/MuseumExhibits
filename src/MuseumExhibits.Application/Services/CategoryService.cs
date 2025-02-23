using AutoMapper;
using MuseumExhibits.Application.Abstractions;
using MuseumExhibits.Application.DTO;
using MuseumExhibits.Core.Abstractions;
using MuseumExhibits.Core.Models;

namespace MuseumExhibits.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CategoryDTO> GetById(Guid id)
        {
            var category = await _repository.GetByIdAsync(id);
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<IEnumerable<CategoryDTO>> GetAll()
        {
            var categories = await _repository.GetAllAsync();
            return categories.Select(c => _mapper.Map<CategoryDTO>(c));
        }

        public async Task<Guid> Create(CategoryDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            return await _repository.CreateAsync(category);
        }

        public async Task Update(Guid id, CategoryDTO categoryDto)
        {
            var category = await _repository.GetByIdAsync(id);
            _mapper.Map(categoryDto, category);

            await _repository.UpdateAsync(category);
        }

        public async Task Delete(Guid id)
        {
            var category = await _repository.GetByIdAsync(id);

            await _repository.DeleteAsync(category);
        }

        public async Task<List<CategoryDTO>> GetByPage(int page, int pageSize)
        {
            var categories = await _repository.GetByPageAsync(page, pageSize);
            return categories.Select(c => _mapper.Map<CategoryDTO>(c)).ToList();
        }

    }
}
