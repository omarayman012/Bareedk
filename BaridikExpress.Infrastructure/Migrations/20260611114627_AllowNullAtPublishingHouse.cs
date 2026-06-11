using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaridikExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AllowNullAtPublishingHouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_PublishingHouses_PublishingHouseId",
                table: "Blogs");

            migrationBuilder.AlterColumn<Guid>(
                name: "PublishingHouseId",
                table: "Blogs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "CreatedTime",
                table: "Blogs",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreatedDate",
                table: "Blogs",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_PublishingHouses_PublishingHouseId",
                table: "Blogs",
                column: "PublishingHouseId",
                principalTable: "PublishingHouses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_PublishingHouses_PublishingHouseId",
                table: "Blogs");

            migrationBuilder.AlterColumn<Guid>(
                name: "PublishingHouseId",
                table: "Blogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "CreatedTime",
                table: "Blogs",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0),
                oldClrType: typeof(TimeOnly),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreatedDate",
                table: "Blogs",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_PublishingHouses_PublishingHouseId",
                table: "Blogs",
                column: "PublishingHouseId",
                principalTable: "PublishingHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
