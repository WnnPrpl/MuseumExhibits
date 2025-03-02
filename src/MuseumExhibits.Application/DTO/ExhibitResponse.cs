
namespace MuseumExhibits.Application.DTO
{
    public class ExhibitResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AuthorOrManufacturer { get; set; } = string.Empty;
        public string? ArchivalMaterials { get; set; }
        public string? Bibliography { get; set; }
        public decimal? Cost { get; set; }
        public string? Measurements { get; set; }
        public string? ReturnDetails { get; set; }
        public string? RestorationDetails { get; set; }
        public string ResponsibleKeeper { get; set; } = string.Empty;
        public string? PreciousMetalContent { get; set; }
        public string? PreciousStoneContent { get; set; }
        public string? StorageGroup { get; set; }
        public string? InventoryVerificationData { get; set; }
        public string? MovementDetails { get; set; }
        public DateOnly? RemovalDate { get; set; }
        public DateOnly? EntryDate { get; set; }
        public DateOnly? ConfirmationDate { get; set; }
        public string? SourceOfArrival { get; set; }
        public string? AdditionalDetails { get; set; }
        public string? AssociatedPersonsAndEvents { get; set; }
        public int? ElementCount { get; set; }
        public string Classification { get; set; } = string.Empty;
        public string Material { get; set; } = string.Empty;
        public string? TemporaryStoragePurpose { get; set; }
        public string? PlaceOfCreation { get; set; }
        public bool? IsTransportable { get; set; }
        public string RegistrationNumber { get; set; } = string.Empty;
        public string? FullDescription { get; set; }
        public string? ShortDescription { get; set; }
        public string EnteredBy { get; set; } = string.Empty;
        public string? RemovalReason { get; set; }
        public string? RelatedDocuments { get; set; }
        public string? RestorationRecommendations { get; set; }
        public string? ArrivalMethod { get; set; }
        public string PreservationState { get; set; } = string.Empty;
        public DateOnly? ReturnDeadline { get; set; }
        public string? Technique { get; set; }
        public CategoryDTO? Category { get; set; }
        public string? DiscoveryTimeAndPlace { get; set; }
        public string? ExistenceTimeAndPlace { get; set; }
        public DateOnly? CreationExactDate { get; set; }
        public int? CreationYear { get; set; }
        public int? CreationCentury { get; set; }
        public bool Visible { get; set; }

        public List<ImageResponse> Images { get; set; } = new List<ImageResponse>();
    }
}
