using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyRentalManagement.Migrations
{
    /// <inheritdoc />
    public partial class Seeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 1,
                columns: new[] { "SentDate", "TenantId" },
                values: new object[] { new DateTime(2024, 10, 23, 15, 42, 14, 771, DateTimeKind.Local).AddTicks(3679), 1 });

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 2,
                column: "SentDate",
                value: new DateTime(2024, 10, 25, 15, 42, 14, 771, DateTimeKind.Local).AddTicks(3738));

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 3,
                columns: new[] { "SentDate", "TenantId" },
                values: new object[] { new DateTime(2024, 10, 26, 15, 42, 14, 771, DateTimeKind.Local).AddTicks(3743), 1 });

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 4,
                column: "SentDate",
                value: new DateTime(2024, 10, 27, 15, 42, 14, 771, DateTimeKind.Local).AddTicks(3748));

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 5,
                columns: new[] { "SentDate", "TenantId" },
                values: new object[] { new DateTime(2024, 10, 28, 15, 42, 14, 771, DateTimeKind.Local).AddTicks(3752), 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 1,
                columns: new[] { "SentDate", "TenantId" },
                values: new object[] { new DateTime(2024, 10, 23, 15, 19, 37, 644, DateTimeKind.Local).AddTicks(9535), null });

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 2,
                column: "SentDate",
                value: new DateTime(2024, 10, 25, 15, 19, 37, 644, DateTimeKind.Local).AddTicks(9597));

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 3,
                columns: new[] { "SentDate", "TenantId" },
                values: new object[] { new DateTime(2024, 10, 26, 15, 19, 37, 644, DateTimeKind.Local).AddTicks(9602), null });

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 4,
                column: "SentDate",
                value: new DateTime(2024, 10, 27, 15, 19, 37, 644, DateTimeKind.Local).AddTicks(9605));

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 5,
                columns: new[] { "SentDate", "TenantId" },
                values: new object[] { new DateTime(2024, 10, 28, 15, 19, 37, 644, DateTimeKind.Local).AddTicks(9608), null });
        }
    }
}
