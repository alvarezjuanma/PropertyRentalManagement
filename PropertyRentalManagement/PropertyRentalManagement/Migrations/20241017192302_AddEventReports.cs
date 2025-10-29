using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyRentalManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddEventReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventReports",
                columns: table => new
                {
                    EventReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventReports", x => x.EventReportId);
                    table.ForeignKey(
                        name: "FK_EventReports_PropertyManagers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "PropertyManagers",
                        principalColumn: "ManagerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventReports_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventReports_ManagerId",
                table: "EventReports",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_EventReports_TenantId",
                table: "EventReports",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventReports");
        }
    }
}
