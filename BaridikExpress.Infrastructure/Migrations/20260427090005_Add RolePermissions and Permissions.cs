using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaridikExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRolePermissionsandPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_AspNetRoles_RoleId",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_AspNetUsers_UserId",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Permissions_Permission_Id1",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "RolePermission_Id",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Role_Id",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "RolePermissions");

            migrationBuilder.RenameColumn(
                name: "Permission_Name",
                table: "Permissions",
                newName: "PermissionName");

            migrationBuilder.RenameColumn(
                name: "Permission_Id",
                table: "Permissions",
                newName: "PermissionId");

            migrationBuilder.RenameColumn(
                name: "Permission_Id1",
                table: "RolePermissions",
                newName: "PermissionId");

            migrationBuilder.RenameColumn(
                name: "Permission_Id",
                table: "RolePermissions",
                newName: "RolePermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_Roles_UserId",
                table: "RolePermissions",
                newName: "IX_RolePermissions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Roles_RoleId",
                table: "RolePermissions",
                newName: "IX_RolePermissions_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Roles_Permission_Id1",
                table: "RolePermissions",
                newName: "IX_RolePermissions_PermissionId");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "RolePermissions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions",
                column: "RolePermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_AspNetRoles_RoleId",
                table: "RolePermissions",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_AspNetUsers_UserId",
                table: "RolePermissions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "PermissionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_AspNetRoles_RoleId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_AspNetUsers_UserId",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionId",
                table: "RolePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions");

            migrationBuilder.RenameTable(
                name: "RolePermissions",
                newName: "Roles");

            migrationBuilder.RenameColumn(
                name: "PermissionName",
                table: "Permissions",
                newName: "Permission_Name");

            migrationBuilder.RenameColumn(
                name: "PermissionId",
                table: "Permissions",
                newName: "Permission_Id");

            migrationBuilder.RenameColumn(
                name: "PermissionId",
                table: "Roles",
                newName: "Permission_Id1");

            migrationBuilder.RenameColumn(
                name: "RolePermissionId",
                table: "Roles",
                newName: "Permission_Id");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_UserId",
                table: "Roles",
                newName: "IX_Roles_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_RoleId",
                table: "Roles",
                newName: "IX_Roles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "Roles",
                newName: "IX_Roles_Permission_Id1");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "Roles",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "RolePermission_Id",
                table: "Roles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Role_Id",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "RolePermission_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_AspNetRoles_RoleId",
                table: "Roles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_AspNetUsers_UserId",
                table: "Roles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Permissions_Permission_Id1",
                table: "Roles",
                column: "Permission_Id1",
                principalTable: "Permissions",
                principalColumn: "Permission_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
