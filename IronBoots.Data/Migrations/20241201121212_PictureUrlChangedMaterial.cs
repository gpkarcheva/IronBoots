using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IronBoots.Data.Migrations
{
    /// <inheritdoc />
    public partial class PictureUrlChangedMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Materials",
                newName: "PictureUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PictureUrl",
                table: "Materials",
                newName: "ImageUrl");
        }
    }
}
