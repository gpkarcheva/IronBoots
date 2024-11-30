using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IronBoots.Data.Migrations
{
    /// <inheritdoc />
    public partial class Help : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Addresses_AddressId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_AspNetUsers_UserId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Addresses_AddressId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_AddressId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AddressesTowns",
                table: "AddressesTowns");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "TownId",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "Clients",
                newName: "AddressTownId");

            migrationBuilder.RenameIndex(
                name: "IX_Clients_AddressId",
                table: "Clients",
                newName: "IX_Clients_AddressTownId");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Clients",
                type: "uniqueidentifier",
                nullable: false,
                comment: "Id of the current user",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AddressesTowns",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "AddressesTowns",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Id of the client that has this combination");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AddressesTowns",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AddressesTowns",
                table: "AddressesTowns",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AddressesTowns_AddressId_TownId",
                table: "AddressesTowns",
                columns: new[] { "AddressId", "TownId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_AddressesTowns_AddressTownId",
                table: "Clients",
                column: "AddressTownId",
                principalTable: "AddressesTowns",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_AspNetUsers_UserId",
                table: "Clients",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_AddressesTowns_AddressTownId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_AspNetUsers_UserId",
                table: "Clients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AddressesTowns",
                table: "AddressesTowns");

            migrationBuilder.DropIndex(
                name: "IX_AddressesTowns_AddressId_TownId",
                table: "AddressesTowns");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AddressesTowns");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "AddressesTowns");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AddressesTowns");

            migrationBuilder.RenameColumn(
                name: "AddressTownId",
                table: "Clients",
                newName: "AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Clients_AddressTownId",
                table: "Clients",
                newName: "IX_Clients_AddressId");

            migrationBuilder.AddColumn<Guid>(
                name: "AddressId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Id of the address");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Clients",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "Id of the current user");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "Addresses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Reference to the client the address belongs to");

            migrationBuilder.AddColumn<Guid>(
                name: "TownId",
                table: "Addresses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Town Id for easy tracking of orders/shipments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AddressesTowns",
                table: "AddressesTowns",
                columns: new[] { "AddressId", "TownId" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AddressId",
                table: "Orders",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Addresses_AddressId",
                table: "Clients",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_AspNetUsers_UserId",
                table: "Clients",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Addresses_AddressId",
                table: "Orders",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }
    }
}
