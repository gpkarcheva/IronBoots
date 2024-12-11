using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IronBoots.Data.Migrations
{
    /// <inheritdoc />
    public partial class ClientProductToContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientProduct_Clients_ProductId",
                table: "ClientProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientProduct_Products_ClientId",
                table: "ClientProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientProduct",
                table: "ClientProduct");

            migrationBuilder.RenameTable(
                name: "ClientProduct",
                newName: "ClientsProducts");

            migrationBuilder.RenameIndex(
                name: "IX_ClientProduct_ProductId",
                table: "ClientsProducts",
                newName: "IX_ClientsProducts_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientsProducts",
                table: "ClientsProducts",
                columns: new[] { "ClientId", "ProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ClientsProducts_Clients_ProductId",
                table: "ClientsProducts",
                column: "ProductId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientsProducts_Products_ClientId",
                table: "ClientsProducts",
                column: "ClientId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientsProducts_Clients_ProductId",
                table: "ClientsProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientsProducts_Products_ClientId",
                table: "ClientsProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientsProducts",
                table: "ClientsProducts");

            migrationBuilder.RenameTable(
                name: "ClientsProducts",
                newName: "ClientProduct");

            migrationBuilder.RenameIndex(
                name: "IX_ClientsProducts_ProductId",
                table: "ClientProduct",
                newName: "IX_ClientProduct_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientProduct",
                table: "ClientProduct",
                columns: new[] { "ClientId", "ProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ClientProduct_Clients_ProductId",
                table: "ClientProduct",
                column: "ProductId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientProduct_Products_ClientId",
                table: "ClientProduct",
                column: "ClientId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
