namespace MuseumExhibits.Application.DTO
{
    public class CollectionResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<ExhibitSummaryDTO> Exhibits { get; set; } = [];
    }
}
