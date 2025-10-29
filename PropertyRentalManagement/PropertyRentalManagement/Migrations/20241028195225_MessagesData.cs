using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyRentalManagement.Migrations
{
    /// <inheritdoc />
    public partial class MessagesData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 1,
                column: "SentDate",
                value: new DateTime(2024, 10, 23, 15, 52, 25, 17, DateTimeKind.Local).AddTicks(892));

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 2,
                column: "SentDate",
                value: new DateTime(2024, 10, 25, 15, 52, 25, 17, DateTimeKind.Local).AddTicks(1016));

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 3,
                column: "SentDate",
                value: new DateTime(2024, 10, 26, 15, 52, 25, 17, DateTimeKind.Local).AddTicks(1029));

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 4,
                column: "SentDate",
                value: new DateTime(2024, 10, 27, 15, 52, 25, 17, DateTimeKind.Local).AddTicks(1039));

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 5,
                column: "SentDate",
                value: new DateTime(2024, 10, 28, 15, 52, 25, 17, DateTimeKind.Local).AddTicks(1051));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "MessageId",
                keyValue: 1,
                column: "SentDate",
                value: new DateTime(2024, 10, 23, 15, 42, 14, 771, DateTimeKind.Local).AddTicks(3679));

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
                column: "SentDate",
                value: new DateTime(2024, 10, 26, 15, 42, 14, 771, DateTimeKind.Local).AddTicks(3743));

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
                column: "SentDate",
                value: new DateTime(2024, 10, 28, 15, 42, 14, 771, DateTimeKind.Local).AddTicks(3752));
        }
    }
}
