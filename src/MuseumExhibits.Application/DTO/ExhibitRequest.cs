
using System.ComponentModel.DataAnnotations;

namespace MuseumExhibits.Application.DTO
{
    public class ExhibitRequest
    {    
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string AuthorOrManufacturer { get; set; }

        [StringLength(500)]
        public string? ArchivalMaterials { get; set; }

        [StringLength(500)]
        public string? Bibliography { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Cost { get; set; }

        [StringLength(100)]
        public string? Measurements { get; set; }

        [StringLength(500)]
        public string? ReturnDetails { get; set; }

        [StringLength(500)]
        public string? RestorationDetails { get; set; }

        [Required]
        [StringLength(100)]
        public string ResponsibleKeeper { get; set; }

        [StringLength(100)]
        public string? PreciousMetalContent { get; set; }

        [StringLength(100)]
        public string? PreciousStoneContent { get; set; }

        [StringLength(50)]
        public string? StorageGroup { get; set; }

        [StringLength(500)]
        public string? InventoryVerificationData { get; set; }

        [StringLength(500)]
        public string? MovementDetails { get; set; }

        public DateOnly? RemovalDate { get; set; }

        public DateOnly? EntryDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        public DateOnly? ConfirmationDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [StringLength(500)]
        public string? SourceOfArrival { get; set; }

        [StringLength(500)]
        public string? AdditionalDetails { get; set; }

        [StringLength(500)]
        public string? AssociatedPersonsAndEvents { get; set; }

        [Range(1, int.MaxValue)]
        public int? ElementCount { get; set; }

        [Required]
        [StringLength(100)]
        public string Classification { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Material { get; set; } = string.Empty;

        [StringLength(500)]
        public string? TemporaryStoragePurpose { get; set; }

        [StringLength(200)]
        public string? PlaceOfCreation { get; set; }

        public bool? IsTransportable { get; set; }

        [Required]
        [StringLength(50)]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string? FullDescription { get; set; }

        [StringLength(500)]
        public string? ShortDescription { get; set; }

        [Required]
        [StringLength(100)]
        public string EnteredBy { get; set; } = string.Empty;

        [StringLength(500)]
        public string? RemovalReason { get; set; }

        [StringLength(500)]
        public string? RelatedDocuments { get; set; }

        [StringLength(500)]
        public string? RestorationRecommendations { get; set; }

        [StringLength(500)]
        public string? ArrivalMethod { get; set; }

        [Required]
        [StringLength(100)]
        public string PreservationState { get; set; } = string.Empty;

        public DateOnly? ReturnDeadline { get; set; }

        [StringLength(100)]
        public string? Technique { get; set; }

        public Guid? CategoryId { get; set; }

        [StringLength(500)]
        public string? DiscoveryTimeAndPlace { get; set; }

        [StringLength(500)]
        public string? ExistenceTimeAndPlace { get; set; }

        public DateOnly? CreationExactDate { get; set; }

        [Range(1, 9999)]
        public int? CreationYear { get; set; }

        [Range(1, 99)]
        public int? CreationCentury { get; set; }

        public bool Visible { get; set; } = false;

    }

}
