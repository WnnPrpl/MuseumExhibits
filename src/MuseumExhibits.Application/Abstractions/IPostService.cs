using MuseumExhibits.Application.DTO;
using MuseumExhibits.Core.Filters;

namespace MuseumExhibits.Application.Abstractions
{
    public interface IPostService
    {
        Task<PostResponse> GetById(Guid id);
        Task<PagedResult<PostResponse>> Get(PostQueryParameters queryParams, bool isAdmin = false);
        Task<Guid> Create(PostRequest request);
        Task Update(Guid id, PostRequest request);
        Task Delete(Guid id);
    }
}
