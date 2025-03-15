using Microsoft.EntityFrameworkCore;
using MuseumExhibits.Core.Abstractions;
using MuseumExhibits.Core.Models;
using MuseumExhibits.Infrastructure.Data;

namespace MuseumExhibits.Infrastructure.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly MuseumExhibitsDbContext _context;

        public AdminRepository(MuseumExhibitsDbContext context)
        {
            _context = context;
        }

        public async Task<Admin> GetByEmailAsync(string email)
        {
            return await _context.Admins.FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task CreateAsync(Admin admin)
        {
            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();
        }
    }
}
