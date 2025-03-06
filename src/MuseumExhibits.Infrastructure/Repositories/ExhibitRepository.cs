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
                query = query.Where(e => EF.Functions.Like(e.Name.ToLower(), $"%{filter.Name.ToLower()}%"));

            if (filter.CreationExactDate.HasValue)
            {
                query = query.Where(e => e.CreationExactDate == filter.CreationExactDate);
            }

            if (filter.CreationYear.HasValue)
            {
                query = query.Where(e => e.CreationYear == filter.CreationYear);
            }

            if (filter.CreationCentury.HasValue)
            {
                int centuryMin = (filter.CreationCentury.Value - 1) * 100 + 1;
                int centuryMax = filter.CreationCentury.Value * 100;

                query = query.Where(e =>
                    (e.CreationExactDate.HasValue && e.CreationExactDate.Value.Year >= centuryMin && e.CreationExactDate.Value.Year <= centuryMax) ||
                    (e.CreationYear.HasValue && e.CreationYear.Value >= centuryMin && e.CreationYear.Value <= centuryMax) ||
                    (e.CreationCentury.HasValue && e.CreationCentury.Value == filter.CreationCentury.Value)
                );
            }

            if (filter.CreationYearMin.HasValue)
            {
                query = query.Where(e =>
                    (e.CreationExactDate.HasValue ? e.CreationExactDate.Value.Year :
                     e.CreationYear.HasValue ? e.CreationYear.Value :
                     e.CreationCentury.HasValue ? ((e.CreationCentury.Value - 1) * 100 + 1) : int.MaxValue)
                    >= filter.CreationYearMin.Value);
            }

            if (filter.CreationYearMax.HasValue)
            {
                query = query.Where(e =>
                    (e.CreationExactDate.HasValue ? e.CreationExactDate.Value.Year :
                     e.CreationYear.HasValue ? e.CreationYear.Value :
                     e.CreationCentury.HasValue ? (e.CreationCentury.Value * 100) : int.MinValue)
                    <= filter.CreationYearMax.Value);
            }

            if (filter.EntryDate.HasValue)
                query = query.Where(e => e.EntryDate == filter.EntryDate);

            if (!string.IsNullOrWhiteSpace(filter.CategoryName))
                query = query.Where(e => EF.Functions.Like(e.Category.Name.ToLower(), $"%{filter.CategoryName.ToLower()}%"));

            return query;
        }

        private IQueryable<Exhibit> ApplySorting(IQueryable<Exhibit> query, ExhibitFilter filter)
        {
            switch (filter.SortBy?.ToLower())
            {
                case "name":
                    query = filter.Descending ? query.OrderByDescending(e => e.Name) : query.OrderBy(e => e.Name);
                    break;

                case "creation":
                    query = filter.Descending
                        ? query.OrderByDescending(e =>
                              e.CreationExactDate.HasValue ? e.CreationExactDate :
                              e.CreationYear.HasValue ? new DateOnly(e.CreationYear.Value, 1, 1) :
                              e.CreationCentury.HasValue ? new DateOnly((e.CreationCentury.Value - 1) * 100 + 1, 1, 1) :
                              (DateOnly?)null)
                        : query.OrderBy(e =>
                              e.CreationExactDate.HasValue ? e.CreationExactDate :
                              e.CreationYear.HasValue ? new DateOnly(e.CreationYear.Value, 1, 1) :
                              e.CreationCentury.HasValue ? new DateOnly((e.CreationCentury.Value - 1) * 100 + 1, 1, 1) :
                              (DateOnly?)null);
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
