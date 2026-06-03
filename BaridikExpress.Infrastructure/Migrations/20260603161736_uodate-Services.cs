using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaridikExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class uodateServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Services",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Services");
        }
    }
}
