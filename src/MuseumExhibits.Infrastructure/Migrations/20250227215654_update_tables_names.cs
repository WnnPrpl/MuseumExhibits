using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuseumExhibits.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_tables_names : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exhibit_Category_CategoryId",
                table: "Exhibit");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_Exhibit_ExhibitId",
                table: "Image");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Image",
                table: "Image");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exhibit",
                table: "Exhibit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "Image",
                newName: "Images");

            migrationBuilder.RenameTable(
                name: "Exhibit",
                newName: "Exhibits");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameIndex(
                name: "IX_Image_ExhibitId",
                table: "Images",
                newName: "IX_Images_ExhibitId");

            migrationBuilder.RenameIndex(
                name: "IX_Exhibit_CategoryId",
                table: "Exhibits",
                newName: "IX_Exhibits_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                table: "Images",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exhibits",
                table: "Exhibits",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Exhibits_Categories_CategoryId",
                table: "Exhibits",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Exhibits_ExhibitId",
                table: "Images",
                column: "ExhibitId",
                principalTable: "Exhibits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exhibits_Categories_CategoryId",
                table: "Exhibits");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Exhibits_ExhibitId",
                table: "Images");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exhibits",
                table: "Exhibits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Images",
                newName: "Image");

            migrationBuilder.RenameTable(
                name: "Exhibits",
                newName: "Exhibit");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameIndex(
                name: "IX_Images_ExhibitId",
                table: "Image",
                newName: "IX_Image_ExhibitId");

            migrationBuilder.RenameIndex(
                name: "IX_Exhibits_CategoryId",
                table: "Exhibit",
                newName: "IX_Exhibit_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Image",
                table: "Image",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exhibit",
                table: "Exhibit",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Exhibit_Category_CategoryId",
                table: "Exhibit",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Exhibit_ExhibitId",
                table: "Image",
                column: "ExhibitId",
                principalTable: "Exhibit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
