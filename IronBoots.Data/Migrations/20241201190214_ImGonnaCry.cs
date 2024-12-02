using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IronBoots.Data.Migrations
{
    /// <inheritdoc />
    public partial class ImGonnaCry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_AddressesTowns_AddressTownId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_AddressTownId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "AddressTownId",
                table: "Clients");

            migrationBuilder.CreateIndex(
                name: "IX_AddressesTowns_ClientId",
                table: "AddressesTowns",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddressesTowns_Clients_ClientId",
                table: "AddressesTowns",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddressesTowns_Clients_ClientId",
                table: "AddressesTowns");

            migrationBuilder.DropIndex(
                name: "IX_AddressesTowns_ClientId",
                table: "AddressesTowns");

            migrationBuilder.AddColumn<Guid>(
                name: "AddressTownId",
                table: "Clients",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "ID of the address");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_AddressTownId",
                table: "Clients",
                column: "AddressTownId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_AddressesTowns_AddressTownId",
                table: "Clients",
                column: "AddressTownId",
                principalTable: "AddressesTowns",
                principalColumn: "Id");
        }
    }
}
