using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IronBoots.Data.Migrations
{
    /// <inheritdoc />
    public partial class DateTimeOrderNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ActualAssignedDate",
                table: "Orders",
                type: "datetime2",
                nullable: true,
                comment: "When is the order actually assigned to a shipment",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "When is the order actually assigned to a shipment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ActualAssignedDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "When is the order actually assigned to a shipment",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "When is the order actually assigned to a shipment");
        }
    }
}
