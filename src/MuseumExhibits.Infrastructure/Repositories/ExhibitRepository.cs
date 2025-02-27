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
            return await _context.Exhibits
                .AsNoTracking()
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id);

        }

        public async Task<IEnumerable<Exhibit>> GetAllAsync(bool isAdmin)
        {
            var query = _context.Exhibits.AsQueryable();

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

        public async Task<IEnumerable<Exhibit>> GetByCategoryIdAsync(Guid categoryId, bool isAdmin)
        {
            var query = _context.Exhibits.AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(e => e.Visible);
            }

            return await query.AsNoTracking().Where(e => e.CategoryId == categoryId).ToListAsync();
        }

        public async Task<(IEnumerable<Exhibit> Exhibits, int TotalCount)> GetExhibitsAsync(ExhibitFilter filter)
        {
            IQueryable<Exhibit> query = _context.Exhibits.AsNoTracking();

            // Фільтрація
            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(e => e.Name.Contains(filter.Name));

            if (filter.CreationExactDate.HasValue)
                query = query.Where(e => e.CreationExactDate == filter.CreationExactDate);

            else if (filter.CreationYear.HasValue)
                query = query.Where(e => e.CreationYear == filter.CreationYear);

            else if (filter.CreationCentury.HasValue)
                query = query.Where(e => e.CreationCentury == filter.CreationCentury);

            if (filter.EntryDate.HasValue)
                query = query.Where(e => e.EntryDate == filter.EntryDate);

            //if (filter.CategoryId.HasValue)
            //    query = query.Where(e => e.CategoryId == filter.CategoryId);

            // Сортування
            switch (filter.SortBy?.ToLower())
            {
                case "name":
                    query = filter.Descending ? query.OrderByDescending(e => e.Name) : query.OrderBy(e => e.Name);
                    break;
                case "creationexactdate":
                    query = filter.Descending ? query.OrderByDescending(e => e.CreationExactDate) : query.OrderBy(e => e.CreationExactDate);
                    break;
                case "entrydate":
                default:
                    query = filter.Descending ? query.OrderByDescending(e => e.EntryDate) : query.OrderBy(e => e.EntryDate);
                    break;
            }

            int totalCount = await query.CountAsync();

            query = query.Skip((filter.PageNumber - 1) * filter.PageSize)
                         .Take(filter.PageSize);


            var exhibits = await query.ToListAsync();
            return (exhibits, totalCount);
        }

    }
}
