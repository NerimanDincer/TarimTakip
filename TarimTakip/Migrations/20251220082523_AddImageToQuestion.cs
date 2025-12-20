using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarimTakip.Migrations
{
    /// <inheritdoc />
    public partial class AddImageToQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Questions");
        }
    }
}
