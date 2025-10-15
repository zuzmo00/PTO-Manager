using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTO_Manager.Migrations
{
    /// <inheritdoc />
    public partial class RequestBlock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_UserId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ModificationTime",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ModifierUserUd",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "RequestNumber",
                table: "Requests",
                newName: "RequestBlockId");

            migrationBuilder.RenameColumn(
                name: "DepartmentNev",
                table: "Department",
                newName: "DepartmentName");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Requests",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateTable(
                name: "RequestBlocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ModifierUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationTime = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestBlocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestBlocks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestBlockId",
                table: "Requests",
                column: "RequestBlockId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestBlocks_UserId",
                table: "RequestBlocks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_RequestBlocks_RequestBlockId",
                table: "Requests",
                column: "RequestBlockId",
                principalTable: "RequestBlocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Users_UserId",
                table: "Requests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_RequestBlocks_RequestBlockId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_UserId",
                table: "Requests");

            migrationBuilder.DropTable(
                name: "RequestBlocks");

            migrationBuilder.DropIndex(
                name: "IX_Requests_RequestBlockId",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "RequestBlockId",
                table: "Requests",
                newName: "RequestNumber");

            migrationBuilder.RenameColumn(
                name: "DepartmentName",
                table: "Department",
                newName: "DepartmentNev");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Requests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ModificationTime",
                table: "Requests",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifierUserUd",
                table: "Requests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Requests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Users_UserId",
                table: "Requests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
