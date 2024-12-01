using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IronBoots.Data.Migrations
{
    /// <inheritdoc />
    public partial class ShipmentStatusFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShipmentStatus",
                table: "Shipments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "The current status of the order");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShipmentStatus",
                table: "Shipments");
        }
    }
}
