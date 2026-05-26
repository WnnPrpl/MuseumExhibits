using Microsoft.EntityFrameworkCore;
using MuseumExhibits.Core.Abstractions;
using MuseumExhibits.Core.Models;
using MuseumExhibits.Infrastructure.Data;

namespace MuseumExhibits.Infrastructure.Repositories
{
    public class AdminRepository(MuseumExhibitsDbContext context) : IAdminRepository
    {
        private readonly MuseumExhibitsDbContext _context = context;

        public async Task<Admin?> GetByEmailAsync(string email) =>
            await _context.Admins.FirstOrDefaultAsync(a => a.Email == email);

        public async Task CreateAsync(Admin admin)
        {
            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();
        }
    }
}
