using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaridikExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeliveryType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "DeliveryTypes");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "DeliveryTypes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "DeliveryTypes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "DeliveryTypes");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "DeliveryTypes");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DeliveryTypes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
