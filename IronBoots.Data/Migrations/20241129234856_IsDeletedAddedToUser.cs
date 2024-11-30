using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IronBoots.Data.Migrations
{
    /// <inheritdoc />
    public partial class IsDeletedAddedToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PlannedAssignedDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                comment: "When is the order supposed to be assigned to a shipment",
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldComment: "When is the order supposed to be assigned to a shipment");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActualAssignedDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                comment: "When is the order actually assigned to a shipment",
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldComment: "When is the order actually assigned to a shipment");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "PlannedAssignedDate",
                table: "Orders",
                type: "date",
                nullable: false,
                comment: "When is the order supposed to be assigned to a shipment",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "When is the order supposed to be assigned to a shipment");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ActualAssignedDate",
                table: "Orders",
                type: "date",
                nullable: false,
                comment: "When is the order actually assigned to a shipment",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "When is the order actually assigned to a shipment");
        }
    }
}
