namespace MuseumExhibits.Application.DTO
{
    public class CollectionSummaryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ExhibitCount { get; set; }
    }
}
