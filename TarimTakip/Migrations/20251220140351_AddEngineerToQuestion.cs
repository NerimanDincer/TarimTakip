using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarimTakip.Migrations
{
    /// <inheritdoc />
    public partial class AddEngineerToQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EngineerId",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_EngineerId",
                table: "Questions",
                column: "EngineerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Users_EngineerId",
                table: "Questions",
                column: "EngineerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Users_EngineerId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_EngineerId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "EngineerId",
                table: "Questions");
        }
    }
}
