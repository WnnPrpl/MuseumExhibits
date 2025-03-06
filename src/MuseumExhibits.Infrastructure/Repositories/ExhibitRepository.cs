using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MuseumExhibits.Core.Abstractions;
using MuseumExhibits.Core.Filters;
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
            try
            {
                return await _context.Exhibits
                    .AsNoTracking()
                    .Include(e => e.Category)
                    .Include(e => e.Images)
                    .FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving exhibit from the database.", ex);
            }
        }

        public IQueryable<Exhibit> Get()
        {
            return _context.Exhibits
                .AsNoTracking()
                .Include(e => e.Category)
                .Include(e => e.Images);
        }

        public async Task<Guid> CreateAsync(Exhibit exhibit)
        {
            _context.Exhibits.Add(exhibit);
            await _context.SaveChangesAsync();
            return exhibit.Id;
        }

        public async Task UpdateAsync(Exhibit exhibit)
        {
            _context.Exhibits.Update(exhibit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var exhibit = await GetByIdAsync(id);
            if (exhibit == null)
            {
                throw new KeyNotFoundException($"Exhibit with ID {id} not found.");
            }
            _context.Exhibits.Remove(exhibit);
            await _context.SaveChangesAsync();
        }
       

    }
}
