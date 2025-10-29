using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyRentalManagement.Migrations
{
    /// <inheritdoc />
    public partial class Apartments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfBathrooms",
                table: "Apartments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfBedrooms",
                table: "Apartments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "PetsAllowed",
                table: "Apartments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RentAmount",
                table: "Apartments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Apartments",
                keyColumn: "ApartmentId",
                keyValue: 1,
                columns: new[] { "NumberOfBathrooms", "NumberOfBedrooms", "PetsAllowed", "RentAmount" },
                values: new object[] { 1, 2, true, 1500 });

            migrationBuilder.UpdateData(
                table: "Apartments",
                keyColumn: "ApartmentId",
                keyValue: 2,
                columns: new[] { "NumberOfBathrooms", "NumberOfBedrooms", "PetsAllowed", "RentAmount" },
                values: new object[] { 2, 3, false, 2000 });

            migrationBuilder.UpdateData(
                table: "Apartments",
                keyColumn: "ApartmentId",
                keyValue: 3,
                columns: new[] { "NumberOfBathrooms", "NumberOfBedrooms", "PetsAllowed", "RentAmount" },
                values: new object[] { 1, 1, true, 1200 });

            migrationBuilder.UpdateData(
                table: "Apartments",
                keyColumn: "ApartmentId",
                keyValue: 4,
                columns: new[] { "NumberOfBathrooms", "NumberOfBedrooms", "PetsAllowed", "RentAmount" },
                values: new object[] { 2, 2, false, 1800 });

            migrationBuilder.UpdateData(
                table: "Apartments",
                keyColumn: "ApartmentId",
                keyValue: 5,
                columns: new[] { "NumberOfBathrooms", "NumberOfBedrooms", "PetsAllowed", "RentAmount" },
                values: new object[] { 3, 4, true, 2500 });

            migrationBuilder.UpdateData(
                table: "Apartments",
                keyColumn: "ApartmentId",
                keyValue: 6,
                columns: new[] { "NumberOfBathrooms", "NumberOfBedrooms", "PetsAllowed", "RentAmount" },
                values: new object[] { 1, 2, false, 1600 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfBathrooms",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "NumberOfBedrooms",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "PetsAllowed",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "RentAmount",
                table: "Apartments");
        }
    }
}
