using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class RemovedIsSold : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSold",
                table: "SoldCostumes");

            migrationBuilder.DropColumn(
                name: "IsSold",
                table: "Costumes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSold",
                table: "SoldCostumes",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSold",
                table: "Costumes",
                type: "bit",
                nullable: true);
        }
    }
}
