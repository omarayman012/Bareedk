using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaridikExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PublishingHouses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PublishingHouses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebsiteLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GovernmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VillageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuildingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FloorNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistinctiveMark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublishingHouses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublishingHouses_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PublishingHouses_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PublishingHouses_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId");
                    table.ForeignKey(
                        name: "FK_PublishingHouses_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PublishingHouses_Governments_GovernmentId",
                        column: x => x.GovernmentId,
                        principalTable: "Governments",
                        principalColumn: "GovernmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PublishingHouses_Villages_VillageId",
                        column: x => x.VillageId,
                        principalTable: "Villages",
                        principalColumn: "VillageId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PublishingHouses_CityId",
                table: "PublishingHouses",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_PublishingHouses_CountryId",
                table: "PublishingHouses",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PublishingHouses_CreatedById",
                table: "PublishingHouses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PublishingHouses_GovernmentId",
                table: "PublishingHouses",
                column: "GovernmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PublishingHouses_UpdatedById",
                table: "PublishingHouses",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PublishingHouses_VillageId",
                table: "PublishingHouses",
                column: "VillageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublishingHouses");
        }
    }
}
