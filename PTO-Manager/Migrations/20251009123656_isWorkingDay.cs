using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTO_Manager.Migrations
{
    /// <inheritdoc />
    public partial class isWorkingDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsWOrkingDay",
                table: "SpecialDays",
                newName: "IsWorkingDay");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsWorkingDay",
                table: "SpecialDays",
                newName: "IsWOrkingDay");
        }
    }
}
