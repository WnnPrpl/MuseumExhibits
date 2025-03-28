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
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exhibits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorOrManufacturer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArchivalMaterials = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bibliography = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Measurements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReturnDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RestorationDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponsibleKeeper = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreciousMetalContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreciousStoneContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StorageGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InventoryVerificationData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MovementDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemovalDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EntryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ConfirmationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    SourceOfArrival = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssociatedPersonsAndEvents = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ElementCount = table.Column<int>(type: "int", nullable: true),
                    Classification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Material = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemporaryStoragePurpose = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaceOfCreation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTransportable = table.Column<bool>(type: "bit", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MuseumDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnteredBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RemovalReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelatedDocuments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RestorationRecommendations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArrivalMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreservationState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReturnDeadline = table.Column<DateOnly>(type: "date", nullable: true),
                    Technique = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DiscoveryTimeAndPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExistenceTimeAndPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationExactDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CreationYear = table.Column<int>(type: "int", nullable: true),
                    CreationCentury = table.Column<int>(type: "int", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exhibits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exhibits_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsTitleImage = table.Column<bool>(type: "bit", nullable: false),
                    ExhibitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Exhibits_ExhibitId",
                        column: x => x.ExhibitId,
                        principalTable: "Exhibits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exhibits_CategoryId",
                table: "Exhibits",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ExhibitId",
                table: "Images",
                column: "ExhibitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Exhibits");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
