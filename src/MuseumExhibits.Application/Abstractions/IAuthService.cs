
using MuseumExhibits.Application.DTO;

namespace MuseumExhibits.Application.Abstractions
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginRequest request);
        Task<string> RegisterAsync(RegisterRequest request);
    }
}
