using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyRentalManagement.Migrations
{
    /// <inheritdoc />
    public partial class Mensajes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 1,
                columns: new[] { "ManagerId", "SentDate" },
                values: new object[] { 4, new DateTime(2024, 10, 24, 11, 1, 40, 917, DateTimeKind.Local).AddTicks(7470) });

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 2,
                columns: new[] { "ManagerId", "SentDate" },
                values: new object[] { 4, new DateTime(2024, 10, 26, 11, 1, 40, 917, DateTimeKind.Local).AddTicks(7529) });

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 3,
                columns: new[] { "ManagerId", "SentDate" },
                values: new object[] { 4, new DateTime(2024, 10, 27, 11, 1, 40, 917, DateTimeKind.Local).AddTicks(7534) });

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 4,
                columns: new[] { "ManagerId", "Sender", "SentDate", "TenantId" },
                values: new object[] { 4, "tenant1@example.com", new DateTime(2024, 10, 28, 11, 1, 40, 917, DateTimeKind.Local).AddTicks(7538), 1 });

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 5,
                columns: new[] { "ManagerId", "SentDate" },
                values: new object[] { 5, new DateTime(2024, 10, 29, 11, 1, 40, 917, DateTimeKind.Local).AddTicks(7542) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 1,
                columns: new[] { "ManagerId", "SentDate" },
                values: new object[] { 1, new DateTime(2024, 10, 23, 15, 52, 25, 17, DateTimeKind.Local).AddTicks(892) });

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 2,
                columns: new[] { "ManagerId", "SentDate" },
                values: new object[] { 1, new DateTime(2024, 10, 25, 15, 52, 25, 17, DateTimeKind.Local).AddTicks(1016) });

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 3,
                columns: new[] { "ManagerId", "SentDate" },
                values: new object[] { 1, new DateTime(2024, 10, 26, 15, 52, 25, 17, DateTimeKind.Local).AddTicks(1029) });

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 4,
                columns: new[] { "ManagerId", "Sender", "SentDate", "TenantId" },
                values: new object[] { 1, "tenant2@example.com", new DateTime(2024, 10, 27, 15, 52, 25, 17, DateTimeKind.Local).AddTicks(1039), 2 });

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 5,
                columns: new[] { "ManagerId", "SentDate" },
                values: new object[] { 2, new DateTime(2024, 10, 28, 15, 52, 25, 17, DateTimeKind.Local).AddTicks(1051) });
        }
    }
}
