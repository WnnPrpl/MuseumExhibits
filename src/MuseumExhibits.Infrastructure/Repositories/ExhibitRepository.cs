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

        public async Task<(IEnumerable<Exhibit> Exhibits, int TotalCount)> GetAsync(ExhibitFilter filter, bool isAdmin)
        {
            try
            {
                IQueryable<Exhibit> query = _context.Exhibits
                    .AsNoTracking()
                    .Include(e => e.Category)
                    .Include(e => e.Images);

                query = ApplyFiltering(query, filter);
                query = ApplySorting(query, filter);

                int totalCount = await query.CountAsync();

                if (!isAdmin)
                {
                    query = query.Where(e => e.Visible);
                }

                query = query.Skip((filter.PageNumber - 1) * filter.PageSize)
                             .Take(filter.PageSize);

                var exhibits = await query.ToListAsync();
                return (exhibits, totalCount);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving exhibits from the database.", ex);
            }
        }

        private IQueryable<Exhibit> ApplyFiltering(IQueryable<Exhibit> query, ExhibitFilter filter)
        {
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

            if (!string.IsNullOrWhiteSpace(filter.CategoryName))
            {
                query = query.Where(e => e.Category != null && e.Category.Name.Contains(filter.CategoryName));
            }
            return query;
        }

        private IQueryable<Exhibit> ApplySorting(IQueryable<Exhibit> query, ExhibitFilter filter)
        {
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
            return query;
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
