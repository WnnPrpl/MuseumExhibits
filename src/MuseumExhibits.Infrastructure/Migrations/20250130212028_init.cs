using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuseumExhibits.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exhibit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    AuthorOrManufacturer = table.Column<string>(type: "text", nullable: false),
                    ArchivalMaterials = table.Column<string>(type: "text", nullable: false),
                    Bibliography = table.Column<string>(type: "text", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric", nullable: false),
                    Measurements = table.Column<string>(type: "text", nullable: false),
                    ReturnDetails = table.Column<string>(type: "text", nullable: false),
                    RestorationDetails = table.Column<string>(type: "text", nullable: false),
                    ResponsibleKeeper = table.Column<string>(type: "text", nullable: false),
                    PreciousMetalContent = table.Column<string>(type: "text", nullable: false),
                    PreciousStoneContent = table.Column<string>(type: "text", nullable: false),
                    StorageGroup = table.Column<string>(type: "text", nullable: false),
                    InventoryVerificationData = table.Column<string>(type: "text", nullable: false),
                    MovementDetails = table.Column<string>(type: "text", nullable: false),
                    RemovalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConfirmationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SourceOfArrival = table.Column<string>(type: "text", nullable: false),
                    AdditionalDetails = table.Column<string>(type: "text", nullable: false),
                    AssociatedPersonsAndEvents = table.Column<string>(type: "text", nullable: false),
                    ElementCount = table.Column<int>(type: "integer", nullable: false),
                    Classification = table.Column<string>(type: "text", nullable: false),
                    Material = table.Column<string>(type: "text", nullable: false),
                    TemporaryStoragePurpose = table.Column<string>(type: "text", nullable: false),
                    PlaceOfCreation = table.Column<string>(type: "text", nullable: false),
                    IsTransportable = table.Column<bool>(type: "boolean", nullable: false),
                    MuseumDetails = table.Column<string>(type: "text", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "text", nullable: false),
                    FullDescription = table.Column<string>(type: "text", nullable: false),
                    ShortDescription = table.Column<string>(type: "text", nullable: false),
                    EnteredBy = table.Column<string>(type: "text", nullable: false),
                    RemovalReason = table.Column<string>(type: "text", nullable: false),
                    RelatedDocuments = table.Column<string>(type: "text", nullable: false),
                    RestorationRecommendations = table.Column<string>(type: "text", nullable: false),
                    ArrivalMethod = table.Column<string>(type: "text", nullable: false),
                    PreservationState = table.Column<string>(type: "text", nullable: false),
                    ReturnDeadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Technique = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    DiscoveryTimeAndPlace = table.Column<string>(type: "text", nullable: false),
                    ExistenceTimeAndPlace = table.Column<string>(type: "text", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Visible = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exhibit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exhibit_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    PublicId = table.Column<string>(type: "text", nullable: false),
                    IsTitleImage = table.Column<bool>(type: "boolean", nullable: false),
                    ExhibitId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Image_Exhibit_ExhibitId",
                        column: x => x.ExhibitId,
                        principalTable: "Exhibit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exhibit_CategoryId",
                table: "Exhibit",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_ExhibitId",
                table: "Image",
                column: "ExhibitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Exhibit");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
