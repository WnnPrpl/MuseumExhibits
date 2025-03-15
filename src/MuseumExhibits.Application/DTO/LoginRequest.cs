using System.ComponentModel.DataAnnotations;


namespace MuseumExhibits.Application.DTO
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
