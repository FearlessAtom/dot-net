using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EncryptionClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class isencrypted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "EditingDate",
                table: "RecentFiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 11, 23, 9, 33, 396, DateTimeKind.Local).AddTicks(2801),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 8, 21, 32, 45, 425, DateTimeKind.Local).AddTicks(4684));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "EditingDate",
                table: "RecentFiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 8, 21, 32, 45, 425, DateTimeKind.Local).AddTicks(4684),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 11, 23, 9, 33, 396, DateTimeKind.Local).AddTicks(2801));
        }
    }
}
