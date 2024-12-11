using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IronBoots.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedPlannedAssignedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlannedAssignedDate",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ActualAssignedDate",
                table: "Orders",
                newName: "AssignedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AssignedDate",
                table: "Orders",
                newName: "ActualAssignedDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedAssignedDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "When is the order supposed to be assigned to a shipment");
        }
    }
}
