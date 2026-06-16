using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaridikExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addTalkService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TalkServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceBusinessPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShipmentVolumeRange = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GovernmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VillageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WorkEmail = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CompanyAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    AdditionalInformation = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TalkServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TalkServices_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TalkServices_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TalkServices_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId");
                    table.ForeignKey(
                        name: "FK_TalkServices_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId");
                    table.ForeignKey(
                        name: "FK_TalkServices_Governments_GovernmentId",
                        column: x => x.GovernmentId,
                        principalTable: "Governments",
                        principalColumn: "GovernmentId");
                    table.ForeignKey(
                        name: "FK_TalkServices_ServiceBusinessPlans_ServiceBusinessPlanId",
                        column: x => x.ServiceBusinessPlanId,
                        principalTable: "ServiceBusinessPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TalkServices_Villages_VillageId",
                        column: x => x.VillageId,
                        principalTable: "Villages",
                        principalColumn: "VillageId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TalkServices_CityId",
                table: "TalkServices",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_TalkServices_CountryId",
                table: "TalkServices",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_TalkServices_CreatedById",
                table: "TalkServices",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TalkServices_GovernmentId",
                table: "TalkServices",
                column: "GovernmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TalkServices_ServiceBusinessPlanId",
                table: "TalkServices",
                column: "ServiceBusinessPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TalkServices_Status",
                table: "TalkServices",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TalkServices_UpdatedById",
                table: "TalkServices",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TalkServices_VillageId",
                table: "TalkServices",
                column: "VillageId");

            migrationBuilder.CreateIndex(
                name: "IX_TalkServices_WorkEmail",
                table: "TalkServices",
                column: "WorkEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TalkServices");
        }
    }
}
