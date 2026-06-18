using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuseumExhibits.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovePostCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Posts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
