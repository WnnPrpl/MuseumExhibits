using Microsoft.EntityFrameworkCore;
using MuseumExhibits.Core.Abstractions;
using MuseumExhibits.Core.Models;
using MuseumExhibits.Infrastructure.Data;

namespace MuseumExhibits.Infrastructure.Repositories
{
    public class CollectionRepository(MuseumExhibitsDbContext context) : ICollectionRepository
    {
        private readonly MuseumExhibitsDbContext _context = context;

        public async Task<Collection> GetByIdAsync(Guid id)
        {
            var collection = await _context.Collections
                .Include(c => c.Exhibits)
                    .ThenInclude(e => e.Images)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (collection == null)
                throw new KeyNotFoundException($"Collection with ID {id} not found.");

            return collection;
        }

        public async Task<IEnumerable<Collection>> GetAllAsync() =>
            await _context.Collections
                .AsNoTracking()
                .Include(c => c.Exhibits)
                    .ThenInclude(e => e.Images)
                .ToListAsync();

        public async Task<(IEnumerable<Collection> Collections, int TotalCount)> GetPagedAsync(int page, int pageSize)
        {
            var query = _context.Collections
                .AsNoTracking()
                .Include(c => c.Exhibits)
                    .ThenInclude(e => e.Images);

            int totalCount = await query.CountAsync();

            var collections = await query
                .OrderBy(c => c.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (collections, totalCount);
        }

        public async Task<Guid> CreateAsync(Collection collection)
        {
            _context.Collections.Add(collection);
            await _context.SaveChangesAsync();
            return collection.Id;
        }

        public async Task UpdateAsync(Collection collection)
        {
            _context.Collections.Update(collection);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var collection = await _context.Collections.FindAsync(id)
                ?? throw new KeyNotFoundException($"Collection with ID {id} not found.");

            _context.Collections.Remove(collection);
            await _context.SaveChangesAsync();
        }
    }
}
