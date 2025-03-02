
namespace MuseumExhibits.Application.DTO
{
    public class ExhibitSummaryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string MainImageURL { get; set; }
        public CategoryDTO Category { get; set; }
    }
}
