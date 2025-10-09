using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTO_Manager.Migrations
{
    /// <inheritdoc />
    public partial class NameChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MukaszunetiNap",
                table: "KulonlegesNapok",
                newName: "IsWOrkingDay");

            migrationBuilder.RenameColumn(
                name: "Datum",
                table: "KulonlegesNapok",
                newName: "Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsWOrkingDay",
                table: "KulonlegesNapok",
                newName: "MukaszunetiNap");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "KulonlegesNapok",
                newName: "Datum");
        }
    }
}
