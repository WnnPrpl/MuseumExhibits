using Microsoft.EntityFrameworkCore;
using MuseumExhibits.Core.Abstractions;
using MuseumExhibits.Core.Filters;
using MuseumExhibits.Core.Models;
using MuseumExhibits.Infrastructure.Data;

namespace MuseumExhibits.Infrastructure.Repositories
{
    public class PostRepository(MuseumExhibitsDbContext context) : IPostRepository
    {
        private readonly MuseumExhibitsDbContext _context = context;

        public async Task<Post> GetByIdAsync(Guid id)
        {
            var post = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
                throw new KeyNotFoundException($"Post with ID {id} not found.");

            return post;
        }

        public async Task<(IEnumerable<Post> Posts, int TotalCount)> GetAsync(PostFilter filter, bool isAdmin)
        {
            IQueryable<Post> query = _context.Posts.AsNoTracking();

            if (!isAdmin)
                query = query.Where(p => p.Visible);

            if (!string.IsNullOrWhiteSpace(filter.Title))
                query = query.Where(p => EF.Functions.Like(p.Title.ToLower(), $"%{filter.Title.ToLower()}%"));

            if (!string.IsNullOrWhiteSpace(filter.Category))
                query = query.Where(p => p.Category == filter.Category);

            query = query.OrderByDescending(p => p.PublishedAt);

            int totalCount = await query.CountAsync();

            var posts = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return (posts, totalCount);
        }

        public async Task<Guid> CreateAsync(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post.Id;
        }

        public async Task UpdateAsync(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var post = await _context.Posts.FindAsync(id)
                ?? throw new KeyNotFoundException($"Post with ID {id} not found.");

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }
}
