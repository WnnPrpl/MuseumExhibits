using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MuseumExhibits.Application.Abstractions;
using MuseumExhibits.Application.DTO;
using MuseumExhibits.Core.Abstractions;
using MuseumExhibits.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MuseumExhibits.Application.Services
{
    public class AuthService(IAdminRepository adminRepository, IConfiguration configuration) : IAuthService
    {
        private readonly IAdminRepository _adminRepository = adminRepository;
        private readonly IConfiguration _configuration = configuration;

        public async Task<string> RegisterAsync(RegisterRequest request)
        {
            var existingAdmin = await _adminRepository.GetByEmailAsync(request.Email);
            if (existingAdmin != null)
                throw new Exception("User with this email already exists.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var admin = new Admin
            {
                Email = request.Email,
                FullName = request.FullName,
                HashedPassword = hashedPassword
            };

            await _adminRepository.CreateAsync(admin);
            return GenerateJwtToken(admin);
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            var admin = await _adminRepository.GetByEmailAsync(request.Email);
            if (admin == null || !BCrypt.Net.BCrypt.Verify(request.Password, admin.HashedPassword))
                throw new Exception("Wrong email or password.");

            return GenerateJwtToken(admin);
        }

        public string GenerateJwtToken(Admin admin)
        {
            var secretKey = _configuration["JwtSettings:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
                throw new Exception("No JWT Secret Key in config.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            List<Claim> claims =
            [
                new Claim(JwtRegisteredClaimNames.Sub, admin.Email),
                new Claim(ClaimTypes.Name, admin.FullName),
                new Claim("AdminId", admin.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];

            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var expiryMinutes = int.Parse(_configuration["JwtSettings:ExpiryMinutes"]!);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
