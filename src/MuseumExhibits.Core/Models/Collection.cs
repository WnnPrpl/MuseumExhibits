namespace MuseumExhibits.Core.Models
{
    public class Collection
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<Exhibit> Exhibits { get; set; } = [];
    }
}
