
namespace MuseumExhibits.Core.Models
{
    public class Admin
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }

    }
}
