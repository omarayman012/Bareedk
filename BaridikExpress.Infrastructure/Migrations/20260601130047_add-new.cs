using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaridikExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addnew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShipmentAttachment_AspNetUsers_CreatedById",
                table: "ShipmentAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_ShipmentAttachment_AspNetUsers_UpdatedById",
                table: "ShipmentAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_ShipmentAttachment_Shipments_ShipmentId",
                table: "ShipmentAttachment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShipmentAttachment",
                table: "ShipmentAttachment");

            migrationBuilder.RenameTable(
                name: "ShipmentAttachment",
                newName: "ShipmentAttachments");

            migrationBuilder.RenameIndex(
                name: "IX_ShipmentAttachment_UpdatedById",
                table: "ShipmentAttachments",
                newName: "IX_ShipmentAttachments_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_ShipmentAttachment_ShipmentId",
                table: "ShipmentAttachments",
                newName: "IX_ShipmentAttachments_ShipmentId");

            migrationBuilder.RenameIndex(
                name: "IX_ShipmentAttachment_CreatedById",
                table: "ShipmentAttachments",
                newName: "IX_ShipmentAttachments_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShipmentAttachments",
                table: "ShipmentAttachments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShipmentAttachments_AspNetUsers_CreatedById",
                table: "ShipmentAttachments",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShipmentAttachments_AspNetUsers_UpdatedById",
                table: "ShipmentAttachments",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShipmentAttachments_Shipments_ShipmentId",
                table: "ShipmentAttachments",
                column: "ShipmentId",
                principalTable: "Shipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShipmentAttachments_AspNetUsers_CreatedById",
                table: "ShipmentAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_ShipmentAttachments_AspNetUsers_UpdatedById",
                table: "ShipmentAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_ShipmentAttachments_Shipments_ShipmentId",
                table: "ShipmentAttachments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShipmentAttachments",
                table: "ShipmentAttachments");

            migrationBuilder.RenameTable(
                name: "ShipmentAttachments",
                newName: "ShipmentAttachment");

            migrationBuilder.RenameIndex(
                name: "IX_ShipmentAttachments_UpdatedById",
                table: "ShipmentAttachment",
                newName: "IX_ShipmentAttachment_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_ShipmentAttachments_ShipmentId",
                table: "ShipmentAttachment",
                newName: "IX_ShipmentAttachment_ShipmentId");

            migrationBuilder.RenameIndex(
                name: "IX_ShipmentAttachments_CreatedById",
                table: "ShipmentAttachment",
                newName: "IX_ShipmentAttachment_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShipmentAttachment",
                table: "ShipmentAttachment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShipmentAttachment_AspNetUsers_CreatedById",
                table: "ShipmentAttachment",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShipmentAttachment_AspNetUsers_UpdatedById",
                table: "ShipmentAttachment",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShipmentAttachment_Shipments_ShipmentId",
                table: "ShipmentAttachment",
                column: "ShipmentId",
                principalTable: "Shipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
