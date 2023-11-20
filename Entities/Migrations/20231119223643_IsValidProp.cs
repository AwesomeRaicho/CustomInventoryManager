using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class IsValidProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSold",
                table: "Products",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSold",
                table: "Costumes",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSold",
                table: "Clothes",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSold",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsSold",
                table: "Costumes");

            migrationBuilder.DropColumn(
                name: "IsSold",
                table: "Clothes");
        }
    }
}
