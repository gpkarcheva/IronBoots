using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IronBoots.Data.Migrations
{
    /// <inheritdoc />
    public partial class DateTimeActuallyNullableThisTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ShipmentDate",
                table: "Shipments",
                type: "datetime2",
                nullable: true,
                comment: "The date the shipment started",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "The date the shipment started");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeliveryDate",
                table: "Shipments",
                type: "datetime2",
                nullable: true,
                comment: "The date the shipment was completed",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "The date the shipment was completed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ShipmentDate",
                table: "Shipments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "The date the shipment started",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "The date the shipment started");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeliveryDate",
                table: "Shipments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "The date the shipment was completed",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "The date the shipment was completed");
        }
    }
}
