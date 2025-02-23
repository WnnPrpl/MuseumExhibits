using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MuseumExhibits.Core.Abstractions;
using MuseumExhibits.Core.Models;
using MuseumExhibits.Infrastructure.Data;

namespace MuseumExhibits.Infrastructure.Repostories
{
    public class ExhibitRepository : IExhibitRepository
    {
        private readonly MuseumExhibitsDbContext _context;

        public ExhibitRepository(MuseumExhibitsDbContext context)
        {
            _context = context;
        }

        public async Task<Exhibit> GetByIdAsync(Guid id)
        {
            return await _context.Exhibit
                .AsNoTracking()
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id);

        }

        public async Task<IEnumerable<Exhibit>> GetAllAsync(bool isAdmin)
        {
            var query = _context.Exhibit.AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(e => e.Visible);
            }

            return await query.AsNoTracking()
                .Include(e => e.Category)
                .ToListAsync();
        }

        public async Task<Guid> CreateAsync(Exhibit exhibit)
        {
            _context.Exhibit.Add(exhibit);
            await _context.SaveChangesAsync();
            return exhibit.Id;
        }

        public async Task UpdateAsync(Exhibit exhibit)
        {
            _context.Exhibit.Update(exhibit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var exhibit = await GetByIdAsync(id);
            if (exhibit == null)
            {
                throw new KeyNotFoundException($"Exhibit with ID {id} not found.");
            }
            _context.Exhibit.Remove(exhibit);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Exhibit>> GetByCategoryIdAsync(Guid categoryId, bool isAdmin)
        {
            var query = _context.Exhibit.AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(e => e.Visible);
            }

            return await query.AsNoTracking().Where(e => e.CategoryId == categoryId).ToListAsync();
        }

        public async Task<IEnumerable<Exhibit>> GetByPageAsync(int page, int pageSize, bool isAdmin)
        {
            var query = _context.Exhibit.AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(e => e.Visible);
            }

            return await query
                .AsNoTracking()
                .Include(e => e.Category)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

    }
}
