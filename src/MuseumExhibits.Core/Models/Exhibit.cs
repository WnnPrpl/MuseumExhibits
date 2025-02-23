using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using static System.Net.Mime.MediaTypeNames;

namespace MuseumExhibits.Core.Models
{
    public class Exhibit
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string AuthorOrManufacturer { get; set; } = string.Empty;

        public string? ArchivalMaterials { get; set; }

        public string? Bibliography { get; set; }

        public decimal? Cost { get; set; }

        public string? Measurements { get; set; }

        public string? ReturnDetails { get; set; }

        public string? RestorationDetails { get; set; }

        [Required]
        public string ResponsibleKeeper { get; set; } = string.Empty;

        public string? PreciousMetalContent { get; set; }

        public string? PreciousStoneContent { get; set; }

        public string? StorageGroup { get; set; }

        public string? InventoryVerificationData { get; set; }

        public string? MovementDetails { get; set; }

        public DateOnly? RemovalDate { get; set; }

        public DateOnly? EntryDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        public DateOnly? ConfirmationDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        public string? SourceOfArrival { get; set; }

        public string? AdditionalDetails { get; set; }

        public string? AssociatedPersonsAndEvents { get; set; }

        public int? ElementCount { get; set; }

        public string Classification { get; set; } = string.Empty;

        [Required]
        public string Material { get; set; } = string.Empty;

        public string? TemporaryStoragePurpose { get; set; }

        public string? PlaceOfCreation { get; set; }

        public bool? IsTransportable { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string MuseumDetails { get; set; } = string.Empty;

        [Required]
        public string RegistrationNumber { get; set; } = string.Empty;
        [Required]
        public string? FullDescription { get; set; }

        public string? ShortDescription { get; set; }

        [Required]
        public string EnteredBy { get; set; } = string.Empty;

        public string? RemovalReason { get; set; }

        public string? RelatedDocuments { get; set; }

        public string? RestorationRecommendations { get; set; }

        public string? ArrivalMethod { get; set; }

        [Required]
        public string PreservationState { get; set; }

        public DateOnly? ReturnDeadline { get; set; }

        public string? Technique { get; set; }

        public Guid? CategoryId { get; set; }

        public Category? Category { get; set; }

        public string? DiscoveryTimeAndPlace { get; set; }

        public string? ExistenceTimeAndPlace { get; set; }

        //для можливості введення дати в різних форматах
        public DateOnly? CreationExactDate { get; set; }
        public int? CreationYear { get; set; }
        public int? CreationCentury { get; set; }

        public List<Image> Images { get; set; } = new List<Image>();

        public bool Visible { get; set; } = false;
    }

}
