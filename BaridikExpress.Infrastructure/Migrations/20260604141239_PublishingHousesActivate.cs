using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaridikExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PublishingHousesActivate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "PublishingHouses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "PublishingHouses");
        }
    }
}
