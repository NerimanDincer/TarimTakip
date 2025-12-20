using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarimTakip.Migrations
{
    /// <inheritdoc />
    public partial class AddCoordinatesToRegion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Regions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Regions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Regions");
        }
    }
}
