
using System.ComponentModel.DataAnnotations;

namespace MuseumExhibits.Application.DTO
{
    public class ExhibitQueryParameters
    {
        public string? Name { get; set; }
        public int? CreationYear { get; set; }
        public int? CreationCentury { get; set; }
        public DateOnly? CreationExactDate { get; set; }

        public int? CreationYearMin { get; set; }
        public int? CreationYearMax { get; set; }


        public DateOnly? EntryDate { get; set; }
        public string? CategoryName { get; set; }

        public string? SortBy { get; set; } = "EntryDate";
        public bool Descending { get; set; } = true;

        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, int.MaxValue)]
        public int PageSize { get; set; } = 10;
    }
}
