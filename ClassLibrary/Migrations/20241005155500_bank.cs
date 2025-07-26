using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassLibrary.Migrations
{
    /// <inheritdoc />
    public partial class bank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutomatedTellerMachines_Banks_BankId",
                table: "AutomatedTellerMachines");

            migrationBuilder.DropIndex(
                name: "IX_AutomatedTellerMachines_BankId",
                table: "AutomatedTellerMachines");

            migrationBuilder.AlterColumn<int>(
                name: "BankId",
                table: "AutomatedTellerMachines",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BankId",
                table: "AutomatedTellerMachines",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_AutomatedTellerMachines_BankId",
                table: "AutomatedTellerMachines",
                column: "BankId");

            migrationBuilder.AddForeignKey(
                name: "FK_AutomatedTellerMachines_Banks_BankId",
                table: "AutomatedTellerMachines",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id");
        }
    }
}
