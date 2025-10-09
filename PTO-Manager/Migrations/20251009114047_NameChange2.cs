using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTO_Manager.Migrations
{
    /// <inheritdoc />
    public partial class NameChange2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FennmaradoNapok");

            migrationBuilder.DropTable(
                name: "Kerelmek");

            migrationBuilder.DropTable(
                name: "Ugyintezok");

            migrationBuilder.DropTable(
                name: "Szemelyek");

            migrationBuilder.DropTable(
                name: "Reszleg");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KulonlegesNapok",
                table: "KulonlegesNapok");

            migrationBuilder.RenameTable(
                name: "KulonlegesNapok",
                newName: "SpecialDays");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpecialDays",
                table: "SpecialDays",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReszlegNev = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nev = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Torzsszam = table.Column<int>(type: "int", nullable: false),
                    ReszlegId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Jelszo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Department_ReszlegId",
                        column: x => x.ReszlegId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Administrators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SzemelyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReszlegId = table.Column<int>(type: "int", nullable: false),
                    Kerhet = table.Column<bool>(type: "bit", nullable: false),
                    Biralhat = table.Column<bool>(type: "bit", nullable: false),
                    Visszavonhat = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Administrators_Department_ReszlegId",
                        column: x => x.ReszlegId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Administrators_Users_SzemelyId",
                        column: x => x.SzemelyId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Remaining",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SzemelyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ev = table.Column<DateOnly>(type: "date", nullable: false),
                    OsszeesSzab = table.Column<int>(type: "int", nullable: false),
                    EddigKivett = table.Column<int>(type: "int", nullable: false),
                    Fuggoben = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remaining", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Remaining_Users_SzemelyId",
                        column: x => x.SzemelyId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SzemelyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Statusz = table.Column<int>(type: "int", nullable: false),
                    Datum = table.Column<DateOnly>(type: "date", nullable: false),
                    KerelemSzam = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tipus = table.Column<int>(type: "int", nullable: false),
                    MosdositoSzemelyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModositasiIdo = table.Column<DateOnly>(type: "date", nullable: false),
                    Megjegyzes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_Users_SzemelyId",
                        column: x => x.SzemelyId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Administrators_ReszlegId",
                table: "Administrators",
                column: "ReszlegId");

            migrationBuilder.CreateIndex(
                name: "IX_Administrators_SzemelyId",
                table: "Administrators",
                column: "SzemelyId");

            migrationBuilder.CreateIndex(
                name: "IX_Remaining_SzemelyId",
                table: "Remaining",
                column: "SzemelyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_SzemelyId",
                table: "Requests",
                column: "SzemelyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ReszlegId",
                table: "Users",
                column: "ReszlegId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administrators");

            migrationBuilder.DropTable(
                name: "Remaining");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpecialDays",
                table: "SpecialDays");

            migrationBuilder.RenameTable(
                name: "SpecialDays",
                newName: "KulonlegesNapok");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KulonlegesNapok",
                table: "KulonlegesNapok",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Reszleg",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReszlegNev = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reszleg", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Szemelyek",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReszlegId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Jelszo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nev = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Torzsszam = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Szemelyek", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Szemelyek_Reszleg_ReszlegId",
                        column: x => x.ReszlegId,
                        principalTable: "Reszleg",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FennmaradoNapok",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SzemelyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EddigKivett = table.Column<int>(type: "int", nullable: false),
                    Ev = table.Column<DateOnly>(type: "date", nullable: false),
                    Fuggoben = table.Column<int>(type: "int", nullable: false),
                    OsszeesSzab = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FennmaradoNapok", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FennmaradoNapok_Szemelyek_SzemelyId",
                        column: x => x.SzemelyId,
                        principalTable: "Szemelyek",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kerelmek",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SzemelyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Datum = table.Column<DateOnly>(type: "date", nullable: false),
                    KerelemSzam = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Megjegyzes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModositasiIdo = table.Column<DateOnly>(type: "date", nullable: false),
                    MosdositoSzemelyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Statusz = table.Column<int>(type: "int", nullable: false),
                    Tipus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kerelmek", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kerelmek_Szemelyek_SzemelyId",
                        column: x => x.SzemelyId,
                        principalTable: "Szemelyek",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ugyintezok",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReszlegId = table.Column<int>(type: "int", nullable: false),
                    SzemelyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Biralhat = table.Column<bool>(type: "bit", nullable: false),
                    Kerhet = table.Column<bool>(type: "bit", nullable: false),
                    Visszavonhat = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ugyintezok", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ugyintezok_Reszleg_ReszlegId",
                        column: x => x.ReszlegId,
                        principalTable: "Reszleg",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ugyintezok_Szemelyek_SzemelyId",
                        column: x => x.SzemelyId,
                        principalTable: "Szemelyek",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FennmaradoNapok_SzemelyId",
                table: "FennmaradoNapok",
                column: "SzemelyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kerelmek_SzemelyId",
                table: "Kerelmek",
                column: "SzemelyId");

            migrationBuilder.CreateIndex(
                name: "IX_Szemelyek_ReszlegId",
                table: "Szemelyek",
                column: "ReszlegId");

            migrationBuilder.CreateIndex(
                name: "IX_Ugyintezok_ReszlegId",
                table: "Ugyintezok",
                column: "ReszlegId");

            migrationBuilder.CreateIndex(
                name: "IX_Ugyintezok_SzemelyId",
                table: "Ugyintezok",
                column: "SzemelyId");
        }
    }
}
