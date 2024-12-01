using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IronBoots.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixingShipmentVehicleOrderRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vehicles_ShipmentId",
                table: "Vehicles");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShipmentId",
                table: "Vehicles",
                type: "uniqueidentifier",
                nullable: true,
                comment: "Id of the shipment",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Id of the shipment");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShipmentId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true,
                comment: "Id of the shipment the order belongs to",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Id of the shipment the order belongs to");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ShipmentId",
                table: "Vehicles",
                column: "ShipmentId",
                unique: true,
                filter: "[ShipmentId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vehicles_ShipmentId",
                table: "Vehicles");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShipmentId",
                table: "Vehicles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Id of the shipment",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true,
                oldComment: "Id of the shipment");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShipmentId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Id of the shipment the order belongs to",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true,
                oldComment: "Id of the shipment the order belongs to");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ShipmentId",
                table: "Vehicles",
                column: "ShipmentId",
                unique: true);
        }
    }
}
