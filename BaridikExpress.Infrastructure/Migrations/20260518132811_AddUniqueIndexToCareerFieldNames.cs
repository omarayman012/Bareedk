using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaridikExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToCareerFieldNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CareerFields_NameAr",
                table: "CareerFields");

            migrationBuilder.DropIndex(
                name: "IX_CareerFields_NameEn",
                table: "CareerFields");

            migrationBuilder.CreateIndex(
                name: "IX_CareerFields_NameAr",
                table: "CareerFields",
                column: "NameAr",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CareerFields_NameEn",
                table: "CareerFields",
                column: "NameEn",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CareerFields_NameAr",
                table: "CareerFields");

            migrationBuilder.DropIndex(
                name: "IX_CareerFields_NameEn",
                table: "CareerFields");

            migrationBuilder.CreateIndex(
                name: "IX_CareerFields_NameAr",
                table: "CareerFields",
                column: "NameAr");

            migrationBuilder.CreateIndex(
                name: "IX_CareerFields_NameEn",
                table: "CareerFields",
                column: "NameEn");
        }
    }
}
