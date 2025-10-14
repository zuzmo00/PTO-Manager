using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTO_Manager.Migrations
{
    /// <inheritdoc />
    public partial class Refactored : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Administrators_Department_ReszlegId",
                table: "Administrators");

            migrationBuilder.DropForeignKey(
                name: "FK_Administrators_Users_SzemelyId",
                table: "Administrators");

            migrationBuilder.DropForeignKey(
                name: "FK_Remaining_Users_SzemelyId",
                table: "Remaining");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Department_ReszlegId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Preferenciak");

            migrationBuilder.DropColumn(
                name: "Megjegyzes",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "EddigKivett",
                table: "Remaining");

            migrationBuilder.RenameColumn(
                name: "Torzsszam",
                table: "Users",
                newName: "Employeeid");

            migrationBuilder.RenameColumn(
                name: "ReszlegId",
                table: "Users",
                newName: "DepartmentId");

            migrationBuilder.RenameColumn(
                name: "Nev",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "Jelszo",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_Users_ReszlegId",
                table: "Users",
                newName: "IX_Users_DepartmentId");

            migrationBuilder.RenameColumn(
                name: "Tipus",
                table: "Requests",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Statusz",
                table: "Requests",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "MosdositoSzemelyId",
                table: "Requests",
                newName: "ModifierUserUd");

            migrationBuilder.RenameColumn(
                name: "ModositasiIdo",
                table: "Requests",
                newName: "ModificationTime");

            migrationBuilder.RenameColumn(
                name: "KerelemSzam",
                table: "Requests",
                newName: "RequestNumber");

            migrationBuilder.RenameColumn(
                name: "SzemelyId",
                table: "Remaining",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "OsszesSzab",
                table: "Remaining",
                newName: "RemainingDays");

            migrationBuilder.RenameColumn(
                name: "Fuggoben",
                table: "Remaining",
                newName: "AllHoliday");

            migrationBuilder.RenameColumn(
                name: "Ev",
                table: "Remaining",
                newName: "Year");

            migrationBuilder.RenameIndex(
                name: "IX_Remaining_SzemelyId",
                table: "Remaining",
                newName: "IX_Remaining_UserId");

            migrationBuilder.RenameColumn(
                name: "Leiras",
                table: "Log",
                newName: "Descirption");

            migrationBuilder.RenameColumn(
                name: "Datum",
                table: "Log",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "ReszlegNev",
                table: "Department",
                newName: "DepartmentNev");

            migrationBuilder.RenameColumn(
                name: "Visszavonhat",
                table: "Administrators",
                newName: "CanRevoke");

            migrationBuilder.RenameColumn(
                name: "SzemelyId",
                table: "Administrators",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ReszlegId",
                table: "Administrators",
                newName: "DepartmentId");

            migrationBuilder.RenameColumn(
                name: "Kerhet",
                table: "Administrators",
                newName: "CanRequest");

            migrationBuilder.RenameColumn(
                name: "Biralhat",
                table: "Administrators",
                newName: "CanDecide");

            migrationBuilder.RenameIndex(
                name: "IX_Administrators_SzemelyId",
                table: "Administrators",
                newName: "IX_Administrators_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Administrators_ReszlegId",
                table: "Administrators",
                newName: "IX_Administrators_DepartmentId");

            migrationBuilder.CreateTable(
                name: "Preferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preferences", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Administrators_Department_DepartmentId",
                table: "Administrators",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Administrators_Users_UserId",
                table: "Administrators",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Remaining_Users_UserId",
                table: "Remaining",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Department_DepartmentId",
                table: "Users",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Administrators_Department_DepartmentId",
                table: "Administrators");

            migrationBuilder.DropForeignKey(
                name: "FK_Administrators_Users_UserId",
                table: "Administrators");

            migrationBuilder.DropForeignKey(
                name: "FK_Remaining_Users_UserId",
                table: "Remaining");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Department_DepartmentId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Preferences");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "Nev");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "Jelszo");

            migrationBuilder.RenameColumn(
                name: "Employeeid",
                table: "Users",
                newName: "Torzsszam");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Users",
                newName: "ReszlegId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_DepartmentId",
                table: "Users",
                newName: "IX_Users_ReszlegId");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Requests",
                newName: "Tipus");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Requests",
                newName: "Statusz");

            migrationBuilder.RenameColumn(
                name: "RequestNumber",
                table: "Requests",
                newName: "KerelemSzam");

            migrationBuilder.RenameColumn(
                name: "ModifierUserUd",
                table: "Requests",
                newName: "MosdositoSzemelyId");

            migrationBuilder.RenameColumn(
                name: "ModificationTime",
                table: "Requests",
                newName: "ModositasiIdo");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "Remaining",
                newName: "Ev");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Remaining",
                newName: "SzemelyId");

            migrationBuilder.RenameColumn(
                name: "RemainingDays",
                table: "Remaining",
                newName: "OsszesSzab");

            migrationBuilder.RenameColumn(
                name: "AllHoliday",
                table: "Remaining",
                newName: "Fuggoben");

            migrationBuilder.RenameIndex(
                name: "IX_Remaining_UserId",
                table: "Remaining",
                newName: "IX_Remaining_SzemelyId");

            migrationBuilder.RenameColumn(
                name: "Descirption",
                table: "Log",
                newName: "Leiras");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Log",
                newName: "Datum");

            migrationBuilder.RenameColumn(
                name: "DepartmentNev",
                table: "Department",
                newName: "ReszlegNev");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Administrators",
                newName: "SzemelyId");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Administrators",
                newName: "ReszlegId");

            migrationBuilder.RenameColumn(
                name: "CanRevoke",
                table: "Administrators",
                newName: "Visszavonhat");

            migrationBuilder.RenameColumn(
                name: "CanRequest",
                table: "Administrators",
                newName: "Kerhet");

            migrationBuilder.RenameColumn(
                name: "CanDecide",
                table: "Administrators",
                newName: "Biralhat");

            migrationBuilder.RenameIndex(
                name: "IX_Administrators_UserId",
                table: "Administrators",
                newName: "IX_Administrators_SzemelyId");

            migrationBuilder.RenameIndex(
                name: "IX_Administrators_DepartmentId",
                table: "Administrators",
                newName: "IX_Administrators_ReszlegId");

            migrationBuilder.AddColumn<string>(
                name: "Megjegyzes",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EddigKivett",
                table: "Remaining",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Preferenciak",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preferenciak", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Administrators_Department_ReszlegId",
                table: "Administrators",
                column: "ReszlegId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Administrators_Users_SzemelyId",
                table: "Administrators",
                column: "SzemelyId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Remaining_Users_SzemelyId",
                table: "Remaining",
                column: "SzemelyId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Department_ReszlegId",
                table: "Users",
                column: "ReszlegId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
