using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyRentalManagement.Migrations
{
    /// <inheritdoc />
    public partial class TenantUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tenants_PropertyManagers_ManagerId",
                table: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_ManagerId",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Tenants");

            migrationBuilder.AddColumn<int>(
                name: "PropertyManagerManagerId",
                table: "Tenants",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "TenantId",
                keyValue: 1,
                column: "PropertyManagerManagerId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_PropertyManagerManagerId",
                table: "Tenants",
                column: "PropertyManagerManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_PropertyManagers_PropertyManagerManagerId",
                table: "Tenants",
                column: "PropertyManagerManagerId",
                principalTable: "PropertyManagers",
                principalColumn: "ManagerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tenants_PropertyManagers_PropertyManagerManagerId",
                table: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_PropertyManagerManagerId",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "PropertyManagerManagerId",
                table: "Tenants");

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "Tenants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "TenantId",
                keyValue: 1,
                column: "ManagerId",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_ManagerId",
                table: "Tenants",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_PropertyManagers_ManagerId",
                table: "Tenants",
                column: "ManagerId",
                principalTable: "PropertyManagers",
                principalColumn: "ManagerId");
        }
    }
}
