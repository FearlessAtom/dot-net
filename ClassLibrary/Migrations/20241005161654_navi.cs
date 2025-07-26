using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class navi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AutomatedTellerMachines",
                table: "AutomatedTellerMachines");

            migrationBuilder.RenameTable(
                name: "AutomatedTellerMachines",
                newName: "ATMs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ATMs",
                table: "ATMs",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ATMs",
                table: "ATMs");

            migrationBuilder.RenameTable(
                name: "ATMs",
                newName: "AutomatedTellerMachines");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AutomatedTellerMachines",
                table: "AutomatedTellerMachines",
                column: "Id");
        }
    }
}
