using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaridikExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationToDelivary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "Gov",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "Village",
                table: "Deliveries");

            migrationBuilder.AddColumn<Guid>(
                name: "CityId",
                table: "Deliveries",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "Deliveries",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GovernmentId",
                table: "Deliveries",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VillageId",
                table: "Deliveries",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_CityId",
                table: "Deliveries",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_CountryId",
                table: "Deliveries",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_GovernmentId",
                table: "Deliveries",
                column: "GovernmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_VillageId",
                table: "Deliveries",
                column: "VillageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Cities_CityId",
                table: "Deliveries",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Countries_CountryId",
                table: "Deliveries",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Governments_GovernmentId",
                table: "Deliveries",
                column: "GovernmentId",
                principalTable: "Governments",
                principalColumn: "GovernmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Villages_VillageId",
                table: "Deliveries",
                column: "VillageId",
                principalTable: "Villages",
                principalColumn: "VillageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Cities_CityId",
                table: "Deliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Countries_CountryId",
                table: "Deliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Governments_GovernmentId",
                table: "Deliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Villages_VillageId",
                table: "Deliveries");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_CityId",
                table: "Deliveries");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_CountryId",
                table: "Deliveries");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_GovernmentId",
                table: "Deliveries");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_VillageId",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "GovernmentId",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "VillageId",
                table: "Deliveries");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Deliveries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Deliveries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gov",
                table: "Deliveries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Village",
                table: "Deliveries",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
