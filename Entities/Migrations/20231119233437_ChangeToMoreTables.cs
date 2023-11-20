using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToMoreTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSold",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsSold",
                table: "Clothes");

            migrationBuilder.CreateTable(
                name: "SoldClothes",
                columns: table => new
                {
                    SoldClothesID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Theme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClothesType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchasePrice = table.Column<double>(type: "float", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExitDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldClothes", x => x.SoldClothesID);
                });

            migrationBuilder.CreateTable(
                name: "SoldCostumes",
                columns: table => new
                {
                    SoldCostumeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CostumeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchasePrice = table.Column<double>(type: "float", nullable: true),
                    IsSold = table.Column<bool>(type: "bit", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExitDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldCostumes", x => x.SoldCostumeID);
                });

            migrationBuilder.CreateTable(
                name: "SoldProducts",
                columns: table => new
                {
                    SoldProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Theme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchasePrice = table.Column<double>(type: "float", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExitDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldProducts", x => x.SoldProductID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoldClothes");

            migrationBuilder.DropTable(
                name: "SoldCostumes");

            migrationBuilder.DropTable(
                name: "SoldProducts");

            migrationBuilder.AddColumn<bool>(
                name: "IsSold",
                table: "Products",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSold",
                table: "Clothes",
                type: "bit",
                nullable: true);
        }
    }
}
