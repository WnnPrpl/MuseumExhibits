using MuseumExhibits.Core.Filters;
using MuseumExhibits.Core.Models;

namespace MuseumExhibits.Core.Abstractions
{
    public interface IPostRepository
    {
        Task<Post> GetByIdAsync(Guid id);
        Task<(IEnumerable<Post> Posts, int TotalCount)> GetAsync(PostFilter filter, bool isAdmin);
        Task<Guid> CreateAsync(Post post);
        Task UpdateAsync(Post post);
        Task DeleteAsync(Guid id);
    }
}
