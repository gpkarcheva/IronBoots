using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IronBoots.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedForeignKeyVehicleShipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Shipments_ShipmentId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_ShipmentId",
                table: "Vehicles");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_VehicleId",
                table: "Shipments",
                column: "VehicleId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_Vehicles_VehicleId",
                table: "Shipments",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Vehicles_VehicleId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_VehicleId",
                table: "Shipments");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ShipmentId",
                table: "Vehicles",
                column: "ShipmentId",
                unique: true,
                filter: "[ShipmentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Shipments_ShipmentId",
                table: "Vehicles",
                column: "ShipmentId",
                principalTable: "Shipments",
                principalColumn: "Id");
        }
    }
}
