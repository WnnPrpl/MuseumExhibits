using MuseumExhibits.Application.DTO;

namespace MuseumExhibits.Application.Abstractions
{
    public interface ICategoryService
    {
        Task<CategoryDTO> GetById(Guid id);
        Task<IEnumerable<CategoryDTO>> GetAll();
        Task<Guid> Create(CategoryDTO categoryDto);
        Task Update(Guid id, CategoryDTO categoryDto);
        Task Delete(Guid id);
        Task<List<CategoryDTO>> GetByPage(int page, int pageSize);
    }
}
