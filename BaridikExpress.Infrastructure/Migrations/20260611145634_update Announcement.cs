using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaridikExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateAnnouncement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcement_AspNetUsers_CreatedById",
                table: "Announcement");

            migrationBuilder.DropForeignKey(
                name: "FK_Announcement_AspNetUsers_UpdatedById",
                table: "Announcement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Announcement",
                table: "Announcement");

            migrationBuilder.RenameTable(
                name: "Announcement",
                newName: "Announcements");

            migrationBuilder.RenameIndex(
                name: "IX_Announcement_UpdatedById",
                table: "Announcements",
                newName: "IX_Announcements_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Announcement_CreatedById",
                table: "Announcements",
                newName: "IX_Announcements_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Announcements",
                table: "Announcements",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_AspNetUsers_CreatedById",
                table: "Announcements",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_AspNetUsers_UpdatedById",
                table: "Announcements",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_AspNetUsers_CreatedById",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_AspNetUsers_UpdatedById",
                table: "Announcements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Announcements",
                table: "Announcements");

            migrationBuilder.RenameTable(
                name: "Announcements",
                newName: "Announcement");

            migrationBuilder.RenameIndex(
                name: "IX_Announcements_UpdatedById",
                table: "Announcement",
                newName: "IX_Announcement_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Announcements_CreatedById",
                table: "Announcement",
                newName: "IX_Announcement_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Announcement",
                table: "Announcement",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcement_AspNetUsers_CreatedById",
                table: "Announcement",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcement_AspNetUsers_UpdatedById",
                table: "Announcement",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
