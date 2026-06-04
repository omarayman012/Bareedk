using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaridikExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyinVechicles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Vehicles");
        }
    }
}
