using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IronBoots.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixFKsClientProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientsProducts_Clients_ProductId",
                table: "ClientsProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientsProducts_Products_ClientId",
                table: "ClientsProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientsProducts_Clients_ClientId",
                table: "ClientsProducts",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientsProducts_Products_ProductId",
                table: "ClientsProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientsProducts_Clients_ClientId",
                table: "ClientsProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientsProducts_Products_ProductId",
                table: "ClientsProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientsProducts_Clients_ProductId",
                table: "ClientsProducts",
                column: "ProductId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientsProducts_Products_ClientId",
                table: "ClientsProducts",
                column: "ClientId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
