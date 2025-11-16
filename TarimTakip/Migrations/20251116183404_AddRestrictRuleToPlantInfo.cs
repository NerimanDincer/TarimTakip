using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarimTakip.Migrations
{
    /// <inheritdoc />
    public partial class AddRestrictRuleToPlantInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlantRegions_PlantInfos_PlantInfoId",
                table: "PlantRegions");

            migrationBuilder.DropIndex(
                name: "IX_PlantRegions_PlantInfoId",
                table: "PlantRegions");

            migrationBuilder.DropColumn(
                name: "PlantInfoId",
                table: "PlantRegions");

            migrationBuilder.CreateIndex(
                name: "IX_PlantRegions_PlantId",
                table: "PlantRegions",
                column: "PlantId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantRegions_PlantInfos_PlantId",
                table: "PlantRegions",
                column: "PlantId",
                principalTable: "PlantInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlantRegions_PlantInfos_PlantId",
                table: "PlantRegions");

            migrationBuilder.DropIndex(
                name: "IX_PlantRegions_PlantId",
                table: "PlantRegions");

            migrationBuilder.AddColumn<int>(
                name: "PlantInfoId",
                table: "PlantRegions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlantRegions_PlantInfoId",
                table: "PlantRegions",
                column: "PlantInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantRegions_PlantInfos_PlantInfoId",
                table: "PlantRegions",
                column: "PlantInfoId",
                principalTable: "PlantInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
