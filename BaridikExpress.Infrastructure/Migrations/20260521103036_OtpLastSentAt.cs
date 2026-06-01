using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaridikExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OtpLastSentAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EmailOtpFirstSentAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailOtpLastSentAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmailOtpSendCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PhoneOtpLastSentAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailOtpFirstSentAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmailOtpLastSentAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmailOtpSendCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhoneOtpLastSentAt",
                table: "AspNetUsers");
        }
    }
}
