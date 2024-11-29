using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IronBoots.Data.Migrations
{
    /// <inheritdoc />
    public partial class ValidationUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "WeightCapacity",
                table: "Vehicles",
                type: "float",
                nullable: false,
                comment: "Max weight the vehicle can carry in kg",
                oldClrType: typeof(double),
                oldType: "float",
                oldComment: "Max weight the vehicle can carry");

            migrationBuilder.AlterColumn<double>(
                name: "SizeCapacity",
                table: "Vehicles",
                type: "float",
                nullable: false,
                comment: "Max size the vehicle can carry in cm2",
                oldClrType: typeof(double),
                oldType: "float",
                oldComment: "Max size the vehicle can carry");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Vehicles",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<double>(
                name: "Weight",
                table: "Products",
                type: "float",
                nullable: false,
                comment: "Net weight of the product in kg",
                oldClrType: typeof(double),
                oldType: "float",
                oldComment: "Net weight of the product");

            migrationBuilder.AlterColumn<double>(
                name: "Size",
                table: "Products",
                type: "float",
                nullable: false,
                comment: "Net size of the product in cm2",
                oldClrType: typeof(double),
                oldType: "float",
                oldComment: "Net size of the product");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Last name of the user",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Last name of the user");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "First name of the user",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "First name of the user");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Vehicles");

            migrationBuilder.AlterColumn<double>(
                name: "WeightCapacity",
                table: "Vehicles",
                type: "float",
                nullable: false,
                comment: "Max weight the vehicle can carry",
                oldClrType: typeof(double),
                oldType: "float",
                oldComment: "Max weight the vehicle can carry in kg");

            migrationBuilder.AlterColumn<double>(
                name: "SizeCapacity",
                table: "Vehicles",
                type: "float",
                nullable: false,
                comment: "Max size the vehicle can carry",
                oldClrType: typeof(double),
                oldType: "float",
                oldComment: "Max size the vehicle can carry in cm2");

            migrationBuilder.AlterColumn<double>(
                name: "Weight",
                table: "Products",
                type: "float",
                nullable: false,
                comment: "Net weight of the product",
                oldClrType: typeof(double),
                oldType: "float",
                oldComment: "Net weight of the product in kg");

            migrationBuilder.AlterColumn<double>(
                name: "Size",
                table: "Products",
                type: "float",
                nullable: false,
                comment: "Net size of the product",
                oldClrType: typeof(double),
                oldType: "float",
                oldComment: "Net size of the product in cm2");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Last name of the user",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Last name of the user");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                comment: "First name of the user",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "First name of the user");
        }
    }
}
