using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTO_Manager.Migrations
{
    /// <inheritdoc />
    public partial class NameChanges3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_SzemelyId",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "SzemelyId",
                table: "Requests",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Datum",
                table: "Requests",
                newName: "Date");

            migrationBuilder.RenameIndex(
                name: "IX_Requests_SzemelyId",
                table: "Requests",
                newName: "IX_Requests_UserId");

            migrationBuilder.AlterColumn<Guid>(
                name: "MosdositoSzemelyId",
                table: "Requests",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ModositasiIdo",
                table: "Requests",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Megjegyzes",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Users_UserId",
                table: "Requests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_UserId",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Requests",
                newName: "SzemelyId");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Requests",
                newName: "Datum");

            migrationBuilder.RenameIndex(
                name: "IX_Requests_UserId",
                table: "Requests",
                newName: "IX_Requests_SzemelyId");

            migrationBuilder.AlterColumn<Guid>(
                name: "MosdositoSzemelyId",
                table: "Requests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ModositasiIdo",
                table: "Requests",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Megjegyzes",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Users_SzemelyId",
                table: "Requests",
                column: "SzemelyId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
