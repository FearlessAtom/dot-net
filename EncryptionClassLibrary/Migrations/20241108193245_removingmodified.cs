using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EncryptionClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class removingmodified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Modified",
                table: "RecentFiles");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EditingDate",
                table: "RecentFiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 8, 21, 32, 45, 425, DateTimeKind.Local).AddTicks(4684),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 8, 20, 41, 28, 876, DateTimeKind.Local).AddTicks(5582));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "EditingDate",
                table: "RecentFiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 8, 20, 41, 28, 876, DateTimeKind.Local).AddTicks(5582),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 11, 8, 21, 32, 45, 425, DateTimeKind.Local).AddTicks(4684));

            migrationBuilder.AddColumn<bool>(
                name: "Modified",
                table: "RecentFiles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
