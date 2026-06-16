using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaridikExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCoulmsAtPublishingHouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "CreatedDate",
                table: "Blogs",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "CreatedTime",
                table: "Blogs",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<Guid>(
                name: "PublishingHouseId",
                table: "Blogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_PublishingHouseId",
                table: "Blogs",
                column: "PublishingHouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_PublishingHouses_PublishingHouseId",
                table: "Blogs",
                column: "PublishingHouseId",
                principalTable: "PublishingHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_PublishingHouses_PublishingHouseId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_PublishingHouseId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "PublishingHouseId",
                table: "Blogs");
        }
    }
}
