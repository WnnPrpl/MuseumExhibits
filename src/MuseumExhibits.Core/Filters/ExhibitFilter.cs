
namespace MuseumExhibits.Core.Filters
{
    public class ExhibitFilter
    {
        // Фільтрація
        public string? Name { get; set; }
        public int? CreationYear { get; set; }
        public int? CreationCentury { get; set; }
        public DateOnly? CreationExactDate { get; set; }
        public DateOnly? EntryDate { get; set; }
        public Guid? CategoryId { get; set; }

        // Сортування
        public string? SortBy { get; set; } = "EntryDate";
        public bool Descending { get; set; } = true;

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
