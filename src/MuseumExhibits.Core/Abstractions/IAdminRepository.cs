
using MuseumExhibits.Core.Models;

namespace MuseumExhibits.Core.Abstractions
{
    public interface IAdminRepository
    {
        Task<Admin> GetByEmailAsync(string email);
        Task CreateAsync(Admin admin);
    }
}
