using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaridikExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateourPlanTableandrelforcustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Plan_PlanId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Plan_AspNetUsers_CreatedById",
                table: "Plan");

            migrationBuilder.DropForeignKey(
                name: "FK_Plan_AspNetUsers_UpdatedById",
                table: "Plan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Plan",
                table: "Plan");

            migrationBuilder.RenameTable(
                name: "Plan",
                newName: "Plans");

            migrationBuilder.RenameIndex(
                name: "IX_Plan_UpdatedById",
                table: "Plans",
                newName: "IX_Plans_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Plan_CreatedById",
                table: "Plans",
                newName: "IX_Plans_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Plans",
                table: "Plans",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Plans_PlanId",
                table: "Customers",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_AspNetUsers_CreatedById",
                table: "Plans",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_AspNetUsers_UpdatedById",
                table: "Plans",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Plans_PlanId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Plans_AspNetUsers_CreatedById",
                table: "Plans");

            migrationBuilder.DropForeignKey(
                name: "FK_Plans_AspNetUsers_UpdatedById",
                table: "Plans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Plans",
                table: "Plans");

            migrationBuilder.RenameTable(
                name: "Plans",
                newName: "Plan");

            migrationBuilder.RenameIndex(
                name: "IX_Plans_UpdatedById",
                table: "Plan",
                newName: "IX_Plan_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Plans_CreatedById",
                table: "Plan",
                newName: "IX_Plan_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Plan",
                table: "Plan",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Plan_PlanId",
                table: "Customers",
                column: "PlanId",
                principalTable: "Plan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Plan_AspNetUsers_CreatedById",
                table: "Plan",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Plan_AspNetUsers_UpdatedById",
                table: "Plan",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
