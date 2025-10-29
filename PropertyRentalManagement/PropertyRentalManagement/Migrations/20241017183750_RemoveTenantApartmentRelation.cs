using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PropertyRentalManagement.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTenantApartmentRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "PropertyOwners",
                columns: table => new
                {
                    OwnerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyOwners", x => x.OwnerId);
                    table.ForeignKey(
                        name: "FK_PropertyOwners_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyManagers",
                columns: table => new
                {
                    ManagerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyManagers", x => x.ManagerId);
                    table.ForeignKey(
                        name: "FK_PropertyManagers_PropertyOwners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "PropertyOwners",
                        principalColumn: "OwnerId");
                    table.ForeignKey(
                        name: "FK_PropertyManagers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    BuildingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.BuildingId);
                    table.ForeignKey(
                        name: "FK_Buildings_PropertyManagers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "PropertyManagers",
                        principalColumn: "ManagerId");
                });

            migrationBuilder.CreateTable(
                name: "Apartments",
                columns: table => new
                {
                    ApartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ApartmentNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuildingId = table.Column<int>(type: "int", nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartments", x => x.ApartmentId);
                    table.ForeignKey(
                        name: "FK_Apartments_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "BuildingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Apartments_PropertyManagers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "PropertyManagers",
                        principalColumn: "ManagerId");
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    TenantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: false),
                    ApartmentsApartmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.TenantId);
                    table.ForeignKey(
                        name: "FK_Tenants_Apartments_ApartmentsApartmentId",
                        column: x => x.ApartmentsApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "ApartmentId");
                    table.ForeignKey(
                        name: "FK_Tenants_PropertyManagers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "PropertyManagers",
                        principalColumn: "ManagerId");
                    table.ForeignKey(
                        name: "FK_Tenants_PropertyOwners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "PropertyOwners",
                        principalColumn: "OwnerId");
                    table.ForeignKey(
                        name: "FK_Tenants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    AppointmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: false),
                    ApartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_Appointments_Apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "ApartmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_PropertyManagers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "PropertyManagers",
                        principalColumn: "ManagerId");
                    table.ForeignKey(
                        name: "FK_Appointments_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "Password", "Phone", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "owner1@example.com", "pass123", "555-555-5551", 0, "owner1" },
                    { 2, "manager1@example.com", "pass123", "555-555-5552", 1, "manager1" },
                    { 3, "tenant1@example.com", "pass123", "555-555-5553", 2, "tenant1" }
                });

            migrationBuilder.InsertData(
                table: "PropertyOwners",
                columns: new[] { "OwnerId", "Email", "Name", "Phone", "UserId" },
                values: new object[] { 1, "owner1@example.com", "John Doe", "555-555-5551", 1 });

            migrationBuilder.InsertData(
                table: "PropertyManagers",
                columns: new[] { "ManagerId", "Email", "Name", "OwnerId", "Phone", "UserId" },
                values: new object[] { 1, "manager1@example.com", "Jane Smith", 1, "555-555-5552", 2 });

            migrationBuilder.InsertData(
                table: "Buildings",
                columns: new[] { "BuildingId", "Address", "ManagerId", "Name" },
                values: new object[,]
                {
                    { 1, "123 Main St", 1, "Sunset Apartments" },
                    { 2, "456 River Ave", 1, "Riverfront Plaza" },
                    { 3, "789 Valley Rd", 1, "Green Valley Towers" }
                });

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "TenantId", "ApartmentsApartmentId", "Email", "ManagerId", "Name", "OwnerId", "Phone", "UserId" },
                values: new object[] { 1, null, "tenant1@example.com", 1, "Michael Brown", 1, "555-555-5553", 3 });

            migrationBuilder.InsertData(
                table: "Apartments",
                columns: new[] { "ApartmentId", "ApartmentNumber", "BuildingId", "ManagerId", "Status" },
                values: new object[,]
                {
                    { 1, "101", 1, 1, 1 },
                    { 2, "102", 1, 1, 0 },
                    { 3, "201", 2, 1, 1 },
                    { 4, "202", 2, 1, 2 },
                    { 5, "301", 3, 1, 1 },
                    { 6, "302", 3, 1, 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_BuildingId",
                table: "Apartments",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_ManagerId",
                table: "Apartments",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ApartmentId",
                table: "Appointments",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ManagerId",
                table: "Appointments",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_TenantId",
                table: "Appointments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_ManagerId",
                table: "Buildings",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyManagers_OwnerId",
                table: "PropertyManagers",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyManagers_UserId",
                table: "PropertyManagers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyOwners_UserId",
                table: "PropertyOwners",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_ApartmentsApartmentId",
                table: "Tenants",
                column: "ApartmentsApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_ManagerId",
                table: "Tenants",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_OwnerId",
                table: "Tenants",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_UserId",
                table: "Tenants",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "Apartments");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "PropertyManagers");

            migrationBuilder.DropTable(
                name: "PropertyOwners");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
